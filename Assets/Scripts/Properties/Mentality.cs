using UnityEngine;
using System.Collections;

public class Mentality : MonoBehaviour {

  public Texture mentalityBarTexture;
  public Texture mentalityBarBackgroundTexture;

  public float maxMentalityPoint;
  public float faintPerSecond;

  private float mentalityPoint;

  private CharacterMotor playerCharacterMotor;
  private MouseLook2 playerMouseLook;

  void Start () {
    mentalityPoint = maxMentalityPoint;

    playerCharacterMotor = GetComponent<CharacterMotor>();
    playerMouseLook = GetComponent<MouseLook2>();
  }

  void Update () {
    if (GameState.state != GameState.PLAYING) {
      return;
    }

    mentalityPoint -= Time.deltaTime * faintPerSecond;
    if (mentalityPoint <= 0) {
      if (GameState.state != GameState.LOSING) {
        GameState.state = GameState.LOSING;
        playerCharacterMotor.enabled = false;
        playerMouseLook.enabled = false;
      }
    }
  }

  /*
  void OnGUI () {

    if (GameState.state != GameState.PLAYING) {
      return;
    }

    GUI.depth = 0;

    int width = Screen.height / 8;
    int height = Screen.height / 16;
    int startX = width / 10;
    int startY = 20;

    GUI.DrawTexture(new Rect(startX, startY, width, height), mentalityBarBackgroundTexture);

    GUILayout.BeginArea(new Rect(startX, startY, width * (mentalityPoint / maxMentalityPoint), height));

    GUI.DrawTexture(new Rect(0, 0, width, height), mentalityBarTexture);

    GUILayout.EndArea();
  }
  */

  public bool enough (float need) {
    return (mentalityPoint >= need);
  }

  public void gain (float point) {
    mentalityPoint += point;
    if (mentalityPoint > maxMentalityPoint) {
      mentalityPoint = maxMentalityPoint;
    }
  }

  public void use (float cost) {
    if (mentalityPoint >= cost) {
      mentalityPoint -= cost;
    }
  }

  public void setMaxMentalityPoint (float point) {
    maxMentalityPoint = point;
  }

  public float getMaxMentalityPoint () {
    return maxMentalityPoint;
  }

  public float getMentalityPointPercent () {
    return mentalityPoint / maxMentalityPoint;
  }

}
