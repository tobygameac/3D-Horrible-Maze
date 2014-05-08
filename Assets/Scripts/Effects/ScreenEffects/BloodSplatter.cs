using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodSplatter : MonoBehaviour {

  private static System.Random random = new System.Random(); // Only need one random seed

  public Texture[] bloodTextures;
  private int minWidth;
  private int maxWidth;
  private int minHeight;
  private int maxHeight;

  private List<int> bloodIndex;
  private List<Rect> bloodRect;
  private List<int> bloodRotation;
  private List<float> bloodShowTime;
  private List<float> bloodShowedTime;
  private List<float> bloodShowTimeDelay;

  private CameraShaker cameraShaker;

  private SoundEffectManager soundEffectManager;

  void Start () {
    cameraShaker = GameObject.FindWithTag("Main").GetComponent<CameraShaker>();

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    minWidth = (int)(Screen.width * 0.25f);
    maxWidth = (int)(Screen.width * 0.75f);
    minHeight = (int)(Screen.height * 0.25f);
    maxHeight = (int)(Screen.height * 0.75f);

    bloodIndex = new List<int>();
    bloodRect = new List<Rect>();
    bloodRotation = new List<int>();
    bloodShowTime = new List<float>();
    bloodShowedTime = new List<float>();
    bloodShowTimeDelay = new List<float>();
  }

  void Update () {
    for (int i = 0; i < bloodIndex.Count; i++) {
      if (bloodShowTimeDelay[i] > 0) {
        bloodShowTimeDelay[i] -= Time.deltaTime;
        continue;
      }
      if (bloodShowedTime[i] < 1e-6) {
        soundEffectManager.playBloodHitSound();
      }
      bloodShowedTime[i] += Time.deltaTime;
      if (bloodShowedTime[i] > bloodShowTime[i]) {
        bloodIndex.RemoveAt(i);
        bloodRect.RemoveAt(i);
        bloodRotation.RemoveAt(i);
        bloodShowTime.RemoveAt(i);
        bloodShowedTime.RemoveAt(i);
        bloodShowTimeDelay.RemoveAt(i);
        i--;
      }
    }
  }

  void OnGUI () {
    for (int i = 0; i < bloodIndex.Count; i++) {
      if (bloodShowTimeDelay[i] > 0) {
        continue;
      }
      if (bloodShowedTime[i] <= bloodShowTime[i]) {
        Color originalColor = GUI.color;
        float bloodAlpha = 1 - (bloodShowedTime[i] / bloodShowTime[i]);
        GUI.color = new Color(1, 1, 1, bloodAlpha);
        Vector2 pivotPoint = new Vector2(bloodRect[i].xMin + bloodRect[i].width / 2, bloodRect[i].yMin + bloodRect[i].height / 2);
        GUIUtility.RotateAroundPivot(bloodRotation[i], pivotPoint); 
        GUI.DrawTexture(bloodRect[i], bloodTextures[bloodIndex[i]]);
        GUI.color = originalColor;
      }
    }
  }

  private bool intersect (Rect rectA, Rect rectB) {
    bool c1 = rectA.xMin < rectB.xMax;
    bool c2 = rectA.xMax > rectB.xMin;
    bool c3 = rectA.yMin < rectB.yMax;
    bool c4 = rectA.yMax > rectB.yMin;
    return c1 && c2 && c3 && c4;
  }

  private bool noIntersectWithAllRects (Rect rect) {
    for (int i = 0; i < bloodRect.Count; i++) {
      if (intersect(rect, bloodRect[i])) {
        return false;
      }
    }
    return true;
  }

  public void addBlood (int bloodCount = -1, float showTime = 1.5f, float showTimeDelay = 0.3f, bool shakeCamera = false) {
    if (bloodCount < 0) {
      bloodCount = random.Next(3) + 1;
    }
    for (int i = 0; i < bloodCount; i++) {
      bloodIndex.Add(random.Next(bloodTextures.Length));
      int sizeX = minWidth + random.Next(maxWidth - minWidth);
      int sizeY = minHeight + random.Next(maxHeight - minHeight);
      Rect rect = new Rect(random.Next(Screen.width - sizeX), random.Next(Screen.height - sizeY), sizeX, sizeY);
      /*
      int tried = 0, maximumTried = 100;
      while (!noIntersectWithAllRects(rect)) {
        if (tried > maximumTried) {
          break;
        }
        sizeX = minWidth + random.Next(maxWidth - minWidth) / (tried + 1);
        sizeY = minHeight + random.Next(maxHeight - minHeight) / (tried + 1);
        rect = new Rect(random.Next(Screen.width - sizeX), random.Next(Screen.height - sizeY), sizeX, sizeY);
        tried++;
      }
      */
      bloodRect.Add(rect);
      bloodRotation.Add(random.Next(360));
      bloodShowTime.Add(showTime + (float)random.NextDouble());
      bloodShowedTime.Add(0);
      bloodShowTimeDelay.Add(showTimeDelay * (float)random.NextDouble());
    }
    if (shakeCamera) {
      StartCoroutine(cameraShaker.shakeCamera());
    }
  }

}
