using UnityEngine;
using System.Collections;

public class Vitality : MonoBehaviour {

  public Texture vitalityBarTexture;

  public float maxVitalityPoint = 100;

  private float vitalityPoint;

  void Start () {
    vitalityPoint = maxVitalityPoint;
  }

  void Update () {
    if (vitalityPoint < 100) {
      vitalityPoint += Time.deltaTime;
    }
  }

  public bool enough (float need) {
    return (vitalityPoint >= need);
  }

  public void gain (float point) {
    vitalityPoint += point;
    if (vitalityPoint > maxVitalityPoint) {
      vitalityPoint = maxVitalityPoint;
    }
  }

  public void use (float need) {
    if (vitalityPoint >= need) {
      vitalityPoint -= need;
    }
  }

  void OnGUI () {
    GUI.depth = 0;

    int width = Screen.height / 8;
    int height = Screen.height / 16;
    int startX = width / 10;
    int startY = 60;

    GUILayout.BeginArea(new Rect(startX, startY, width * (vitalityPoint / maxVitalityPoint), height));

    GUI.DrawTexture(new Rect(0, 0, width, height), vitalityBarTexture);

    GUILayout.EndArea();
  }
}
