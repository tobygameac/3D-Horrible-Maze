using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QTE : MonoBehaviour {

  private static System.Random random = new System.Random(); // Only need one random seed

  public Texture[] QTETextures;
  private static int textureIndex;
  private static bool isShowing = false;
  
  public static List<int> generateQTE (int QTEEventLength) {
    List<int> QTEvent = new List<int>();
    for (int i = 0; i < QTEEventLength; i++) {
      QTEvent.Add(random.Next(4));
    }
    return QTEvent;
  }

  public static void showQTE (List<int> QTEvent) {
    if (QTEvent.Count == 0) {
      isShowing = false;
      return;
    }
    isShowing = true;
    textureIndex = QTEvent[0];
  }

  void OnGUI () {
    if (isShowing) {
      GUI.DrawTexture(new Rect(100, 100, 100, 100), QTETextures[textureIndex]);
    }
  }
}
