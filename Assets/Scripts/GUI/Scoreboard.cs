using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

  public GUISkin skin;

  public Texture scoreboardTexture;

  private int score;

  public int additionEventFrame;
  private int nextAdditionEventScore;
  public int QTELengthUpgradeFrame;
  private int nextQTELengthUpgradeScore;
  public int QTEWrongMentalityUpgradeFrame;
  private int nextQTEWrongMentalityUpgradeScore;
  public int QTETimeLimitUpgradeFrame;
  private int nextQTETimeLimitUpgradeScore;
  public int insaneFrame;
  private Boss boss;

  private int newAddedScore;
  private bool isShowingNewAddedScore;
  public float showingNewAddedScoreTime = 1.5f;
  private float showedNewAddedScoreTime;

  private MazeGenerator maze;

  private int eps = 7;

  private LoadingWithAnimation loadingWithAnimation;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();

    boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();

    loadingWithAnimation = Camera.main.GetComponent<LoadingWithAnimation>();

    nextAdditionEventScore = additionEventFrame;
    nextQTEWrongMentalityUpgradeScore = QTEWrongMentalityUpgradeFrame;
    nextQTELengthUpgradeScore = QTELengthUpgradeFrame;
    nextQTETimeLimitUpgradeScore = QTETimeLimitUpgradeFrame;

    score = -eps;
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
    GUI.skin = skin;

    if (GameState.state == GameState.FINISHED) {
      GameState.score = score;
      loadingWithAnimation.loadLevelWithAnimation("SubmitMenu");
    }

    if (GameState.state != GameState.PLAYING) {
      return;
    }

    if (GameMode.mode != GameMode.INFINITE) {
      return;
    }

    float width = Screen.height / 4;
    float height = Screen.height / 4;
    float startX = Screen.width - width - width / 5;
    float startY = 0;
    //GUI.DrawTexture(new Rect(startX, startY, width, height), scoreboardTexture);
    Color originalColor = GUI.color;
    GUI.color = new Color(0.65f, 0, 0);
    if (score + eps == 0) {
      GUI.skin.label.fontSize = (int)((Screen.width + Screen.height) / 20.0f);
    } else {
      GUI.skin.label.fontSize = (int)((Screen.width + Screen.height) / 25.0f * 3.0f / ((int)Mathf.Log10(score + eps) + 1));
    }
    GUI.Label(new Rect(startX + width / 3.5f, startY + height / 3, width, height), (score + eps).ToString());
    if (isShowingNewAddedScore) {
      float alpha = (showingNewAddedScoreTime - showedNewAddedScoreTime) / showingNewAddedScoreTime;
      GUI.color = new Color(1, 1, 0, alpha);
      GUI.skin.label.fontSize = (Screen.width + Screen.height) / 75;
      GUI.Label(new Rect(startX + width / 3, startY + height / (8 - alpha * 6), width, height),  "+" + newAddedScore);
    }
    GUI.color = originalColor;
  }

  public int getScore () {
    return score;
  }

  public void addScore (int addedScore) {
    if (GameMode.mode != GameMode.INFINITE) {
      return;
    }
    score += addedScore;
    newAddedScore = addedScore;
    isShowingNewAddedScore = true;
    showedNewAddedScoreTime = 0;
    if (score >= nextAdditionEventScore) {
      maze.allocateRandomEvent(1);
      nextAdditionEventScore += additionEventFrame;
    }
    if (score >= nextQTELengthUpgradeScore) {
      boss.addQTELength(1);
      nextQTELengthUpgradeScore += QTELengthUpgradeFrame;
    }
    if (score >= nextQTEWrongMentalityUpgradeScore) {
      boss.addQTEWrongMentality(1);
      nextQTEWrongMentalityUpgradeScore += QTEWrongMentalityUpgradeFrame;
    }
    if (score >= nextQTETimeLimitUpgradeScore) {
      boss.addQTETimeLimit(-0.1f);
      nextQTETimeLimitUpgradeScore += QTETimeLimitUpgradeFrame;
    }
    if (score >= insaneFrame) {
      boss.becomeInsaneMode();
    }
  }

}
