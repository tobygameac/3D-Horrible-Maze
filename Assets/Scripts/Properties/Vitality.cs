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

  public void use (float need) {
    if (vitalityPoint >= need) {
      vitalityPoint -= need;
    }
  }

  void OnGUI () {
    GUI.DrawTexture(new Rect(10, 40, (int)vitalityPoint, 10), vitalityBarTexture, ScaleMode.StretchToFill);
  }
}
