using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

  public GUISkin titleSkin;

  public float fadeInTime;
  public float fadeOutTime;
  private float fadedInTime;
  private float fadedOutTime;
  private float alpha;

  void Start () {
    GameState.state = GameState.LOADING;

    fadedInTime = 0;
    fadedOutTime = 0;
  }

  void Update () {
    if (fadedInTime <= fadeInTime) {
      fadedInTime += Time.deltaTime;
      if (fadedInTime > fadeInTime) {
        return;
      }
      alpha = fadedInTime / fadeInTime;
      return;
    }
    fadedOutTime += Time.deltaTime;
    if (fadedOutTime <= fadeOutTime) {
      alpha = 1 - (fadedOutTime / fadeOutTime);
    } else {
      Application.LoadLevel("MainMenu");
    }
  }
  void OnGUI () {
    GUI.skin = titleSkin;
    GUI.skin.label.fontSize = (Screen.width + Screen.height) / 10;
    float titleWidth = Screen.width * 0.5f;
    float titleHeight = Screen.height * 0.3f;
    GUI.color = new Color(0.3f, 0, 0, alpha);
    GUI.Label(new Rect((Screen.width - titleWidth) / 2, (Screen.height - titleHeight) / 2, titleWidth, titleHeight), "H orridor");
  }

}
