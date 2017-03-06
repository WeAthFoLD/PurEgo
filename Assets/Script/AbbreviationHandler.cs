using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class AbbreviationHandler : MonoBehaviour {
    PlayerController player;
    VignetteAndChromaticAberration vca;

    public AnimationCurve samplingCurve;

    void Start () {
        player = PlayerController.instance;
        vca = GetComponent<VignetteAndChromaticAberration>();
    }
	
    void Update () {
        var modifier = player.time % player.waveInterval == 0 ? 1.5f : 1f;
        var abbr = samplingCurve.Evaluate(player.timeSinceLastBeat) * modifier;
        vca.chromaticAberration = abbr;
    }

}
