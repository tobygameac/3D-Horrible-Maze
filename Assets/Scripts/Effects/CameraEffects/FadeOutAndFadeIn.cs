using UnityEngine;
using System.Collections;

public class FadeOutAndFadeIn : MonoBehaviour {

  public Texture mask;

  public float fadeOutTime;
  public float fadeInTime;
  private float fadedOutTime;
  private float fadedInTime;
  private float alpha;

  void Start () {
    start();
  }

  void Update () {
    if (fadedOutTime <= fadeOutTime) {
      fadedOutTime += Time.deltaTime;
      if (fadedOutTime > fadeOutTime) {
        return;
      }
      alpha = fadedOutTime / fadeOutTime;
      return;
    }
    fadedInTime += Time.deltaTime;
    if (fadedInTime <= fadeInTime) {
      alpha = 1 - (fadedInTime / fadeInTime);
    }
  }

  void OnGUI () {
    GUI.depth = -1;
    GUI.color = new Color(0, 0, 0, alpha);
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mask);
  }

  public void start () {
    fadedOutTime = 0;
    fadedInTime = 0;
  }
}
