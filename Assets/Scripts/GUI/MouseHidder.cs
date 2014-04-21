using UnityEngine;
using System.Collections;

public class MouseHidder : MonoBehaviour {

  public Texture cursorTexture;

  private bool showMouse;

  void Start () {
    Screen.showCursor = false;
  }

  void Update () {
    if (GameState.state == GameState.PLAYING) {
      showMouse = false;
      Screen.lockCursor = true;
    } else {
      showMouse = true;
      Screen.lockCursor = false;
    }
  }

  void OnDestroy() {
    Screen.showCursor = true;
    Screen.lockCursor = false;
  }

  void OnGUI () {
    if (showMouse) {
      GUI.depth = 0;
      Vector3 mousePosition = Input.mousePosition;
      int width = Screen.width / 25;
      int height = width;
      GUI.DrawTexture(new Rect(mousePosition.x, Screen.height - mousePosition.y, width, height), cursorTexture);
    }
  }

}
