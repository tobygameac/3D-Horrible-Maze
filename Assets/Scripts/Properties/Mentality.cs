using UnityEngine;
using System.Collections;

public class Mentality : MonoBehaviour {

  public Texture mentalityBarTexture;

  public float maxMentalityPoint = 100;
  public float faintPerSecond = 1 / 15.0f;

  private float mentalityPoint;

  public float baseSightRange = 5;

  private GameObject sight;

  private CharacterMotor playerCharacterMotor;
  private MouseLook playerMouseLook;

  void Start () {
    mentalityPoint = maxMentalityPoint;
    sight = transform.FindChild("sight").gameObject;

    playerCharacterMotor = GetComponent<CharacterMotor>();
    playerMouseLook = GetComponent<MouseLook>();
  }

  void Update () {
    mentalityPoint -= Time.deltaTime * faintPerSecond;
    if (mentalityPoint <= 0) {
      if (GameState.state != GameState.LOSING) {
        GameState.state = GameState.LOSING;
        playerCharacterMotor.enabled = false;
        playerMouseLook.enabled = false;
      }
    }
    // Fading
    float ambientR = (mentalityPoint / maxMentalityPoint) * 0.27f + 0.03f;
    float ambientG = (mentalityPoint / maxMentalityPoint) * 0.27f + 0.03f;
    float ambientB = (mentalityPoint / maxMentalityPoint) * 0.27f + 0.03f;
    RenderSettings.ambientLight = new Color(ambientR, ambientG, ambientB);
    sight.light.range = baseSightRange + baseSightRange * (mentalityPoint / 100.0f);
    sight.light.spotAngle = 30 + 0.6f * mentalityPoint;
  }

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

  void OnGUI () {
    if (GameState.state != GameState.PLAYING) {
      return;
    }
    GUI.depth = 0;

    int width = Screen.height / 8;
    int height = Screen.height / 16;
    int startX = width / 10;
    int startY = 20;

    GUILayout.BeginArea(new Rect(startX, startY, width * (mentalityPoint / maxMentalityPoint), height));

    GUI.DrawTexture(new Rect(0, 0, width, height), mentalityBarTexture);

    GUILayout.EndArea();
  }
}
