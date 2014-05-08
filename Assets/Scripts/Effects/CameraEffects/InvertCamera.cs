using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Camera))]

public class InvertCamera : MonoBehaviour {

  void Start () {
  }

  void OnPreCull () {
    camera.ResetWorldToCameraMatrix();
    camera.ResetProjectionMatrix();
    camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(new Vector3(1, -1, 1));
  }
 
  void OnPreRender () {
    GL.SetRevertBackfacing(true);
  }
 
  void OnPostRender () {
    GL.SetRevertBackfacing(false);
  }

}
