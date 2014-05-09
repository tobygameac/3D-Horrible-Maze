using UnityEngine;
using System.Collections;

public class Rank : MonoBehaviour {

  public GUISkin bloodOnHoverSkin;
  public GUISkin titleSkin;

  public Texture backgroundTexture;
  public Texture rankBackgroundTexture;

  private string rankUrl = "http://134.208.43.1:5631/3DhorribleMaze/rank.php";
  private string rankHardUrl = "http://134.208.43.1:5631/3DhorribleMaze/rank%20-%20hard.php";

  private string text;
  private string textHard;

  private SoundEffectManager soundEffectManager;

  void Start () {
    GameState.state = GameState.MENU_VIEWING;
    soundEffectManager = GetComponent<SoundEffectManager>();
    soundEffectManager.adjustSound();
    
    StartCoroutine(GetScores());
  }

  void OnGUI () {

    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

    int width = Screen.height - 100;
    int height = width;

    int buttonWidth = width / 3;
    int buttonHeight = height / 6;

    GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));

    GUI.DrawTexture(new Rect(0, 0, width, height), rankBackgroundTexture);

    GUI.color = new Color(0.7f, 0, 0);
    Color originalColor = GUI.color;

    GUI.skin = titleSkin;
    GUI.skin.label.fontSize = (Screen.width + Screen.height) / 15;
    float titleWidth = width / 2.0f;
    float titleHeight = height / 4.0f;
    GUI.color = new Color(0.3f, 0, 0);
    GUI.Label(new Rect((width - titleWidth) / 2, 0, titleWidth, titleHeight), "Rank");
    GUI.color = originalColor;

    GUI.skin = bloodOnHoverSkin;
    GUI.skin.label.fontSize = (Screen.width + Screen.height) / 75;
    GUI.skin.button.fontSize = (Screen.width + Screen.height) / 75;

    int startX = (width - buttonWidth) / 2;
    int startY = height / 5;

    GUI.skin.button.fontSize = (Screen.width + Screen.height) / 30;
    if (GUI.Button(new Rect(startX, 3.5f * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), "Menu")) {
      soundEffectManager.playButtonSound();
      Application.LoadLevel("MainMenu");
    }

    float textWidth = width / 3.5f;
    float textHeight = height / 1.8f;
    GUI.color = Color.black;
    GUI.Label(new Rect(width / 6, height / 3, textWidth, textHeight), text);
    GUI.Label(new Rect(width / 6 + textWidth + width / 6, height / 3, textWidth, textHeight), textHard);
    GUI.color = originalColor;
    int difficultyTextureWidth = width / 4;
    int difficultyTextureHeight = width / 8;
    GUI.skin.label.fontSize = (Screen.width + Screen.height) / 30;
    GUI.Label(new Rect(width / 6.0f, height / 4.5f, difficultyTextureWidth, difficultyTextureHeight), "Easy");
    GUI.Label(new Rect(width / 6.0f + textWidth + width / 7, height / 4.5f, difficultyTextureWidth, difficultyTextureHeight), "Hard");

    GUILayout.EndArea();
  }

  private IEnumerator GetScores() {
    text = "Loading...";
    WWW rankHS = new WWW(rankUrl);
    yield return rankHS;
    WWW rankHardHS = new WWW(rankHardUrl);
    yield return rankHardHS;
    if (rankHS.error != null && rankHardHS != null) {
      print("There was an error getting the rank : " + rankHS.error);
      print("There was an error getting the rank hard : " + rankHardHS.error);
    } else {
      text = rankHS.text;
      textHard = rankHardHS.text;
    }
  }
}
