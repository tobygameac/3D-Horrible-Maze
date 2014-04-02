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

      switch (GameState.state) {
        case GameState.PAUSING:
          GameState.state = GameState.PLAYING;
          Time.timeScale = 1;
          break;
        case GameState.PLAYING:
          GameState.state = GameState.PAUSING;
          Time.timeScale = 0.0001f;
          break;
        case GameState.SKILLVIEWING:
          GameState.state = GameState.PLAYING;
          Time.timeScale = 1;
          break;
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
