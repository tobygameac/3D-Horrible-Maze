using UnityEngine;
using System.Collections;

public class Mentality : MonoBehaviour {

  public Texture mentalityBarTexture;

  public float maxMentalityPoint = 100;
  public float faintPerSecond = 1 / 15.0f;

  private float mentalityPoint;

  public float baseSightRange = 5;

  private GameObject sight;

  void Start () {
    mentalityPoint = maxMentalityPoint;
    sight = transform.FindChild("sight").gameObject;
  }

  void Update () {
    mentalityPoint -= Time.deltaTime * faintPerSecond;
    if (mentalityPoint <= 0) {
      Application.LoadLevel("MainMenu");
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
