using UnityEngine;
using System.Collections;

public class Rank : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;
  public Texture rankBackgroundTexture;
  public Texture easyTexture;
  public Texture hardTexture;
  public Texture returnButtonTexture;

  private string rankUrl = "http://134.208.43.1:5631/3DhorribleMaze/rank.php";
  private string rankHardUrl = "http://134.208.43.1:5631/3DhorribleMaze/rank%20-%20hard.php";

  private string text;
  private string textHard;

  private SoundEffectManager soundEffectManager;

  void Start () {
    GameState.state = GameState.MENUVIEWING;
    soundEffectManager = GetComponent<SoundEffectManager>();
    soundEffectManager.adjustSound();
    
    StartCoroutine(GetScores());
  }

  void OnGUI () {

    GUI.skin = skin;

    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

    int width = Screen.height - 100;
    int height = width;

    int buttonWidth = width / 3;
    int buttonHeight = height / 6;

    GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));

    GUI.DrawTexture(new Rect(0, 0, width, height), rankBackgroundTexture);

    int startX = (width - buttonWidth) / 2;
    int startY = height / 5;

    if (GUI.Button(new Rect(startX, 3 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), returnButtonTexture)) {
      soundEffectManager.playButtonSound();
      Application.LoadLevel("MainMenu");
    }

    int textWidth = width / 4;
    int textHeight = height / 2;
    Color originalColor = GUI.color;
    GUI.color = Color.black;
    GUI.Label(new Rect(width / 6, height / 3, textWidth, textHeight), text);
    GUI.Label(new Rect(width / 6 + textWidth + width / 6, height / 3, textWidth, textHeight), textHard);
    GUI.color = originalColor;
    int difficultyTextureWidth = width / 4;
    int difficultyTextureHeight = width / 8;
    GUI.DrawTexture(new Rect(width / 7, height / 4.5f, difficultyTextureWidth, difficultyTextureHeight), easyTexture);
    GUI.DrawTexture(new Rect(width / 7 + textWidth + width / 7, height / 4.5f, difficultyTextureWidth, difficultyTextureHeight), hardTexture);

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
