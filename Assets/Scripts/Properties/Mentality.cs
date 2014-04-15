using UnityEngine;
using System.Collections;

public class Mentality : MonoBehaviour {

  public Texture mentalityBarTexture;

  public float maxMentalityPoint = 100;
  public float faintPerSecond = 1 / 15.0f;

  private float mentalityPoint;

  public float baseSightRange = 5;

  private GameObject sight;

  private bool gameover = false;

  private Scoreboard scoreboard;

  private CharacterMotor playerCharacterMotor;
  private MouseLook playerMouseLook;

  void Start () {
    mentalityPoint = maxMentalityPoint;
    sight = transform.FindChild("sight").gameObject;

    playerCharacterMotor = GetComponent<CharacterMotor>();
    playerMouseLook = GetComponent<MouseLook>();

    scoreboard = GameObject.FindWithTag("Main").GetComponent<Scoreboard>();
  }

  void Update () {
    mentalityPoint -= Time.deltaTime * faintPerSecond;
    if (mentalityPoint <= 0) {
      if (!gameover) {
        gameover = true;
        playerCharacterMotor.enabled = false;
        playerMouseLook.enabled = false;
        StartCoroutine(scoreboard.postScore());
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
    GUI.depth = 0;

    GUI.DrawTexture(new Rect(10, 20, (int)mentalityPoint, 10), mentalityBarTexture, ScaleMode.StretchToFill);
  }
}
