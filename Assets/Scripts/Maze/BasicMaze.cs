using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BasicMaze {

  public bool isDebugging = true;

  private static System.Random random = new System.Random(); // Only need one random seed

  private int R, C; // SIZE
  private Point startPoint = new Point();
  private Point endPoint = new Point();

  private List<List<bool>> blocks = new List<List<bool>>();
  private List<Point> availableBlocks = new List<Point>();

  public BasicMaze (int R, int C) {
    this.R = R;
    this.C = C;

    // +2 because of the border on both side
    for (int r = 0; r < R + 2; r++) {
      blocks.Add(new List<bool>());
      for (int c = 0; c < C + 2; c++) {
        blocks[r].Add(false);
      }
    }

    string mazeData = MazeData.getRandomMazeData(R, C);

    if (mazeData != null) {

      string[] token = mazeData.Split();

      startPoint.r = Convert.ToInt32(token[0]);
      startPoint.c = Convert.ToInt32(token[1]);

      // +2 because of the border on both side
      for (int r = 0; r < R + 2; r++) {
        for (int c = 0; c < C + 2; c++) {
          blocks[r][c] = (token[2][r * (C + 2) + c] == '0') ? false : true;
        }
      }

    } else {
      // Set random value on each block
      for (int r = 1; r <= R; r++) {
        for (int c = 1; c <= C; c++) {
          blocks[r][c] = (random.Next(2) == 0) ? false : true;
        }
      }
    }

    setFitness();
  }

  public BasicMaze (BasicMaze other) {
    R = other.R;
    C = other.C;
    startPoint = new Point(other.startPoint);
    endPoint = new Point(other.endPoint);
    blocks = new List<List<bool>>();
    for (int r = 0; r < other.blocks.Count; r++) {
      blocks.Add(new List<bool>());
      for (int c = 0; c < other.blocks[r].Count; c++) {
        blocks[r].Add(other.blocks[r][c]);
      }
    }
    availableBlocks = new List<Point>();
    for (int i = 0; i < other.availableBlocks.Count; i++) {
      availableBlocks.Add(other.availableBlocks[i]);
    }
    fitness = other.fitness;
    conerNumber = other.conerNumber;
    deadendNumber = other.deadendNumber;
    blocksSize = other.blocksSize;
  }

  public void log () {

    if (isDebugging) {
      Debug.Log("R = " + R + ", C = " + C);
      Debug.Log("startR = " + startPoint.r + ", startC = " + startPoint.c);
      Debug.Log("endR = " + endPoint.r + ", endC = " + endPoint.c);
      Debug.Log("Coner number = " + conerNumber);
      Debug.Log("Deadend number = " + deadendNumber);
      Debug.Log("Blocks size = " + blocksSize);
      Debug.Log("Fitness = " + fitness);
      /*
      for (int r = 0; r < blocks.Count; r++) {
        string temp = "";
        for (int c = 0; c < blocks[r].Count; c++) {
          temp += blocks[r][c] ? (r == startPoint.r && c == startPoint.c ? "*" : " ") : "X";
        }
        Debug.Log(temp + "-END");
      }
      */
    }
  }

  public Point getStartPoint () {
    return startPoint;
  }

  public Point getEndPoint () {
    return endPoint;
  }

  public void setBlock (int r, int c, bool b) {
    if (inMaze(r, c)) {
      blocks[r][c] = b;
    }
  }

  public bool inMaze (Point point) {
    return inMaze(point.r, point.c);
  }

  public bool inMaze (int r, int c) {
    return (r >= 1 && r <= R && c >= 1 && c <= C);
  }

  public bool getBlockState (int r, int c) {
    return inMaze(r, c) && blocks[r][c];
  }

  public bool getBlockState (Point point) {
    int r = point.r, c = point.c;
    return inMaze(r, c) && blocks[r][c];
  }

  public Point getRandomAvailableBlock () {
    if (availableBlocks.Count > 0) {
      return new Point(availableBlocks[random.Next(availableBlocks.Count)]);
    } else {
      if (isDebugging) {
        Debug.Log("No available blocks.");
      }
      return null;
    }
  }

  public List<MovingAction> getShortestPath (Point a, Point b) {
    return getShortestPath(a.r, a.c, b.r, b.c);
  }

  public List<MovingAction> getShortestPath (int r1, int c1, int r2, int c2) {

    if (!inMaze(r1, c1)) {
      if (isDebugging) {
        Debug.Log("r1 = " + r1 + ", c1 = " + c1);
      }
      return null;
    }

    if (!inMaze(r2, c2)) {
      if (isDebugging) {
        Debug.Log("r2 = " + r2 + ", c2 = " + c2);
      }
      return null;
    }

    // Shortest path of each coordinates
    List<List<List<MovingAction>>> movingActions = new List<List<List<MovingAction>>>();

    // Initial
    for (int r = 0; r < R + 2; r++) {
      movingActions.Add(new List<List<MovingAction>>());
      for (int c = 0; c < C + 2; c++) {
        movingActions[r].Add(new List<MovingAction>());
      }
    }

    // Visit status of each coordinates
    List<List<bool>> visited = new List<List<bool>>();
    
    for (int r = 0; r < R + 2; r++) {
      visited.Add(new List<bool>());
      for (int c = 0; c < C + 2; c++) {
        visited[r].Add(false);
      }
    }

    visited[r1][c1] = true;

    Queue<Point> q = new Queue<Point>();
    q.Enqueue(new Point(r1, c1));

    // Basic move
    int[] drB = new int[]{1, 0, -1, 0};
    int[] dcB = new int[]{0, 1, 0, -1};

    // Diagonal move
    int[] drD = new int[]{1, -1, -1, 1};
    int[] dcD = new int[]{1, 1, -1, -1};

    // BFS
    while (q.Count > 0) {
      
      Point now = q.Dequeue();
      int r = now.r;
      int c = now.c;
      List<MovingAction> path = movingActions[r][c];

      if (r == r2 && c == c2) {
        return path;
      }

      // Basic move
      for (int d = 0; d < 4; d++) {
        int nr = r + drB[d], nc = c + dcB[d];
        if (getBlockState(nr, nc) && !visited[nr][nc]) {
          q.Enqueue(new Point(nr, nc));
          visited[nr][nc] = true;
          for (int i = 0; i < path.Count; i++) {
            movingActions[nr][nc].Add(new MovingAction(path[i]));
          }
          movingActions[nr][nc].Add(new MovingAction(drB[d], dcB[d]));
        }
      }

      // Diagonal move
      for (int d = 0; d < 4; d++) {
        int nr = r + drD[d], nc = c + dcD[d];
        if (getBlockState(nr, nc) && !visited[nr][nc]) {

          // Check basic directions which next to the diagonal direction
          int d1 = d, d2 = (d + 1) % 4;
          int nr1 = r + drB[d1], nc1 = c + dcB[d1];
          int nr2 = r + drB[d2], nc2 = c + dcB[d2];

          // Moving if both basic blocks are avalible
          if (getBlockState(nr1, nc1) && getBlockState(nr2, nc2)) {
            q.Enqueue(new Point(nr, nc));
            visited[nr][nc] = true;
            movingActions[nr][nc] = new List<MovingAction>(path);
            movingActions[nr][nc].Add(new MovingAction(drD[d], dcD[d]));
          }
        }
      }

    }

    return movingActions[r2][c2];
  }

  private int fitness;
  private int conerNumber;
  private int deadendNumber;
  private int blocksSize;

  public int getFitness () {
    return fitness;
  }

  public void setFitness () {
    setBlocksSize();
    setConerNumber();
    setDeadendNumber();
    fitness = conerNumber * 5 + deadendNumber * 15 + blocksSize * 2;
  }

  private void setBlocksSize () {
    
    availableBlocks = new List<Point>();

    if (startPoint.r == 0 || startPoint.c == 0) {
      // Find the start point

      int findCount = 0;

      do {
        startPoint = new Point(random.Next(R) + 1, random.Next(C) + 1);
        findCount++;

        // Unable to find a start point in 100 times of loop
        if (findCount == 100) {
          break;
        }

      } while (!blocks[startPoint.r][startPoint.c]);
    }

    blocks[startPoint.r][startPoint.c] = true;

    List<List<bool>> visited = new List<List<bool>>();

    for (int r = 0; r < R + 2; r++) {
      visited.Add(new List<bool>());
      for (int c = 0; c < C + 2; c++) {
        visited[r].Add(false);
      }
    }

    visited[startPoint.r][startPoint.c] = true;

    // Find all possible block

    Queue<Point> q = new Queue<Point>();
    q.Enqueue(new Point(startPoint.r, startPoint.c));

    // Move dirction
    int[] dr = new int[4]{1, 0, -1, 0};
    int[] dc = new int[4]{0, 1, 0, -1};

    // BFS
    while (q.Count > 0) {
      Point now = q.Dequeue();
      availableBlocks.Add(new Point(now));
      int r = now.r;
      int c = now.c;
      for (int d = 0; d < 4; d++) {
        int nr = r + dr[d], nc = c + dc[d];
        if (getBlockState(nr, nc) && !visited[nr][nc]) {
          q.Enqueue(new Point(nr, nc));
          visited[nr][nc] = true;
        }
      }

      if (q.Count == 0) {
        endPoint = new Point(r, c);
      }
    }

    blocksSize = availableBlocks.Count;

    /*
    // Clear unreachable blocks
    for (int r = 1; r <= R; r++) {
      for (int c = 1; c <= C; c++) {
        if (!visited[r][c]) {
          blocks[r][c] = false;
        }
      }
    }
    */
  }


  private void setConerNumber () {
    List<List<bool>> isConer = new List<List<bool>>();

    for (int r = 0; r < R + 2; r++) {
      isConer.Add(new List<bool>());
      for (int c = 0; c < C + 2; c++) {
        isConer[r].Add(false);
      }
    }

    // right and up
    for (int r = 2; r <= R; r++) {
      for (int c = 1; c <= C - 2; c++) {
        if (blocks[r][c] && blocks[r][c + 1] && blocks[r][c + 2]) {
          if (!blocks[r - 1][c] && !blocks[r - 1][c + 1] && blocks[r - 1][c + 2]) {
            isConer[r][c + 2] = true;
          }
        }
      }
    }

    // right and down
    for (int r = 1; r <= R - 1; r++) {
      for (int c = 1; c <= C - 2; c++) {
        if (blocks[r][c] && blocks[r][c + 1] && blocks[r][c + 2]) {
          if (!blocks[r + 1][c] && !blocks[r + 1][c + 1] && blocks[r + 1][c + 2]) {
            isConer[r][c + 2] = true;
          }
        }
      }
    }

    // left and up
    for (int r = 2; r <= R; r++) {
      for (int c = 3; c <= C; c++) {
        if (blocks[r][c] && blocks[r][c - 1] && blocks[r][c - 2]) {
          if (!blocks[r - 1][c] && !blocks[r - 1][c - 1] && blocks[r - 1][c - 2]) {
            isConer[r][c - 2] = true;
          }
        }
      }
    }

    // left and down
    for (int r = 1; r <= R - 1; r++) {
      for (int c = 3; c <= C; c++) {
        if (blocks[r][c] && blocks[r][c - 1] && blocks[r][c - 2]) {
          if (!blocks[r + 1][c] && !blocks[r + 1][c - 1] && blocks[r + 1][c - 2]) {
            isConer[r][c - 2] = true;
          }
        }
      }
    }

    // up and right
    for (int r = 3; r <= R; r++) {
      for (int c = 1; c <= C - 1; c++) {
        if (blocks[r][c] && blocks[r - 1][c] && blocks[r - 2][c]) {
          if (!blocks[r][c + 1] && !blocks[r - 1][c + 1] && blocks[r - 2][c + 1]) {
            isConer[r - 2][c] = true;
          }
        }
      }
    }

    // up and left
    for (int r = 3; r <= R; r++) {
      for (int c = 2; c <= C; c++) {
        if (blocks[r][c] && blocks[r - 1][c] && blocks[r - 2][c]) {
          if (!blocks[r][c - 1] && !blocks[r - 1][c - 1] && blocks[r - 2][c - 1]) {
            isConer[r - 2][c] = true;
          }
        }
      }
    }

    // down and right
    for (int r = 1; r <= R - 2; r++) {
      for (int c = 1; c <= C - 1; c++) {
        if (blocks[r][c] && blocks[r + 1][c] && blocks[r + 2][c]) {
          if (!blocks[r][c + 1] && !blocks[r + 1][c + 1] && blocks[r + 2][c + 1]) {
            isConer[r + 2][c] = true;
          }
        }
      }
    }

    // down and left
    for (int r = 1; r <= R - 2; r++) {
      for (int c = 2; c <= C; c++) {
        if (blocks[r][c] && blocks[r + 1][c] && blocks[r + 2][c]) {
          if (!blocks[r][c - 1] && !blocks[r + 1][c - 1] && blocks[r + 2][c - 1]) {
            isConer[r + 2][c] = true;
          }
        }
      }
    }

    int count = 0;
    for (int r = 1; r <= R; r++) {
      for (int c = 1; c <= C; c++) {
        count += isConer[r][c] ? 1 : 0;
      }
    }

    conerNumber = count;
  }

  private void setDeadendNumber () {
    List<List<bool>> isDeadend = new List<List<bool>>();

    for (int r = 0; r < R + 2; r++) {
      isDeadend.Add(new List<bool>());
      for (int c = 0; c < C + 2; c++) {
        isDeadend[r].Add(false);
      }
    }

    // right
    for (int r = 1; r <= R - 1; r++) {
      for (int c = 1; c <= C - 3; c++) {
        if (!blocks[r - 1][c] && !blocks[r - 1][c + 1] && !blocks[r - 1][c + 2]) {
          if (!blocks[r + 1][c] && !blocks[r + 1][c + 1] && !blocks[r + 1][c + 2]) {
            if (blocks[r][c] && blocks[r][c + 1] && blocks[r][c + 2] && !blocks[r][c + 3]) {
              isDeadend[r][c + 2] = true;
            }
          }
        }
      }
    }

    // left
    for (int r = 1; r <= R - 1; r++) {
      for (int c = 3; c <= C; c++) {
        if (!blocks[r - 1][c] && !blocks[r - 1][c - 1] && !blocks[r - 1][c - 2]) {
          if (!blocks[r + 1][c] && !blocks[r + 1][c - 1] && !blocks[r + 1][c - 2]) {
            if (blocks[r][c] && blocks[r][c - 1] && blocks[r][c - 2] && !blocks[r][c - 3]) {
              isDeadend[r][c - 2] = true;
            }
          }
        }
      }
    }

    // up
    for (int r = 3; r <= R; r++) {
      for (int c = 1; c <= C; c++) {
        if (!blocks[r][c - 1] && !blocks[r - 1][c - 1] && !blocks[r - 2][c - 1]) {
          if (!blocks[r][c + 1] && !blocks[r - 1][c + 1] && !blocks[r - 2][c + 1]) {
            if (blocks[r][c] && blocks[r - 1][c] && blocks[r - 2][c] && !blocks[r - 3][c]) {
              isDeadend[r - 2][c] = true;
            }
          }
        }
      }
    }

    // down
    for (int r = 1; r <= R - 3; r++) {
      for (int c = 1; c <= C; c++) {
        if (!blocks[r][c - 1] && !blocks[r + 1][c - 1] && !blocks[r + 2][c - 1]) {
          if (!blocks[r][c + 1] && !blocks[r + 1][c + 1] && !blocks[r + 2][c + 1]) {
            if (blocks[r][c] && blocks[r + 1][c] && blocks[r + 2][c] && !blocks[r + 3][c]) {
              isDeadend[r + 2][c] = true;
            }
          }
        }
      }
    }

    int count = 0;
    for (int r = 1; r <= R; r++) {
      for (int c = 1; c <= C; c++) {
        count += isDeadend[r][c] ? 1 : 0;
      }
    }

    deadendNumber = count;
  }

}
