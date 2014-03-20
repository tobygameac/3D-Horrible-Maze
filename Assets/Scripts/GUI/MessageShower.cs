using UnityEngine;
using System.Collections;

public class MessageShower : MonoBehaviour {

  private static string message;
  private static float remainTime;
  private static bool isShowing = false;

  void Update () {
    if (isShowing) {
      remainTime -= Time.deltaTime;
      if (remainTime <= 0) {
        isShowing = false;
      }
    }
  }

  void OnGUI () {
    if (isShowing) {
      GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 200, 30), message);
    }
  }

  public static void showMessage (string newMessage, float showTime = 0.5f) {
    message = newMessage;
    remainTime = showTime;
    isShowing = true;
  }

}
