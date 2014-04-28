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

  private SoundEffectManager soundEffectManager;

  void Start () {
    GameState.state = GameState.MENUVIEWING;
    index = 0;

    soundEffectManager = GetComponent<SoundEffectManager>();
    soundEffectManager.adjustSound();
  }

  void OnGUI () {
    GUI.skin = skin;

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
        Application.LoadLevel("OldCastle");
      }
    }

    if (GUI.Button(new Rect(buttonWidth / 5, startY, buttonWidth, buttonHeight), menuButtonTexture)) {
      soundEffectManager.playButtonSound();
      Application.LoadLevel("MainMenu");
    }
    if (GUI.Button(new Rect(width - buttonWidth - buttonWidth / 5, startY, buttonWidth, buttonHeight), skipButtonTexture)) {
      soundEffectManager.playButtonSound();
      Application.LoadLevel("OldCastle");
    }

    GUI.color = originalColor;
    
    GUILayout.EndArea();
  }
}
