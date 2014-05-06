using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

  public GUISkin bloodOnHoverSkin;
  public GUISkin nullOnHoverSkin;

  public Texture backgroundTexture;
  public Texture startButtonTexture;
  public Texture rankButtonTexture;
  public Texture optionButtonTexture;
  public Texture easyButtonTexture;
  public Texture hardButtonTexture;
  public Texture returnButtonTexture;
  public Texture exitButtonTexture;
  public Texture soundIconTexture;

  private static System.Random random = new System.Random(); // Only need one random seed

  private bool isAdjustingOption;
  private bool isChoosingDifficulty;

  private SoundEffectManager soundEffectManager;

  private static bool first = true;

  void Start () {
    if (first) {
      GameState.volume = 0.5f;
      first = false;
    }
    GameState.state = GameState.MENU_VIEWING;
    isAdjustingOption = false;
    isChoosingDifficulty = false;
    soundEffectManager = GetComponent<SoundEffectManager>();
    soundEffectManager.adjustSound();
  }

  void OnGUI () {

    GUI.skin = bloodOnHoverSkin;

    //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

    GUI.Label(new Rect(0, 0, 100, 100), "2014/5/5 9.00");

    int width = Screen.height - 100;
    int height = width;

    int buttonWidth = width / 3;
    int buttonHeight = height / 6;

    // Buttons
    GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));

    int startX = (width - buttonWidth) / 2;
    int startY = height / 5;

    if (isAdjustingOption) {

      int soundIconWidth = buttonHeight;
      int soundIconHeight = buttonHeight;

      Color originalColor = GUI.color;
      if (GameState.volume == 0) {
        GUI.color = Color.grey;
      }

      GUI.skin = nullOnHoverSkin;
      if (GUI.Button(new Rect(startX - soundIconWidth / 2 , 2 * (buttonHeight + 10) + startY - soundIconHeight / 2.5f, soundIconWidth, soundIconHeight), soundIconTexture)) {
        GameState.volume = 0;
      }
      GUI.skin = bloodOnHoverSkin;

      GUI.color = originalColor;

      GameState.volume = GUI.HorizontalScrollbar(new Rect(startX + soundIconWidth / 2, 2 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), GameState.volume, 0.01f, 0, 1);
      soundEffectManager.adjustSound();

      if (GUI.Button(new Rect(startX, 3 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), returnButtonTexture)) {
        soundEffectManager.playButtonSound();
        isAdjustingOption = false;
      }
    } else if (isChoosingDifficulty) {
      if (GUI.Button(new Rect(startX, 0 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Escaping Mode")) {
        soundEffectManager.playButtonSound();
        GameState.difficulty = random.Next(2);
        GameState.userStudy = true;
        GameMode.mode = GameMode.ESCAPING;
        Application.LoadLevel("Tutorial");
      }

      if (GUI.Button(new Rect(startX, 1 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Infinite Mode")) {
        soundEffectManager.playButtonSound();
        GameState.difficulty = random.Next(2);
        GameState.userStudy = true;
        GameMode.mode = GameMode.INFINITE;
        Application.LoadLevel("Tutorial");
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
        isAdjustingOption = true;
      }

      if (GUI.Button(new Rect(startX, 3 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), exitButtonTexture)) {
        soundEffectManager.playButtonSound();
        Application.Quit();
      }
    }

    GUILayout.EndArea();
  }
}
