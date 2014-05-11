using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {

  public Texture mask;

  public float fadeInTime;
  private float fadedInTime;
  private float alpha;

  void Start () {
    start();
  }

  void Update () {
    if (fadedInTime <= fadeInTime) {
      fadedInTime += Time.deltaTime;
      alpha = 1 - (fadedInTime / fadeInTime);
    }
  }

  void OnGUI () {
    GUI.depth = -1;
    GUI.color = new Color(0, 0, 0, alpha);
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mask);
  }

  public void start () {
    fadedInTime = 0;
  }
}
