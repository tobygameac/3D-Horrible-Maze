using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeDemo2D : MonoBehaviour {

  public bool showButton;
  public int difficulty;

  public Texture startPointTexture;
  public Texture endPointTexture;
  public Texture availableBlockTexture;
  public Texture notAvailableBlockTexture;
  public Texture cornerBlockTexture;
  public Texture deadendBlockTexture;
  public Texture wallTexture;
  public Texture pathTexture;

  private const int R = 10;
  private const int C = 10;
  private BasicMaze maze;
  private List<MovingAction> path;

  void Start () {
    showButton = true;
    setMazeInformation();
  }

  void OnGUI () {
    int startX = Screen.width / 6;
    int width = (Screen.width - startX) / (C + 2);
    int height = Screen.height / (R + 2);
   
    GUILayout.BeginArea(new Rect(startX, 0, Screen.width - startX, Screen.height));
    
    for (int r = 0; r < R + 2; r++) {
      for (int c = 0; c < C + 2; c++) {
        Texture blockTexture = wallTexture;
        if (maze.isEmptyBlock(r, c)) {
          if (maze.isAvailableBlock(r, c)) {
            if (maze.isStartPoint(r, c)) {
              blockTexture = startPointTexture;
            } else if (maze.isEndPoint(r, c)) {
              blockTexture = endPointTexture;
            } else if (maze.isCornerBlock(r, c)) {
              blockTexture = cornerBlockTexture;
            } else if (maze.isDeadendBlock(r, c)) {
              blockTexture = deadendBlockTexture;
            } else {
              blockTexture = availableBlockTexture;
            }
          } else {
            blockTexture = notAvailableBlockTexture;
          }
        }
        GUI.DrawTexture(new Rect(c * width, r * height, width, height), blockTexture);
      }
    }

    if (path != null) {
      int r = maze.getStartPoint().r;
      int c = maze.getStartPoint().c;
      for (int i = 0; i < path.Count - 1; i++) {
        r = r + path[i].dr;
        c = c + path[i].dc;
        GUI.DrawTexture(new Rect(c * width, r * height, width, height), pathTexture);
      }
    }

    GUILayout.EndArea();

    if (showButton) {
      if (GUI.Button(new Rect(0, Screen.height - 100, 100, 30), "New easy map")) {
        difficulty = 0;
        setMazeInformation();
      }
      if (GUI.Button(new Rect(0, Screen.height - 70, 100, 30), "New hard map")) {
        difficulty = 1;
        setMazeInformation();
      }
      if (GUI.Button(new Rect(0, Screen.height - 40, 100, 30), "Exit")) {
        Application.LoadLevel("MainMenu");
      }
    }
  }

  private void drawLine(Vector2 pointA, Vector2 pointB, float width, Color color) {
    Matrix4x4 matrix = GUI.matrix;
 
    Texture2D lineTex = new Texture2D(1, 1);
 
    Color originalColor = GUI.color;
    GUI.color = color;
 
    float angle = Vector3.Angle(pointB - pointA, Vector2.right);
 
    if (pointA.y > pointB.y) {
      angle = -angle;
    }

    GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
    GUIUtility.RotateAroundPivot(angle, pointA);
    GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1, 1), lineTex);

    GUI.matrix = matrix;
    GUI.color = originalColor;
  }

  private void setMazeInformation () {
    GameState.difficulty = difficulty;
    maze = new BasicMaze(R, C);
    path = maze.getShortestPath(maze.getStartPoint(), maze.getEndPoint());
  }

}
