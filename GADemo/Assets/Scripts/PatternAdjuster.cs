using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternAdjuster : MonoBehaviour {

  public int R;
  public int C;

  public static List<List<bool>> pattern;

  void Start () {
    pattern = new List<List<bool>>();
    for (int r = 0; r < R; r++) {
      pattern.Add(new List<bool>());
      for (int c = 0; c < C; c++) {
        pattern[r].Add(false);
      }
    }
  }

  void OnGUI () {
    int size = Screen.width / 6;
    int width = size / C;
    int height = size / R;
    for (int r = 0; r < R; r++) {
      for (int c = 0; c < C; c++) {
        Color originalColor = GUI.color;
        Color newColor;
        string sign;
        if (pattern[r][c]) {
          sign = "O";
          newColor = Color.green;
        } else {
          sign = "X";
          newColor = Color.black;
        }
        GUI.color = newColor;
        if (GUI.Button(new Rect(c * width, r * height, width, height), sign)) {
          pattern[r][c] = !pattern[r][c];
        }
        GUI.color = originalColor;
      }
    }
  }

}
