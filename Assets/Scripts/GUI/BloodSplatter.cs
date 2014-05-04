using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodSplatter : MonoBehaviour {

  private static System.Random random = new System.Random(); // Only need one random seed

  public Texture[] bloodTextures;
  public int minWidth;
  public int maxWidth;
  public int minHeight;
  public int maxHeight;

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
      bloodShowTimeDelay[i] -= Time.deltaTime;
      if (bloodShowTimeDelay[i] <= 0) {
        if (bloodShowedTime[i] < 1e-6) {
          soundEffectManager.playBloodHitSound();
        }
        bloodShowedTime[i] += Time.deltaTime;
      }
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
      if (bloodShowTimeDelay[i] <= 0 && bloodShowedTime[i] <= bloodShowTime[i]) {
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

  public void addBlood (int bloodCount = -1, float showTime = 1.5f, float showTimeDelay = 0.3f) {
    if (bloodCount < 0) {
      bloodCount = random.Next(4) + 1;
    }
    for (int i = 0; i < bloodCount; i++) {
      bloodIndex.Add(random.Next(bloodTextures.Length));
      bloodSize.Add(new Vector2(minWidth + random.Next(maxWidth - minWidth), minHeight + random.Next(maxHeight - minHeight)));
      bloodPosition.Add(new Vector2(random.Next(Screen.width - (int)bloodSize[i].x), random.Next(Screen.height - (int)bloodSize[i].y)));
      bloodRotation.Add(random.Next(360));
      bloodShowTime.Add(showTime);
      bloodShowedTime.Add(0);
      bloodShowTimeDelay.Add(i * ((float)random.NextDouble() * showTimeDelay));
    }
  }

}
