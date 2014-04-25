using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;
  public Texture startButtonTexture;
  public Texture rankButtonTexture;
  public Texture optionButtonTexture;
  public Texture exitButtonTexture;

  private SoundEffectManager soundEffectManager;

  void Start () {
    GameState.state = GameState.MENUVIEWING;
    soundEffectManager = GetComponent<SoundEffectManager>();
  }

  void OnGUI () {
    GUI.skin = skin;

    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height),  backgroundTexture);

    GUI.Label(new Rect(0, 0, 100, 100), "2014/4/25 9.00");

    int width = Screen.height - 100;
    int height = width;

    int buttonWidth = width / 3;
    int buttonHeight = height / 6;

    // Buttons
    GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));

    int startX = (width - buttonWidth) / 2;
    int startY = height / 5;

    if (GUI.Button(new Rect(startX, 0 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), startButtonTexture)) {
      soundEffectManager.playButtonSound();
      Application.LoadLevel("Tutorial");
    }

    if (GUI.Button(new Rect(startX, 1 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), rankButtonTexture)) {
      soundEffectManager.playButtonSound();
      Application.LoadLevel("Rank");
    }

    if (GUI.Button(new Rect(startX, 2 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), optionButtonTexture)) {
      soundEffectManager.playButtonSound();
    }

    if (GUI.Button(new Rect(startX, 3 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), exitButtonTexture)) {
      soundEffectManager.playButtonSound();
      Application.Quit();
    }

    GUILayout.EndArea();
  }
}
