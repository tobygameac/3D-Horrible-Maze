using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

  public Texture scoreboardTexture;

  private int score = 0;

  private Boss boss;

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
    GUI.Label(new Rect(startX + width / 4, startY + height / 3, width, height),  score.ToString());
    GUI.color = originalColor;
  }

  public void addScore (int addedScore) {
    score += addedScore;
    if ((score % 10) == 0) {
      boss.addQTELength(1);
    }
  }

}
