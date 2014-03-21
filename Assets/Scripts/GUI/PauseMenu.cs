using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;

  public Texture optionButtonTexture;
  public Texture exitButtonTexture;

  public int buttonWidth = 100;
  public int buttonHeight = 50;

  void Update () {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      GameState.state = (GameState.state == GameState.PAUSING) ? GameState.PLAYING : GameState.PAUSING;
      if (GameState.state == GameState.PAUSING) {
        Time.timeScale = 0.0001f;
      } else {
        Time.timeScale = 1;
      }
    }
  }

  void OnGUI () {
    
    if (GameState.state != GameState.PAUSING) {
      return;
    }

    GUI.skin = skin;

    // Background
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height),  backgroundTexture);

    // Buttons
    GUILayout.BeginArea(new Rect(Screen.width / 2 - buttonWidth / 2, 200, 500, 500));

    if (GUI.Button(new Rect(0, (buttonHeight + buttonHeight / 2) * 1, buttonWidth, buttonHeight), optionButtonTexture)) {
      //audio.PlayOneShot(buttonSound);
    }

    if (GUI.Button(new Rect(0, (buttonHeight + buttonHeight / 2) * 2, buttonWidth, buttonHeight), exitButtonTexture)) {
      Application.LoadLevel("MainMenu");
    }

    GUILayout.EndArea();
  }

}
