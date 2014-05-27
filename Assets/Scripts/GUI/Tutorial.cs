using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;
  public Texture[] escapingTutorialPictures;
  public Texture[] infiniteTutorialPictures;
  private Texture[] tutorialPictures;

  private int index;

  private bool almostFinished;

  private SoundEffectManager soundEffectManager;

  private LoadingWithAnimation loadingWithAnimation;

  void Start () {
    GameState.state = GameState.MENU_VIEWING;
    index = 0;

    almostFinished = false;

    soundEffectManager = GetComponent<SoundEffectManager>();
    soundEffectManager.adjustSound();

    loadingWithAnimation = GameObject.FindWithTag("Main").GetComponent<LoadingWithAnimation>();

    switch (GameMode.mode) {
     case GameMode.ESCAPING:
      tutorialPictures = escapingTutorialPictures;
      break;
     case GameMode.INFINITE:
      tutorialPictures = infiniteTutorialPictures;
      break;
    }
  }

  void OnGUI () {

    if (almostFinished) {
      GUI.color = new Color(0, 0, 0);
      GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);
      return;
    }

    if (GameState.state == GameState.LOADING) {
      almostFinished = loadingWithAnimation.almostFinished();
    }

    GUI.skin = skin;

    GUI.depth = 1;

    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

    int width = Screen.height - 100;
    int height = width;

    GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));

    GUI.DrawTexture(new Rect(0, 0, width, height), tutorialPictures[index]);

    int buttonWidth = width / 5;
    int buttonHeight = height / 10;

    int startY = height - buttonHeight - buttonHeight / 2;

    Color originalColor = GUI.color;
    GUI.color = Color.black;

    int gap = buttonWidth / 15;

    GUI.skin.button.fontSize = (Screen.width + Screen.height) / 75;

    if (index != 0) {
      if (GUI.Button(new Rect(width / 2 - buttonWidth - gap / 2, startY, buttonWidth, buttonHeight), "Previous")) {
        soundEffectManager.playFlipSound();
        --index;
      }
    }
    if (index < tutorialPictures.Length - 1) {
      if (GUI.Button(new Rect(width / 2 + gap / 2, startY, buttonWidth, buttonHeight), "Next")) {
        soundEffectManager.playFlipSound();
        index++;
      }
    } else {
      if (GUI.Button(new Rect(width / 2 + gap / 2, startY, buttonWidth, buttonHeight), "Finish")) {
        soundEffectManager.playButtonSound();
        loadLevel();
      }
    }

    GUI.skin.button.fontSize = (Screen.width + Screen.height) / 50;

    if (GUI.Button(new Rect(buttonWidth / 5, startY, buttonWidth, buttonHeight), "Menu")) {
      soundEffectManager.playButtonSound();
      Application.LoadLevel("MainMenu");
    }
    if (GUI.Button(new Rect(width - buttonWidth - buttonWidth / 5, startY, buttonWidth, buttonHeight), "Skip")) {
      soundEffectManager.playButtonSound();
      loadLevel();
    }

    GUI.color = originalColor;
    
    GUILayout.EndArea();
  }

  void loadLevel () {
    switch (GameMode.mode) {
     case GameMode.ESCAPING:
      TargetMenu.clearTarget(false);
      if (GameState.escapingDemo) {
        loadingWithAnimation.loadLevelWithAnimation("EscapingDemo");
      } else {
        loadingWithAnimation.loadLevelWithAnimation("EscapingOldCastle");
      }
      break;
     case GameMode.INFINITE:
      TargetMenu.clearTarget(false);
      loadingWithAnimation.loadLevelWithAnimation("InfiniteOldCastle");
      break;
    }
  }
}
