using UnityEngine;
using System.Collections;

public class MazeDemo2D : MonoBehaviour {

  public Texture startPointTexture;
  public Texture availableBlockTexture;
  public Texture notAvailableBlockTexture;
  public Texture cornerBlockTexture;
  public Texture deadendBlockTexture;
  public Texture wallTexture;

  private const int R = 10;
  private const int C = 10;
  private BasicMaze maze;

  void Start () {
    maze = new BasicMaze(R, C);
  }

  void OnGUI () {
    int width = Screen.width / (C + 2);
    int height = Screen.height / (R + 2);
    for (int r = 0; r < R + 2; r++) {
      for (int c = 0; c < C + 2; c++) {
        Texture blockTexture = wallTexture;
        if (maze.isEmptyBlock(r, c)) {
          if (maze.isAvailableBlock(r, c)) {
            if (maze.isStartPoint(r, c)) {
              blockTexture = startPointTexture;
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
        GUI.DrawTexture(new Rect(r * width, c * height, width, height), blockTexture);
      }
    }
  }

}
