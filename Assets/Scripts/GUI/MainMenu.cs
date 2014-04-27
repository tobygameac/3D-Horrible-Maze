using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;
  public Texture startButtonTexture;
  public Texture rankButtonTexture;
  public Texture optionButtonTexture;
  public Texture easyButtonTexture;
  public Texture hardButtonTexture;
  public Texture returnButtonTexture;
  public Texture exitButtonTexture;
  private bool isChoosingDifficulty;

  private SoundEffectManager soundEffectManager;


  void Start () {
    GameState.state = GameState.MENUVIEWING;
    soundEffectManager = GetComponent<SoundEffectManager>();
    isChoosingDifficulty = false;
  }

  void OnGUI () {
    GUI.skin = skin;

    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

    GUI.Label(new Rect(0, 0, 100, 100), "2014/4/26 9.00");

    int width = Screen.height - 100;
    int height = width;

    int buttonWidth = width / 3;
    int buttonHeight = height / 6;

    // Buttons
    GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));

    int startX = (width - buttonWidth) / 2;
    int startY = height / 5;

    if (isChoosingDifficulty) {
      if (GUI.Button(new Rect(startX, 0 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), easyButtonTexture)) {
        soundEffectManager.playButtonSound();
        GameState.difficulty = 0;
        Application.LoadLevel("Tutorial");
      }

      if (GUI.Button(new Rect(startX, 1 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), hardButtonTexture)) {
        soundEffectManager.playButtonSound();
        GameState.difficulty = 1;
        Application.LoadLevel("OldCastle");
      }

      if (GUI.Button(new Rect(startX, 3 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), returnButtonTexture)) {
        soundEffectManager.playButtonSound();
        isChoosingDifficulty = false;
      }
    } else {
      if (GUI.Button(new Rect(startX, 0 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), startButtonTexture)) {
        soundEffectManager.playButtonSound();
        isChoosingDifficulty = true;
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
    }

    GUILayout.EndArea();
  }
}
