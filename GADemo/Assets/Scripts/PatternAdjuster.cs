using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternAdjuster : MonoBehaviour {

  public int R;
  public int C;

  public static List<List<int>> pattern;

  void Start () {
    pattern = new List<List<int>>();
    for (int r = 0; r < R; r++) {
      pattern.Add(new List<int>());
      for (int c = 0; c < C; c++) {
        pattern[r].Add(0);
      }
    }
  }

  void OnGUI () {
    if (DemoState.mode != DemoState.SIMPLE) {
      return;
    }
    int size = Screen.width / 6;
    int width = size / C;
    int height = size / R;
    for (int r = 0; r < R; r++) {
      for (int c = 0; c < C; c++) {
        Color originalColor = GUI.color;
        Color newColor;
        string sign;
        if (pattern[r][c] == 0) {
          sign = "X";
          newColor = Color.black;
        } else if (pattern[r][c] == 1) {
          sign = "O";
          newColor = Color.green;
        } else {
          sign = "?";
          newColor = Color.yellow;
        }
        GUI.color = newColor;
        if (GUI.Button(new Rect(c * width, r * height, width, height), sign)) {
          pattern[r][c] = (pattern[r][c] + 1) % 3;
        }
        GUI.color = originalColor;
      }
    }
  }

}
