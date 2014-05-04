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
  private List<Vector2> bloodSize;
  private List<Vector2> bloodPosition;
  private List<int> bloodRotation;
  private List<float> bloodShowTime;
  private List<float> bloodShowedTime;
  private List<float> bloodShowTimeDelay;

  private SoundEffectManager soundEffectManager;

  void Start () {
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    minWidth = (int)(Screen.width * 0.25f);
    maxWidth = (int)(Screen.width * 0.75f);
    minHeight = (int)(Screen.height * 0.25f);
    maxHeight = (int)(Screen.height * 0.75f);

    bloodIndex = new List<int>();
    bloodSize = new List<Vector2>();
    bloodPosition = new List<Vector2>();
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
        bloodSize.RemoveAt(i);
        bloodPosition.RemoveAt(i);
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
        Vector2 pivotPoint = new Vector2(bloodPosition[i].x + bloodSize[i].x / 2, bloodPosition[i].y + bloodSize[i].y / 2);
        GUIUtility.RotateAroundPivot(bloodRotation[i], pivotPoint); 
        GUI.DrawTexture(new Rect(bloodPosition[i].x, bloodPosition[i].y, bloodSize[i].x, bloodSize[i].y), bloodTextures[bloodIndex[i]]);
        GUI.color = originalColor;
      }
    }
  }

  public void addBlood (int bloodCount = -1, float showTime = 1.5f, float showTimeDelay = 0.1f) {
    if (bloodCount < 0) {
      bloodCount = random.Next(3) + 1;
    }
    for (int i = 0; i < bloodCount; i++) {
      bloodIndex.Add(random.Next(bloodTextures.Length));
      int sizeX = minWidth + random.Next(maxWidth - minWidth);
      int sizeY = minHeight + random.Next(maxHeight - minHeight);
      bloodSize.Add(new Vector2(sizeX, sizeY));
      bloodPosition.Add(new Vector2(random.Next(Screen.width - sizeX), random.Next(Screen.height - sizeY)));
      bloodRotation.Add(random.Next(360));
      bloodShowTime.Add(showTime);
      bloodShowedTime.Add(0);
      bloodShowTimeDelay.Add(i * ((float)random.NextDouble() * showTimeDelay));
    }
  }

}
