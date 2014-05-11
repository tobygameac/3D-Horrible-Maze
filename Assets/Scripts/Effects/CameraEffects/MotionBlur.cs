using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Camera))]

public class MotionBlur : MonoBehaviour {

  public float accumulation;
  public Shader blurShader;

  private static Texture2D renderTexture;
  private Material blurMaterial;

  void Start () {
    renderTexture = new Texture2D((int)camera.pixelWidth, (int)camera.pixelHeight, TextureFormat.RGB24, false);
    blurMaterial = new Material(blurShader);
  }

  void Update () {
    blurMaterial.SetTexture("_MainTex", renderTexture);
    blurMaterial.SetFloat("_FrameAccumulation", accumulation);
  }

  void OnPostRender() {
    fullScreenQuad(blurMaterial);
    renderTexture.Resize((int)camera.pixelWidth, (int)camera.pixelHeight, TextureFormat.RGB24, false);
    renderTexture.ReadPixels(new Rect(camera.pixelRect), 0, 0);
    renderTexture.Apply();
  }

  private static void fullScreenQuad(Material renderMaterial) {
    GL.PushMatrix();
    for (int i = 0; i < renderMaterial.passCount; i++) {
      renderMaterial.SetPass(i);
      GL.LoadOrtho();
      GL.Begin(GL.QUADS);
      GL.Color(new Color(1, 1, 1, 1));
      GL.MultiTexCoord(0, new Vector3(0, 0, 0));
      GL.Vertex3(0, 0, 0);
      GL.MultiTexCoord(0, new Vector3(0, 1, 0));
      GL.Vertex3(0, 1, 0);
      GL.MultiTexCoord(0, new Vector3(1, 1, 0));
      GL.Vertex3(1, 1, 0);
      GL.MultiTexCoord(0, new Vector3(1, 0, 0));
      GL.Vertex3(1, 0, 0);
      GL.End();
    }
    GL.PopMatrix();
  }
}

