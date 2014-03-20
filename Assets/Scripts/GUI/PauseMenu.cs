using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

  private bool isPaused = false;

  void Update() {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      isPaused = !isPaused;
      if (isPaused) {
        Time.timeScale = 0.0001f;
      } else {
        Time.timeScale = 1;
      }
    }
  }

}
