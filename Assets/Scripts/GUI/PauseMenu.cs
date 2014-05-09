using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

  public GUISkin bloodOnHoverSkin;
  public GUISkin nullOnHoverSkin;
  public GUISkin titleSkin;

  public Texture backgroundTexture;
  public Texture pauseMenuBackgroundTexture;

  public Texture soundIconTexture;

  private bool isAdjustingOption;

  private SoundEffectManager soundEffectManager;

  void Start () {
    isAdjustingOption = false;
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
  }

  void Update () {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      switch (GameState.state) {
        case GameState.PAUSING:
          GameState.state = GameState.PLAYING;
          Time.timeScale = 1;
          isAdjustingOption = false;
          break;
        case GameState.PLAYING:
          GameState.state = GameState.PAUSING;
          Time.timeScale = 0.0001f;
          break;
        case GameState.SKILL_VIEWING:
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

    GUI.depth = 0;

    // Background
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

    // Menu background
    float width = Screen.height - 100;
    float height = width;

    float buttonWidth = width / 2.5f;
    float buttonHeight = height / 6.0f;

    // Buttons
    GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));

    GUI.DrawTexture(new Rect(0, 0, width, height), pauseMenuBackgroundTexture);

    GUI.color = new Color(0.65f, 0, 0);
    Color originalColor = GUI.color;

    GUI.skin = titleSkin;
    GUI.skin.label.fontSize = (Screen.width + Screen.height) / 15;
    GUI.color = new Color(0.3f, 0, 0);
    float titleWidth = width / 2.0f;
    float titleHeight = height / 4.0f;
    GUI.Label(new Rect((width - titleWidth) / 2, 0, titleWidth, titleHeight), "Pause");
    GUI.color = originalColor;

    GUI.skin = bloodOnHoverSkin;
    GUI.skin.button.fontSize = (Screen.width + Screen.height) / 25;
    float startY = height / 4.0f;

    if (isAdjustingOption) {

      float soundIconWidth = buttonHeight;
      float soundIconHeight = buttonHeight;

      if (GameState.volume == 0) {
        GUI.color = Color.grey;
      } else {
        GUI.color = Color.white;
      }

      GUI.skin = nullOnHoverSkin;
      if (GUI.Button(new Rect((width - buttonWidth) / 2 - soundIconWidth / 2 , 1 * (buttonHeight + 10) + startY - soundIconHeight / 2.5f, soundIconWidth, soundIconHeight), soundIconTexture)) {
        GameState.volume = 0;
      }
      GUI.skin = bloodOnHoverSkin;
      GUI.color = Color.white;
      GameState.volume = GUI.HorizontalScrollbar(new Rect((width - buttonWidth) / 2 + soundIconWidth / 2, 1 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), GameState.volume, 0.01f, 0, 1);
      soundEffectManager.adjustSound();
      GUI.color = originalColor;
      if (GUI.Button(new Rect((width - buttonWidth) / 2, 2 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Resume")) {
        soundEffectManager.playButtonSound();
        isAdjustingOption = false;
      }
    } else {
      if (GUI.Button(new Rect((width - buttonWidth) / 2, 0 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Option")) {
        soundEffectManager.playButtonSound();
        isAdjustingOption = true;
      }

      if (GUI.Button(new Rect((width - buttonWidth) / 2, 1 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Resume")) {
        soundEffectManager.playButtonSound();
        GameState.state = GameState.PLAYING;
        Time.timeScale = 1;
        isAdjustingOption = false;
      }

      if (GUI.Button(new Rect((width - buttonWidth) / 2, 2.5f * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Exit")) {
        soundEffectManager.playButtonSound();
        Application.LoadLevel("MainMenu");
      }
    }

    GUILayout.EndArea();
  }

}
