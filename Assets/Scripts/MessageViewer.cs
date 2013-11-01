using UnityEngine;
using System.Collections;

public class MessageViewer : MonoBehaviour {

  private string message;
  private float remainTime;
  bool isViewing = false;

  void Update () {
    if (isViewing) {
      remainTime -= Time.deltaTime;
      if (remainTime <= 0) {
        isViewing = false;
      }
    }
  }

  void OnGUI () {
    if (isViewing) {
      GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 200, 30), message);
    }
  }

  public void viewMessage (string viewMessage, float viewTime = 0.5f) {
    message = viewMessage;
    remainTime = viewTime;
    isViewing = true;
  }

}
