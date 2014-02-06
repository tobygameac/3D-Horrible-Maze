using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QTE : MonoBehaviour {

  private static System.Random random = new System.Random(); // Only need one random seed
  
  public static List<int> generateQTEEvent (int QTEEventLength) {
    List<int> QTEEvent = new List<int>();
    for (int i = 0; i < QTEEventLength; i++) {
      QTEEvent.Add(random.Next(4));
    }
    return QTEEvent;
  }
}
