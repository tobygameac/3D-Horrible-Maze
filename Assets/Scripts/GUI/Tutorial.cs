using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;
  public Texture[] tutorialPictures;
  public Texture menuButtonTexture;
  public Texture previousButtonTexture;
  public Texture nextButtonTexture;
  public Texture finishButtonTexture;
  public Texture skipButtonTexture;

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

    if (index != 0) {
      if (GUI.Button(new Rect(width / 2 - buttonWidth - gap / 2, startY, buttonWidth, buttonHeight), previousButtonTexture)) {
        soundEffectManager.playFlipSound();
        --index;
      }
    }
    if (index < tutorialPictures.Length - 1) {
      if (GUI.Button(new Rect(width / 2 + gap / 2, startY, buttonWidth, buttonHeight), nextButtonTexture)) {
        soundEffectManager.playFlipSound();
        index++;
      }
    } else {
      if (GUI.Button(new Rect(width / 2 + gap / 2, startY, buttonWidth, buttonHeight), finishButtonTexture)) {
        soundEffectManager.playButtonSound();
        loadLevel();
      }
    }

    if (GUI.Button(new Rect(buttonWidth / 5, startY, buttonWidth, buttonHeight), menuButtonTexture)) {
      soundEffectManager.playButtonSound();
      Application.LoadLevel("MainMenu");
    }
    if (GUI.Button(new Rect(width - buttonWidth - buttonWidth / 5, startY, buttonWidth, buttonHeight), skipButtonTexture)) {
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
      loadingWithAnimation.loadLevelWithAnimation("EscapingOldCastle");
      break;
     case GameMode.INFINITE:
      TargetMenu.clearTarget(false);
      loadingWithAnimation.loadLevelWithAnimation("InfiniteOldCastle");
      break;
    }
  }
}
