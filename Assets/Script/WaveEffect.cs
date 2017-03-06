using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
public class WaveEffect : ImageEffectBase {

    PlayerController player;
    Camera cam;

    public AnimationCurve strengthCurve;
    public AnimationCurve lengthCurve;
    public AnimationCurve diminishCurve;

    public RenderTexture backgroundTexture;
    public Texture2D noiseTexture;
    public Texture2D layerTexture;
    public Texture2D backTexture;

    protected override void Start() {
        base.Start();

        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        cam = GetComponent<Camera>();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        var waveScrPos = cam.WorldToScreenPoint(player.transform.position);
        #if UNITY_EDITOR
        waveScrPos.y = Screen.height - waveScrPos.y;
        #elif UNITY_STANDALONE
        waveScrPos.y = Screen.height - waveScrPos.y;
        #endif

        float strength, length, diminish;
        strength = strengthCurve.Evaluate(player.timeSinceWave);
        length = lengthCurve.Evaluate(player.timeSinceWave);
        diminish = diminishCurve.Evaluate(player.timeSinceWave);

        material.SetVector("_WaveScreenPos", waveScrPos);
        material.SetVector("_WaveParams", new Vector4(strength, length, diminish, 0));
        material.SetTexture("_ScreenTex", backgroundTexture);
        material.SetTexture("_NoiseTex", noiseTexture);
        material.SetTexture("_LayerTex", layerTexture);
        material.SetTexture("_BackTex", backTexture);
        material.SetFloat("_CamDelta", transform.position.y / Screen.height);
        material.SetInt("_Dead", player.enabled ? 0 : 1);

        #if UNITY_EDITOR
        material.SetInt("_ReverseUV", 1);
        #elif UNITY_STANDALONE
        material.SetInt("_ReverseUV", 1);
        #endif

        Graphics.Blit(source, destination, material);
    }

}
