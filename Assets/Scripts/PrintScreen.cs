using UnityEngine;
using System.Collections;

public class PrintScreen : MonoBehaviour {

  public int scale;

  void Start () {
    if (scale <= 0) {
      scale = 1;
    }
  }

  void Update () {
    if (Input.GetKeyDown(KeyCode.P)) {
      string filename = "Screenshot " + System.DateTime.Now.ToString("MMddyyyy-hhmmss") + ".png";
      print(filename);
      Application.CaptureScreenshot(filename, scale);
    }
  }
}
