using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

  public Texture scoreboardTexture;

  private int score = 0;
  private bool scoreSubmitted;
  private string playerName = "anonymous";
  private int hard;
  private bool[] optionStatus;

  private Boss boss;

  private string secretKey = "tobygameac";

  private int newAddedScore;
  private bool isShowingNewAddedScore;
  public float showingNewAddedScoreTime = 1.5f;
  private float showedNewAddedScoreTime;

  void Start () {
    boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();
    scoreSubmitted = false;
    hard = -1;
    optionStatus = new bool[3];
    for (int i = 0; i < 3; i++) {
      optionStatus[i] = false;
    }
    optionStatus[2] = true; 
  }

  void Update () {
    if (isShowingNewAddedScore) {
      showedNewAddedScoreTime += Time.deltaTime;
      if (showedNewAddedScoreTime > showingNewAddedScoreTime) {
        isShowingNewAddedScore = false;
      }
    }
  }

  void OnGUI () {
    if (GameState.state == GameState.LOSING) {
      int buttonWidth = Screen.height / 8;
      int buttonHeight = Screen.height / 16;
      if (scoreSubmitted) {
        GUI.Label(new Rect((Screen.width - buttonWidth) / 2, (Screen.height - buttonHeight) / 2, buttonWidth, buttonHeight), "Submitting");
      } else {
        int textAreaWidth = Screen.height / 4;
        int textAreaHeight = Screen.height / 8;
        playerName = GUI.TextArea(new Rect((Screen.width - textAreaWidth) / 2, (Screen.height - textAreaHeight) / 2 - textAreaHeight, textAreaWidth, textAreaHeight), playerName);
        if (GUI.Button(new Rect((Screen.width - buttonWidth) / 2, (Screen.height - buttonHeight) / 2, buttonWidth, buttonHeight), "Submit")) {
          StartCoroutine(postScore(playerName, hard));
        }
        if (GameState.userStudy) {
          if (GUI.Toggle(new Rect((Screen.width - textAreaWidth) / 2, (Screen.height - textAreaHeight) / 2 + 1 * textAreaHeight, textAreaWidth, textAreaHeight), 
                optionStatus[0], "I think this map is easy.")) {
            hard = 0;
            optionStatus[0] = true;
            optionStatus[1] = false;
            optionStatus[2] = false;
          }
          if (GUI.Toggle(new Rect((Screen.width - textAreaWidth) / 2, (Screen.height - textAreaHeight) / 2 + 2 * textAreaHeight, textAreaWidth, textAreaHeight), 
                optionStatus[1], "I think this map is hard.")) {
            hard = 1;
            optionStatus[0] = false;
            optionStatus[1] = true;
            optionStatus[2] = false;
          }
          if (GUI.Toggle(new Rect((Screen.width - textAreaWidth) / 2, (Screen.height - textAreaHeight) / 2 + 3 * textAreaHeight, textAreaWidth, textAreaHeight), 
                optionStatus[2], "I don't know.")) {
            hard = -1;
            optionStatus[0] = false;
            optionStatus[1] = false;
            optionStatus[2] = true;
          }
        }
      }
    }
    if (GameState.state != GameState.PLAYING) {
      return;
    }
    int width = Screen.height / 4;
    int height = Screen.height / 8;
    int startX = Screen.width - width - width / 5;
    int startY = height / 5;
    GUI.DrawTexture(new Rect(startX, startY, width, height), scoreboardTexture);
    Color originalColor = GUI.color;
    GUI.color = Color.black;
    GUI.Label(new Rect(startX + width / 3.5f, startY + height / 3, width, height),  "Score : " + score);
    if (isShowingNewAddedScore) {
      float alpha = (showingNewAddedScoreTime - showedNewAddedScoreTime) / showingNewAddedScoreTime;
      GUI.color = new Color(1, 1, 0, alpha);
      GUI.Label(new Rect(startX + width / 3, startY + height / (8 - alpha * 6), width, height),  "+" + newAddedScore);
    }
    GUI.color = originalColor;
  }

  public int getScore () {
    return score;
  }

  public void addScore (int addedScore) {
    score += addedScore;
    newAddedScore = addedScore;
    isShowingNewAddedScore = true;
    showedNewAddedScoreTime = 0;
    if ((score % 1000) == 0) {
      boss.addQTELength(1);
    }
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

    name = name.Trim();
    if (name.Length >= 20) {
      name = name.Substring(0, 20);
    }

    scoreSubmitted = true;
    
    string postScoreUrl = "http://134.208.43.1:5631/3DhorribleMaze/postScore.php?";

    if (GameState.difficulty > 0) {
      postScoreUrl = "http://134.208.43.1:5631/3DhorribleMaze/postScore%20-%20hard.php?";
    }
 
    string hash = Md5Sum(name + score + secretKey); 
 
    string realPostScoreUrl = postScoreUrl + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hard=" + hard + "&hash=" + hash;
 
    WWW hs_post = new WWW(realPostScoreUrl);
    yield return hs_post;
    if (hs_post.error != null) {
      print("There was an error posting the high score: " + hs_post.error);
    }

    Application.LoadLevel("Rank");
  }

}
