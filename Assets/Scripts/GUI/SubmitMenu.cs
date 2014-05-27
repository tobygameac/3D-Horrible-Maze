using UnityEngine;
using System.Collections;

public class SubmitMenu : MonoBehaviour {

  public GUISkin bloodOnHoverSkin;
  public GUISkin titleSkin;

  public Texture backgroundTexture;
  public Texture submitMenuBackgroundTexture;

  public int score;

  private bool scoreSubmitted;
  private float scoreSubmittedTime;
  private string playerName = "anonymous";
  private int hard;
  private bool[] optionStatus;

  private string secretKey = "tobygameac";
  private int eps = 7;

  void Start () {
    GameMode.mode = GameMode.INFINITE;
    GameState.userStudy = true;
    GameState.state = GameState.FINISHED;
    score = GameState.score;
    if (GameMode.mode != GameMode.INFINITE) {
      score = -eps;
    }
    scoreSubmitted = false;
    hard = -1;
    optionStatus = new bool[3];
    for (int i = 0; i < 3; i++) {
      optionStatus[i] = false;
    }
    optionStatus[2] = true;
  }

  void Update () {
    if (scoreSubmitted) {
      scoreSubmittedTime += Time.deltaTime;
    }
  }

  void OnGUI () {
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

    int width = Screen.height - 100;
    int height = width;

    GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));

    GUI.DrawTexture(new Rect(0, 0, width, height), submitMenuBackgroundTexture);

    GUI.skin = bloodOnHoverSkin;

    float scoreWidth = width / 2.0f;
    float scoreHeight = height / 3.5f;
    GUI.color = new Color(0.65f, 0, 0);
    float startX = (width - scoreWidth) / 2;
    float startY = height * 0.05f;
    if (GameMode.mode == GameMode.INFINITE) {
      float deltaY = 0;
      if (score + eps < 100) {
        GUI.skin.label.fontSize = (int)((width + height) * 0.2f);
      } else {
        deltaY = (Mathf.Log10(score + eps) + 1) * scoreHeight * 0.05f;
        GUI.skin.label.fontSize = (int)((width + height) * 0.6f / ((int)Mathf.Log10(score + eps) + 1));
        //GUI.skin.label.fontSize = (int)((width + height) / 6.0f);
      }
      GUI.Label(new Rect(startX, startY + deltaY, scoreWidth, scoreHeight), (score + eps).ToString());

      float textWidth = height / 1.5f;
      float textHeight = height / 3.0f;
      GUI.skin.label.fontSize = (int)((width + height) / 25);
      GUI.Label(new Rect(startX + scoreWidth, startY + scoreHeight / 2, textWidth, textHeight), "score");
    } else {
      float textWidth = height / 1.5f;
      float textHeight = height / 3.0f;
      startX = (width - textWidth) / 2;
      if (GameState.win) {
        GUI.color = new Color(1.0f, 0, 0.25f);
        GUI.skin.label.fontSize = (int)((width + height) / 5.5f);
        GUI.Label(new Rect(startX, startY, textWidth, textHeight), "W I N");
      } else {
        GUI.color = new Color(0.3f, 0, 0);
        GUI.skin.label.fontSize = (int)((width + height) / 7.0f);
        GUI.Label(new Rect(startX, startY, textWidth, textHeight), "L O S E");
      }
    }
    GUI.skin.label.fontSize = (int)((width + height) / 50);
    GUI.skin.button.fontSize = (int)((width + height) / 25.0f);
    GUI.skin.toggle.fontSize = (int)((width + height) / 50);
    if (scoreSubmitted) {
      float labelWidth = height / 2.0f;
      float labelHeight = height / 16.0f;
      startX = (width - labelWidth) / 2.0f;
      startY += scoreHeight + labelHeight * 0.5f;
      GUI.color = new Color(0.1f, 0, 0);
      GUI.skin.label.fontSize = (int)((width + height) / 25.0f);
      string submittingString = "Submitting";
      int additionalDot = ((int)(Time.time * 100) % 100) / 20;
      for (int i = 0; i < additionalDot; i++) {
        submittingString += ".";
      }
      GUI.Label(new Rect(startX, startY, labelWidth, labelHeight), submittingString);
    } else {
      float textAreaWidth = height / 2.0f;
      float textAreaHeight = height / 16.0f;
      startX = (width - textAreaWidth) / 2.0f;
      startY += scoreHeight + textAreaHeight * 0.5f;
      GUI.color = new Color(1.0f, 1.0f, 1.0f);
      playerName = GUI.TextArea(new Rect(startX, startY, textAreaWidth, textAreaHeight), playerName);
      if (playerName.Length >= 20) {
        playerName = playerName.Substring(0, 20);
      }
      float buttonWidth = height / 3.0f;
      float buttonHeight = height / 8.0f;
      startX = (width - buttonWidth) / 2.0f;
      startY += textAreaHeight + buttonHeight * 0.5f;
      GUI.color = new Color(0.75f, 0, 0);
      if (GUI.Button(new Rect(startX, startY, buttonWidth, buttonHeight), "Submit")) {
        StartCoroutine(postScore(playerName, hard));
      }
      float optionWidth = width / 2.0f;
      float optionHeight = height / 16.0f;
      startX = (width - optionWidth) / 2.0f;
      startY += buttonHeight + optionHeight * 0.5f;
      GUI.color = new Color(0.1f, 0, 0);
      if (GameState.userStudy) {
        if (GUI.Toggle(new Rect(startX, startY + 0 * (optionHeight * 1.25f), optionWidth, optionHeight), 
              optionStatus[0], " I think this map is easy.")) {
          hard = 0;
          optionStatus[0] = true;
          optionStatus[1] = false;
          optionStatus[2] = false;
        }
        if (GUI.Toggle(new Rect(startX, startY + 1 * (optionHeight * 1.25f), optionWidth, optionHeight), 
              optionStatus[1], " I think this map is hard.")) {
          hard = 1;
          optionStatus[0] = false;
          optionStatus[1] = true;
          optionStatus[2] = false;
        }
        if (GUI.Toggle(new Rect(startX, startY + 2 * (optionHeight * 1.25f), optionWidth, optionHeight), 
              optionStatus[2], " I don't know.")) {
          hard = -1;
          optionStatus[0] = false;
          optionStatus[1] = false;
          optionStatus[2] = true;
        }
      }
    }

    GUILayout.EndArea();
  }

  public string Md5Sum(string stringToEncrypt) {
    System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
    byte[] bytes = ue.GetBytes(stringToEncrypt);
   
    // encrypt bytes
    System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
    byte[] hashBytes = md5.ComputeHash(bytes);
   
    // Convert the encrypted bytes back to a string (base 16)
    string hashString = "";

    for (int i = 0; i < hashBytes.Length; i++) {
      hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
    }
    return hashString.PadLeft(32, '0');
  }

  public IEnumerator postScore (string name, int hard) {

    scoreSubmitted = true;
    scoreSubmittedTime = 0;

    name = name.Trim();
    if (name.Length >= 20) {
      name = name.Substring(0, 20);
    }
    
    string postScoreUrl = "http://134.208.43.1:5631/3DhorribleMaze/postScore.php?";

    if (GameState.difficulty == 1) {
      postScoreUrl = "http://134.208.43.1:5631/3DhorribleMaze/postScore%20-%20hard.php?";
    }

    if (GameMode.mode != GameMode.INFINITE) {
      score = -eps;
    }
 
    string hash = Md5Sum(name + (score + eps).ToString() + secretKey); 
 
    string realPostScoreUrl = postScoreUrl + "name=" + WWW.EscapeURL(name) + "&score=" + (score + eps).ToString() + "&hard=" + hard + "&hash=" + hash;
 
    WWW hs_post = new WWW(realPostScoreUrl);
    yield return hs_post;
    if (hs_post.error != null) {
      print("There was an error posting the high score: " + hs_post.error);
    }

    if (GameMode.mode != GameMode.INFINITE) {
      Application.LoadLevel("MainMenu");
    } else {
      Application.LoadLevel("Rank");
    }
  }
}
