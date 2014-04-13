using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QTE : MonoBehaviour {

  private static System.Random random = new System.Random(); // Only need one random seed

  public Texture[] QTETextures;
  public Texture barTexture;
  public Texture barBackgroundTexture;
  private static int textureIndex;
  private static int QTEFullLength;
  private static int QTENowLength;
  private static bool isShowing;
  private bool small;
  public float showingTime = 0.1f;
  private float showedTime;
  
  public static List<int> generateQTE (int QTEventLength) {
    QTEFullLength = QTEventLength;
    List<int> QTEvent = new List<int>();
    for (int i = 0; i < QTEventLength; i++) {
      QTEvent.Add(random.Next(4));
    }
    return QTEvent;
  }

  public static void showQTE (List<int> QTEvent) {
    QTENowLength = QTEvent.Count;
    if (QTEvent.Count == 0) {
      isShowing = false;
      return;
    }
    isShowing = true;
    textureIndex = QTEvent[0];
  }

  void Start () {
    isShowing = false;
    showedTime = 0;
  }

  void Update () {
    if (isShowing) {
      showedTime += Time.deltaTime;
      if (showedTime >= showingTime) {
        showedTime = 0;
        small = !small;
      }
    }
  }

  void OnGUI () {
    if (isShowing) {

      GUI.depth = 1;

      int arrowSize;
      if (small) {
        arrowSize = Screen.height / 10;
      } else {
        arrowSize = Screen.height / 8;
      }
      GUI.DrawTexture(new Rect((Screen.width - arrowSize) / 2, Screen.height / 5, arrowSize, arrowSize), QTETextures[textureIndex]);

      int barWidth = Screen.width / 5;
      int barHeight = Screen.height / 16;

      GUI.DrawTexture(new Rect((Screen.width - barWidth) / 2, Screen.height / 8, barWidth, barHeight), barBackgroundTexture);
      
      GUI.BeginGroup(new Rect((Screen.width - barWidth) / 2, Screen.height / 8, (int)(barWidth * QTENowLength / (float)QTEFullLength), barHeight));
      GUI.DrawTexture(new Rect(0, 0, barWidth, barHeight), barTexture);
      GUI.EndGroup();
    }
  }
}
