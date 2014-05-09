using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

  public GUISkin titleSkin;
  public GUISkin bloodOnHoverSkin;
  public GUISkin nullOnHoverSkin;

  public Texture soundIconTexture;

  private static System.Random random = new System.Random(); // Only need one random seed

  private bool isAdjustingOption;
  private bool isChoosingDifficulty;

  private SoundEffectManager soundEffectManager;

  private static bool first = true;

  private bool waitForPressKey;
  private float alpha;

  void Start () {
    Time.timeScale = 1;
    if (first) {
      GameState.volume = 0.5f;
      first = false;
      waitForPressKey = true;
    }
    GameState.state = GameState.MENU_VIEWING;
    isAdjustingOption = false;
    isChoosingDifficulty = false;
    soundEffectManager = GetComponent<SoundEffectManager>();
    soundEffectManager.adjustSound();
    alpha = 0;
  }

  void Update () {
    if (!waitForPressKey && alpha < 1) {
      alpha += Time.deltaTime;
      if (alpha > 1) {
        alpha = 1;
      }
    }
  }

  void OnGUI () {
    GUI.skin = titleSkin;
    GUI.skin.label.fontSize = (Screen.width + Screen.height) / 8;
    float titleWidth = Screen.width * 0.5f;
    float titleHeight = Screen.height * 0.3f;
    GUI.color = new Color(0.3f, 0, 0);
    GUI.Label(new Rect((Screen.width - titleWidth) / 2, 0, titleWidth, titleHeight), "Corridor");

    GUI.color = new Color(0.65f, 0, 0);
    Color originalColor = GUI.color;

    //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

    GUI.skin = bloodOnHoverSkin;
    GUI.skin.label.fontSize = (Screen.width + Screen.height) / 25;
    GUI.skin.button.fontSize = (Screen.width + Screen.height) / 25;
    GUI.Label(new Rect(0, Screen.height * 0.8f, Screen.width, Screen.height), "2014/5/8 9.00");

    if (waitForPressKey) {
      float messageAlpha = Mathf.Abs(50 - (int)(Time.time * 100) % 100) / 100.0f;
      GUI.color = new Color(0.65f, 0, 0, messageAlpha);
      GUI.skin.label.fontSize = (Screen.width + Screen.height) / 35;
      float messageWidth = Screen.width / 2.6f;
      float messageHeight = Screen.height / 5;
      GUI.Label(new Rect((Screen.width - messageWidth) / 2, Screen.height * 0.7f, messageWidth, messageHeight), "Press any key to continue.");
      if (Event.current.type == EventType.KeyDown || Event.current.type == EventType.MouseDown) {
        waitForPressKey = false;
        alpha = 0;
      }
      return;
    }

    GUI.color = new Color(0.65f, 0, 0, alpha);

    float width = Screen.height - 100;
    float height = width;

    float buttonWidth = width / 2.5f;
    float buttonHeight = height / 6.0f;

    // Buttons
    GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));

    float startX = (width - buttonWidth) / 2.0f;
    float startY = height / 4.0f;

    if (isAdjustingOption) {

      float soundIconWidth = buttonHeight;
      float soundIconHeight = buttonHeight;

      if (GameState.volume == 0) {
        GUI.color = new Color(0.5f, 0.5f, 0.5f, alpha);
      } else {
        GUI.color = new Color(1.0f, 1.0f, 1.0f, alpha);
      }
      GUI.skin = nullOnHoverSkin;
      if (GUI.Button(new Rect(startX - soundIconWidth / 2 , 2 * (buttonHeight + 10) + startY - soundIconHeight / 2.5f, soundIconWidth, soundIconHeight), soundIconTexture)) {
        GameState.volume = 0;
      }
      GUI.skin = bloodOnHoverSkin;
      GUI.color = new Color(1.0f, 1.0f, 1.0f, alpha);
      GameState.volume = GUI.HorizontalScrollbar(new Rect(startX + soundIconWidth / 2, 2 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), GameState.volume, 0.01f, 0, 1);
      soundEffectManager.adjustSound();
      GUI.color = originalColor;
      if (GUI.Button(new Rect(startX, 3 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Return")) {
        soundEffectManager.playButtonSound();
        isAdjustingOption = false;
        alpha = 0;
      }

    } else if (isChoosingDifficulty) {
      if (GUI.Button(new Rect(startX, 0 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Escaping")) {
        soundEffectManager.playButtonSound();
        GameState.difficulty = random.Next(2);
        GameState.userStudy = true;
        GameMode.mode = GameMode.ESCAPING;
        Application.LoadLevel("Tutorial");
      }

      if (GUI.Button(new Rect(startX, 1 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Infinite")) {
        soundEffectManager.playButtonSound();
        GameState.difficulty = random.Next(2);
        GameState.userStudy = true;
        GameMode.mode = GameMode.INFINITE;
        Application.LoadLevel("Tutorial");
      }

      if (GUI.Button(new Rect(startX, 3 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Return")) {
        soundEffectManager.playButtonSound();
        isChoosingDifficulty = false;
        alpha = 0;
      }

    } else {
      if (GUI.Button(new Rect(startX, 0 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Start")) {
        soundEffectManager.playButtonSound();
        isChoosingDifficulty = true;
        alpha = 0;
      }

      if (GUI.Button(new Rect(startX, 1 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Rank")) {
        soundEffectManager.playButtonSound();
        Application.LoadLevel("Rank");
      }

      if (GUI.Button(new Rect(startX, 2 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Option")) {
        soundEffectManager.playButtonSound();
        isAdjustingOption = true;
        alpha = 0;
      }

      if (GUI.Button(new Rect(startX, 3 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Exit")) {
        soundEffectManager.playButtonSound();
        Application.Quit();
      }

    }
    GUILayout.EndArea();
  }
}
