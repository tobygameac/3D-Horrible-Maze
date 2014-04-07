using UnityEngine;
using System.Collections;

public class MessageViewer : MonoBehaviour {

  public Texture messageBackgroundTexture;

  private static string message;
  private static float remainTime;
  private static bool isShowing = false;
  private static bool isErrorMessage = false;

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
      int width = Screen.height / 2;
      int height = Screen.height / 4;
      GUI.DrawTexture(new Rect((Screen.width - width) / 2, Screen.height - height * 2, width, height), messageBackgroundTexture);
      Color originalColor = GUI.color;
      if (isErrorMessage) {
        GUI.color = Color.red;
      } else {
        GUI.color = Color.black;
      }
      GUI.Label(new Rect((Screen.width - width) / 2 + width / 5, Screen.height - height * 2 + height / 3, width, height), message);
      GUI.color = originalColor;
    }
  }

  public static void showMessage (string newMessage, float showTime = 0.5f) {
    message = newMessage;
    remainTime = showTime;
    isShowing = true;
    isErrorMessage = false;
  }

  public static void showErrorMessage (string newMessage, float showTime = 0.5f) {
    message = newMessage;
    remainTime = showTime;
    isShowing = true;
    isErrorMessage = true;
  }

}
