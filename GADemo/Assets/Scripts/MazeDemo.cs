using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeDemo : MonoBehaviour {

  public bool showButton;
  private bool highlight;

  public Texture emptyBlockTexture;
  public Texture wallTexture;
  public Texture patternEmptyBlockTexture;
  public Texture patternWallTexture;

  public Texture startPointTexture;
  public Texture endPointTexture;
  public Texture availableBlockTexture;
  public Texture notAvailableBlockTexture;
  public Texture cornerBlockTexture;
  public Texture deadendBlockTexture;
  public Texture pathTexture;

  private int R;
  private int C;

  private MazeGenerator mazeGenerator;
  private BasicMaze maze;

  public float refreshTime;
  private float refreshCountDown;

  void Start () {
    mazeGenerator = Camera.main.GetComponent<MazeGenerator>();
    R = mazeGenerator.MAZE_R;
    C = mazeGenerator.MAZE_C;
    showButton = true;
    highlight = false;
    setMazeInformation();
    DemoState.state = DemoState.PLAYING;
    DemoState.showAllBest = false;
    refreshCountDown = refreshTime;
  }

  void Update () {
    if (DemoState.autoRefresh) {
      refreshCountDown -= Time.deltaTime;
      if (refreshCountDown <= 0) {
        Application.LoadLevel(Application.loadedLevel);
      }
    }
  }

  void OnGUI () {
    if (showButton) {
      string modeString = null;
      switch (DemoState.mode) {
       case DemoState.SIMPLE:
        modeString = "Game maze mode";
        break;
       case DemoState.MAZE:
        modeString = "Simple mode";
        break;
      }
      GUI.Label(new Rect(0, Screen.height - 425, 150, 30), "fitness : " + maze.getFitness().ToString ("0.00"));
      if (GUI.Button(new Rect(0, Screen.height - 350, 150, 30), modeString)) {
        DemoState.mode = (DemoState.mode + 1) % 2;
        maze.setFitness();
      }
      Color originalColor = GUI.color;
      if (highlight) {
        GUI.color = Color.green;
      }
      if (DemoState.mode == DemoState.SIMPLE) {
        if (GUI.Button(new Rect(0, Screen.height - 400, 150, 30), "Pattern highlight")) {
          highlight = !highlight;
        }
      }
      GUI.color = originalColor;
      string showBestString = null;
      if (DemoState.showAllBest) {
        showBestString = "Show current best";
      } else {
        showBestString = "Show all best";
      }
      if (GUI.Button(new Rect(0, Screen.height - 300, 150, 30), showBestString)) {
        DemoState.showAllBest = !DemoState.showAllBest;
      }
      if (DemoState.autoRefresh) {
        GUI.Label(new Rect(0, Screen.height - 210, 150, 30), "Refresh in " + refreshCountDown.ToString("0"));
        GUI.color = Color.green;
      }
      if (GUI.Button(new Rect(0, Screen.height - 180, 150, 30), "Auto Refresh")) {
        DemoState.autoRefresh = !DemoState.autoRefresh;
        refreshCountDown = refreshTime;
      }
      GUI.color = originalColor;
      if (GUI.Button(new Rect(0, Screen.height - 150, 150, 30), "Restart")) {
        Application.LoadLevel(Application.loadedLevel);
      }
      switch (DemoState.state) {
       case DemoState.PLAYING:
        if (GUI.Button(new Rect(0, Screen.height - 70, 150, 30), "Pause")) {
          DemoState.state = DemoState.PAUSING;
        }
        break;
       case DemoState.PAUSING:
        if (GUI.Button(new Rect(0, Screen.height - 70, 150, 30), "Start")) {
          DemoState.state = DemoState.PLAYING;
        }
        break;
      }
      if (GUI.Button(new Rect(0, Screen.height - 40, 150, 30), "Exit")) {
        Application.Quit();
      }
    }

    if (maze == null) {
      return;  
    }

    int startX = Screen.width / 6;
    int width = (Screen.width - startX) / (C + 2);
    int height = Screen.height / (R + 2);

    switch (DemoState.mode) {
     case DemoState.SIMPLE:
      for (int r = 0; r < R + 2; r++) {
        for (int c = 0; c < C + 2; c++) {
          Texture blockTexture = wallTexture;
          if (maze.isEmptyBlock(r, c)) {
            blockTexture = emptyBlockTexture;
            if (highlight && maze.isPatternBlock(r, c)) {
              blockTexture = patternEmptyBlockTexture;
            }
          } else {
            if (highlight && maze.isPatternBlock(r, c)) {
              blockTexture = patternWallTexture;
            }
          }
          GUI.DrawTexture(new Rect(startX + c * width, r * height, width, height), blockTexture);
        }
      }
      break;
     case DemoState.MAZE:
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
          GUI.DrawTexture(new Rect(startX + c * width, r * height, width, height), blockTexture);
        }
      }
      List<MovingAction> path = maze.getShortestPath(maze.getStartPoint(), maze.getEndPoint());
      if (path != null) {
        int r = maze.getStartPoint().r;
        int c = maze.getStartPoint().c;
        for (int i = 0; i < path.Count - 1; i++) {
          r = r + path[i].dr;
          c = c + path[i].dc;
          GUI.DrawTexture(new Rect(startX + c * width, r * height, width, height), pathTexture);
        }
      }
      break;
    }
  }

  private void setMazeInformation () {
    StartCoroutine(mazeGenerator.generateBasicMaze(value => maze = value));
  }

}
