using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;
  public Texture pauseMenuBackgroundTexture;

  public Texture optionButtonTexture;
  public Texture exitButtonTexture;

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
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

    // Menu background
    int width = Screen.height - 100;
    int height = width;

    int buttonWidth = width / 3;
    int buttonHeight = height / 6;

    GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));

    GUI.DrawTexture(new Rect(0, 0, width, height), pauseMenuBackgroundTexture);

    if (GUI.Button(new Rect((width - buttonWidth) / 2, 1 * (buttonHeight + 10) + height / 7, buttonWidth, buttonHeight), optionButtonTexture)) {
      //audio.PlayOneShot(buttonSound);
    }

    if (GUI.Button(new Rect((width - buttonWidth) / 2, 2 * (buttonHeight + 10) + height / 7, buttonWidth, buttonHeight), exitButtonTexture)) {
      Application.LoadLevel("MainMenu");
    }

    GUILayout.EndArea();
  }

}
