using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;

  public Texture startButtonTexture;
  public Texture optionButtonTexture;
  public Texture exitButtonTexture;

  public int buttonWidth = 100;
  public int buttonHeight = 50;

  void Start () {
    GameState.state = GameState.PLAYING;
    Time.timeScale = 1;
  }

  void OnGUI () {
    GUI.skin = skin;

    // Background
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height),  backgroundTexture);

    GUI.Label(new Rect(0, 0, 100, 100), "2014/2/17 7.00");

    // Buttons
    GUILayout.BeginArea(new Rect(Screen.width / 2 - buttonWidth / 2, 200, 500, 500));

    if (GUI.Button(new Rect(0, (buttonHeight + buttonHeight / 2) * 0, buttonWidth, buttonHeight), startButtonTexture)) {
      //audio.PlayOneShot(buttonSound);
      Application.LoadLevel("Entrance");
    }

    if (GUI.Button(new Rect(0, (buttonHeight + buttonHeight / 2) * 1, buttonWidth, buttonHeight), optionButtonTexture)) {
      //audio.PlayOneShot(buttonSound);
    }

    if (GUI.Button(new Rect(0, (buttonHeight + buttonHeight / 2) * 2, buttonWidth, buttonHeight), exitButtonTexture)) {
      //audio.PlayOneShot(buttonSound);
    }

    GUILayout.EndArea();
  }
}
