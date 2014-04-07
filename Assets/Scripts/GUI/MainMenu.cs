using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;

  public Texture startButtonTexture;
  public Texture optionButtonTexture;
  public Texture exitButtonTexture;

  private SoundEffectManager soundEffectManager;

  void Start () {
    GameState.state = GameState.PLAYING;
    Time.timeScale = 1;

    soundEffectManager = GetComponent<SoundEffectManager>();
  }

  void OnGUI () {
    GUI.skin = skin;

    // Background
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height),  backgroundTexture);

    GUI.Label(new Rect(0, 0, 100, 100), "2014/4/6 21.00");

    int width = Screen.height - 100;
    int height = width;

    int buttonWidth = width / 3;
    int buttonHeight = height / 6;

    // Buttons
    GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));

    if (GUI.Button(new Rect((width - buttonWidth) / 2, 0 * (buttonHeight + 10) + height / 5, buttonWidth, buttonHeight), startButtonTexture)) {
      soundEffectManager.playButtonSound();
      Application.LoadLevel("Entrance");
    }

    if (GUI.Button(new Rect((width - buttonWidth) / 2, 1 * (buttonHeight + 10) + height / 5, buttonWidth, buttonHeight), optionButtonTexture)) {
      soundEffectManager.playButtonSound();
    }

    if (GUI.Button(new Rect((width - buttonWidth) / 2, 2 * (buttonHeight + 10) + height / 5, buttonWidth, buttonHeight), exitButtonTexture)) {
      soundEffectManager.playButtonSound();
      Application.Quit();
    }

    GUILayout.EndArea();
  }
}
