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

  void OnGUI () {
    
    if (isFadeIn) {
      fadedInTime += Time.deltaTime;
      if (fadedInTime > fadeInTime) {
        isFadeIn = false;
      }
      float alpha = fadedInTime / fadeInTime;
      GUI.color = new Color(1, 1, 1, alpha);
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
        }
      }
      float alpha = 1 - (fadedOutTime / fadeOutTime);
      GUI.color = new Color(1, 1, 1, alpha);
    }

    if (target != null) {
      float backgroundWidth = Screen.height / 5.0f;
      float backgroundHeight = Screen.height / 6.0f;
      float targetGap = backgroundHeight * 0.3f;
      float targetWidth = backgroundWidth * 0.9f;
      float targetHeight = backgroundHeight * 0.6f;
      float startX = Screen.width - backgroundWidth * 1.5f;
      float startY = Screen.height * 0.3f + targetGap;
      float targetStartX = startX + targetWidth * 0.2f;
      float targetStartY = startY + targetHeight / 2.0f;
      GUI.DrawTexture(new Rect(startX, startY, backgroundWidth, backgroundHeight), backgroundTexture);
      GUI.Label(new Rect(targetStartX, targetStartY, targetWidth, targetHeight), target);
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
