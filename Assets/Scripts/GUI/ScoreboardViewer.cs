using UnityEngine;
using System.Collections;

public class ScoreboardViewer : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;
  public Texture scoreboardBackgroundTexture;
  public Texture returnButtonTexture;

  private string scoreboardUrl = "http://134.208.43.1:5631/3DhorribleMaze/scoreboard.php";

  private string text;

  private SoundEffectManager soundEffectManager;

  void Start () {
    soundEffectManager = GetComponent<SoundEffectManager>();
    
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

    GUI.DrawTexture(new Rect(0, 0, width, height), scoreboardBackgroundTexture);

    int startX = (width - buttonWidth) / 2;
    int startY = height / 5;

    if (GUI.Button(new Rect(startX, 3 * (buttonHeight + 10) + startY, buttonWidth, buttonHeight), returnButtonTexture)) {
      soundEffectManager.playButtonSound();
      Application.LoadLevel("MainMenu");
    }

    int textWidth = width / 2;
    int textHeight = height / 2;
    Color originalColor = GUI.color;
    GUI.color = Color.black;
    GUI.Label(new Rect(width / 5, height / 3, textWidth, textHeight),  text);
    GUI.color = originalColor;

    GUILayout.EndArea();
  }

  private IEnumerator GetScores() {
    text = "Loading...";
    WWW hs_get = new WWW(scoreboardUrl);
    yield return hs_get;
    if (hs_get.error != null) {
      print("There was an error getting the scoreboard : " + hs_get.error);
    } else {
      text = hs_get.text;
    }
  }
}
