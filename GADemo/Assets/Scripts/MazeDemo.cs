using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeDemo : MonoBehaviour {

  public bool showButton;

  public Texture emptyBlockTexture;
  public Texture wallTexture;
  public Texture patternEmptyBlockTexture;
  public Texture patternWallTexture;

  public int R;
  public int C;

  private MazeGenerator mazeGenerator;
  private BasicMaze maze;

  void Start () {
    mazeGenerator = Camera.main.GetComponent<MazeGenerator>();
    showButton = true;
  }

  void OnGUI () {
    if (showButton) {
      if (GUI.Button(new Rect(0, Screen.height - 90, 100, 30), "Generate")) {
        setMazeInformation();
      }
      if (GUI.Button(new Rect(0, Screen.height - 30, 100, 30), "Exit")) {
        Application.LoadLevel("MainMenu");
      }
    }

    if (maze == null) {
      return;  
    }

    int startX = Screen.width / 6;
    int width = (Screen.width - startX) / (C + 2);
    int height = Screen.height / (R + 2);
    for (int r = 0; r < R + 2; r++) {
      for (int c = 0; c < C + 2; c++) {
        Texture blockTexture = wallTexture;
        if (maze.isEmptyBlock(r, c)) {
          blockTexture = emptyBlockTexture;
          if (maze.isPatternBlock(r, c)) {
            blockTexture = patternEmptyBlockTexture;
          }
        } else {
          if (maze.isPatternBlock(r, c)) {
            blockTexture = patternWallTexture;
          }
        }
        GUI.DrawTexture(new Rect(startX + c * width, r * height, width, height), blockTexture);
      }
    }
  }

  private void setMazeInformation () {
    maze = mazeGenerator.generateBasicMaze();
  }

}
