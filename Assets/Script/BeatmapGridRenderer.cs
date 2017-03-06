using UnityEngine;
using System.Collections;

/// <summary>
/// This class renders debug grid within game screen.
/// </summary>
[ExecuteInEditMode]
public class BeatmapGridRenderer : MonoBehaviour {

    public float GridSize = 2;

    public bool drawLine = false;

    static Material lineMaterial;
    
    static void CreateLineMaterial() {
        if (!lineMaterial) {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    PlayerController player;

    void Start() {
        player = PlayerController.instance;
    }

    void Update() {
        var viewOffset = GridSize * (player.time + player.timeSinceLastBeat / 
        Mathf.Max(0.01f, player.beatLength));
        transform.position = new Vector3(0, -viewOffset, 0);
    }

    // Will be called after all regular rendering is done
    public void OnRenderObject() {
        if (!drawLine || Camera.current.name != "Player Camera") {
            return;
        }

        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);

        var pos = transform.position;
        float xBegin = ((int) ((pos.x - 10) / GridSize)) * GridSize - GridSize * 0.5f;
        float yBegin = GridSize * (1 - player.timeSinceLastBeat / player.beatLength);
        GL.PushMatrix();

        GL.Begin(GL.QUADS);
        var c = 0.3f;
        GL.Color(new Color(c, c, c, 0.3f));
        for (int i = 0; i < 100; ++i) {
            LineHoriz(xBegin, xBegin + 50, yBegin + i * GridSize, 0.05f);
        }
        for (int i = 0; i < 100; ++i) {
            LineVert(yBegin, yBegin + 50, xBegin + i * GridSize, 0.05f);
        }
        GL.End();

        GL.PopMatrix();
    }

    void LineHoriz(float xbeg, float xend, float y, float w) {
        var hw = w / 2;
        GL.Vertex3(xbeg, y-hw, 0);
        GL.Vertex3(xend, y-hw, 0);
        GL.Vertex3(xend, y+hw, 0);
        GL.Vertex3(xbeg, y+hw, 0);
    }

    void LineVert(float ybeg, float yend, float x, float w) {
        var hw = w / 2;
        GL.Vertex3(x - hw, ybeg, 0);
        GL.Vertex3(x + hw, ybeg, 0);
        GL.Vertex3(x + hw, yend, 0);
        GL.Vertex3(x - hw, yend, 0);
    }


}
