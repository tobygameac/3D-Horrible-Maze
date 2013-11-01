using UnityEngine;
using System.Collections;

public class Mentality : MonoBehaviour {

  public Texture memtalityBarTexture;

  private float memtalityPoint;
  public float faintPerSecond = 0.5f;
  private GameObject sight;

  void Start () {
    memtalityPoint = 100;
    sight = transform.FindChild("Sight").gameObject;
  }

  void Update () {
    memtalityPoint -= Time.deltaTime * faintPerSecond;
    if (memtalityPoint <= 0) {
      Application.LoadLevel("MainMenu");
    }
    sight.light.range = 10 + memtalityPoint / 10;
    sight.light.spotAngle = memtalityPoint;
  }

  public bool enough (float need) {
    return (memtalityPoint >= need);
  }

  public void use (float need) {
    if (memtalityPoint >= need) {
      memtalityPoint -= need;
    }
  }

  void OnGUI () {
    GUI.DrawTexture(new Rect(10, 20, (int)memtalityPoint, 10), memtalityBarTexture, ScaleMode.StretchToFill);
  }
}
