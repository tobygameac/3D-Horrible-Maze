using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

  public Texture scoreboardTexture;

  private int score = 0;

  private Boss boss;

  private string secretKey = "tobygameac";

  void Start () {
    boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();
  }

  void OnGUI () {
    int width = Screen.height / 4;
    int height = Screen.height / 8;
    int startX = Screen.width - width - width / 5;
    int startY = height / 5;
    GUI.DrawTexture(new Rect(startX, startY, width, height), scoreboardTexture);
    Color originalColor = GUI.color;
    GUI.color = Color.black;
    GUI.Label(new Rect(startX + width / 4, startY + height / 3, width, height),  "Score : " + score);
    GUI.color = originalColor;
  }

  public void addScore (int addedScore) {
    score += addedScore;
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

  public IEnumerator postScore () {
    string name = "anonymous";
    
    string postScoreUrl = "http://134.208.43.1:5631/3DhorribleMaze/postScore.php?";
 
    string hash = Md5Sum(name + score + secretKey); 
 
    string realPostScoreUrl = postScoreUrl + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
 
    WWW hs_post = new WWW(realPostScoreUrl);
    yield return hs_post;
    if (hs_post.error != null) {
      print("There was an error posting the high score: " + hs_post.error);
    }

    Application.LoadLevel("ViewScoreboard");
  }

}
