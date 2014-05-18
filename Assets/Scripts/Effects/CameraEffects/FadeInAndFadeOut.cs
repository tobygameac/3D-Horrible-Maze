using UnityEngine;
using System.Collections;

public class FadeInAndFadeOut : MonoBehaviour {

  public Texture mask;

  public float fadeInTime;
  public float fadeOutTime;
  private float fadedInTime;
  private float fadedOutTime;
  private float alpha;

  void Start () {
    start();
  }

  void Update () {
    if (fadedInTime <= fadeInTime) {
      fadedInTime += Time.deltaTime;
      if (fadedInTime > fadeInTime) {
        return;
      }
      alpha = 1 - (fadedInTime / fadeInTime);
      return;
    }
    fadedOutTime += Time.deltaTime;
    if (fadedOutTime <= fadeOutTime) {
      alpha = fadedOutTime / fadeOutTime;
    }
  }

  void OnGUI () {
    GUI.depth = -1;
    GUI.color = new Color(0, 0, 0, alpha);
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mask);
  }

  public void start () {
    fadedInTime = 0;
    fadedOutTime = 0;
  }
}
