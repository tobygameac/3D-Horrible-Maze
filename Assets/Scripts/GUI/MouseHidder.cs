using UnityEngine;
using System.Collections;

public class MouseHidder : MonoBehaviour {

  void Update () {
    if (GameState.state != GameState.PAUSING) {
      Screen.showCursor = false;
      Screen.lockCursor = true;
    } else {
      Screen.showCursor = true;
      Screen.lockCursor = false;
    }
  }

  void OnDestroy() {
    Screen.showCursor = true;
    Screen.lockCursor = false;
  }

}
