using UnityEngine;
using System.Collections;

public class MouseHidder : MonoBehaviour {

  public Texture2D cursorTexture;

  void Update () {
    if (GameState.state == GameState.PLAYING || GameState.state == GameState.LOADING) {
      Screen.showCursor = false;
      Screen.lockCursor = true;
    } else {
      Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
      Screen.showCursor = true;
      Screen.lockCursor = false;
    }
  }

  void OnDestroy() {
    /*
    Screen.showCursor = true;
    Screen.lockCursor = false;
    */
  }

}
