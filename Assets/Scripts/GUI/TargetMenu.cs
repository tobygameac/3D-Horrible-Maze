using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetMenu : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;

  private static string target;
  private static string targetToAdd;

  private static bool isFadeIn;
  private static float fadeInTime = 0.75f;
  private static float fadedInTime;
  private static bool isFadeOut;
  private static float fadeOutTime = 0.75f;
  private static float fadedOutTime;

  private SoundEffectManager soundEffectManager;

  void Start () {
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
  }

  void Update () {
    if (isFadeIn) {
      fadedInTime += Time.deltaTime;
      if (fadedInTime > fadeInTime) {
        isFadeIn = false;
      }
    }
    if (isFadeOut) {
      fadedOutTime += Time.deltaTime;
      if (fadedOutTime > fadeOutTime) {
        isFadeOut = false;
        target = null;
        if (targetToAdd != null) {
          target = targetToAdd;
          targetToAdd = null;
          isFadeIn = true;
          fadedInTime = 0;
          soundEffectManager.playFlipSound();
        }
      }
    }
  }

  void OnGUI () {
    
    float percent = 1.0f;

    if (isFadeIn) {
      percent = fadedInTime / fadeInTime;
    }

    if (isFadeOut) {
      percent = 1 - (fadedOutTime / fadeOutTime);
    }

    if (target != null) {
      GUI.skin = skin;
      /*
      float backgroundWidth = Screen.height / 5.0f;
      float backgroundHeight = Screen.height / 6.0f;
      float targetGap = backgroundHeight * 0.3f;
      float targetWidth = backgroundWidth * 0.9f;
      float targetHeight = backgroundHeight * 0.6f;
      float startX = Screen.width - backgroundWidth * 1.5f;
      float startY = Screen.height * 0.3f + targetGap;
      float targetStartX = targetWidth * 0.2f;
      float targetStartY = targetHeight / 2.0f;

      GUILayout.BeginArea(new Rect(startX, startY, backgroundWidth, backgroundHeight * percent));
      GUI.color = new Color(1, 1, 1, percent);
      GUI.DrawTexture(new Rect(0, 0, backgroundWidth, backgroundHeight), backgroundTexture);

      GUI.color = new Color(0.5f, 0, 0, percent);
      GUI.skin.label.fontSize = (Screen.width + Screen.height) / 75;
      GUI.Label(new Rect(targetStartX, targetStartY, targetWidth, targetHeight), target);
      GUILayout.EndArea();

      */
      GUI.skin.label.fontSize = (Screen.width + Screen.height) / 60;
      float width = GUI.skin.label.fontSize * 0.5f * target.Length;
      float height = Screen.height / 6.0f;
      float startX = Screen.width - width * 1.1f;
      float startY = Screen.height / 4.5f;
      GUI.color = new Color(0.5f, 0, 0, percent);
      GUILayout.BeginArea(new Rect(startX, startY, width * percent, height));
      GUI.Label(new Rect(0, 0, width, height), target);
      GUILayout.EndArea();
    }

  }

  public static void clearTarget (bool animation = true) {
    if (!animation) {
      target = null;
      return;
    }
    isFadeOut = true;
    fadedOutTime = 0;
  }

  public static void addTarget (string newTarget) {
    targetToAdd = newTarget;
    clearTarget();
  }

}
