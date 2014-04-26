using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BasicMaze {

  public bool isDebugging = true;

  private static System.Random random = new System.Random(); // Only need one random seed

  private int R, C;
  private Point startPoint;
  private Point endPoint;

  private List<List<bool>> blocks;
  private List<Point> availableBlocks;
  private List<List<bool>> isAvailable;
  private List<List<bool>> isCorner;
  private List<List<bool>> isDeadend;

  private double fitness;
  private int cornerNumber;
  private int deadendNumber;
  private int blocksSize;

  public BasicMaze (int R, int C) {
    this.R = R;
    this.C = C;
    
    blocks = new List<List<bool>>();
    // +2 because of the border on both side
    for (int r = 0; r < R + 2; r++) {
      blocks.Add(new List<bool>());
      for (int c = 0; c < C + 2; c++) {
        blocks[r].Add(false);
      }
    }

    startPoint = new Point();
    endPoint = new Point();

    string mazeData = MazeData.getRandomMazeData(R, C, GameState.difficulty);

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
    isCorner = new List<List<bool>>();
    for (int r = 0; r < other.isCorner.Count; r++) {
      isCorner.Add(new List<bool>());
      for (int c = 0; c < other.isCorner[r].Count; c++) {
        isCorner[r].Add(other.isCorner[r][c]);
      }
    }
    isDeadend = new List<List<bool>>();
    for (int r = 0; r < other.isDeadend.Count; r++) {
      isDeadend.Add(new List<bool>());
      for (int c = 0; c < other.isDeadend[r].Count; c++) {
        isDeadend[r].Add(other.isDeadend[r][c]);
      }
    }
    fitness = other.fitness;
    cornerNumber = other.cornerNumber;
    deadendNumber = other.deadendNumber;
    blocksSize = other.blocksSize;
    isAvailable = new List<List<bool>>();
    for (int r = 0; r < other.isAvailable.Count; r++) {
      isAvailable.Add(new List<bool>());
      for (int c = 0; c < other.isAvailable[r].Count; c++) {
        isAvailable[r].Add(other.isAvailable[r][c]);
      }
    }
  }

  public void log () {

    if (isDebugging) {
      Debug.Log("R = " + R + ", C = " + C);
      Debug.Log("startR = " + startPoint.r + ", startC = " + startPoint.c);
      Debug.Log("endR = " + endPoint.r + ", endC = " + endPoint.c);
      Debug.Log("corner number = " + cornerNumber);
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
  
  public bool isStartPoint (int r, int c) {
    return inMaze(r, c) && r == startPoint.r && c == startPoint.c;
  }

  public bool isStartPoint (Point point) {
    int r = point.r, c = point.c;
    return inMaze(r, c) && r == startPoint.r && c == startPoint.c;
  }

  public bool isEndPoint (int r, int c) {
    return inMaze(r, c) && r == endPoint.r && c == endPoint.c;
  }

  public bool isEndPoint (Point point) {
    int r = point.r, c = point.c;
    return inMaze(r, c) && r == endPoint.r && c == endPoint.c;
  }

  public bool isEmptyBlock (int r, int c) {
    return inMaze(r, c) && blocks[r][c];
  }

  public bool isEmptyBlock (Point point) {
    int r = point.r, c = point.c;
    return inMaze(r, c) && blocks[r][c];
  }

  public bool isAvailableBlock (int r, int c) {
    return inMaze(r, c) && isAvailable[r][c];
  }
  
  public bool isAvailableBlock (Point point) {
    int r = point.r, c = point.c;
    return inMaze(r, c) && isAvailable[r][c];
  }

  public bool isCornerBlock (int r, int c) {
    return inMaze(r, c) && isCorner[r][c];
  }
  
  public bool isCornerBlock (Point point) {
    int r = point.r, c = point.c;
    return inMaze(r, c) && isCorner[r][c];
  }

  public bool isDeadendBlock (int r, int c) {
    return inMaze(r, c) && isDeadend[r][c];
  }
  
  public bool isDeadendBlock (Point point) {
    int r = point.r, c = point.c;
    return inMaze(r, c) && isDeadend[r][c];
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
        if (isEmptyBlock(nr, nc) && !visited[nr][nc]) {
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
        if (isEmptyBlock(nr, nc) && !visited[nr][nc]) {

          // Check basic directions which next to the diagonal direction
          int d1 = d, d2 = (d + 1) % 4;
          int nr1 = r + drB[d1], nc1 = c + dcB[d1];
          int nr2 = r + drB[d2], nc2 = c + dcB[d2];

          // Moving if both basic blocks are avalible
          if (isEmptyBlock(nr1, nc1) && isEmptyBlock(nr2, nc2)) {
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

  public double getFitness () {
    return fitness;
  }

  public void setFitness () {
    setBlocksSize();
    setCornerNumber();
    setDeadendNumber();
    fitness = 0;
    if (blocksSize != 0) {
      fitness = (cornerNumber * deadendNumber) / (double)blocksSize;
      fitness *= Mathf.Sqrt(blocksSize / (float)(R * C));
    }
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

    isAvailable = new List<List<bool>>();
    
    for (int r = 0; r < R + 2; r++) {
      isAvailable.Add(new List<bool>());
      for (int c = 0; c < C + 2; c++) {
        isAvailable[r].Add(false);
      }
    }
    
    isAvailable[startPoint.r][startPoint.c] = true;

    // Find all isAvailable block

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
        if (isEmptyBlock(nr, nc) && !isAvailable[nr][nc]) {
          q.Enqueue(new Point(nr, nc));
          isAvailable[nr][nc] = true;
        }
      }

      if (q.Count == 0) {
        endPoint = new Point(r, c);
      }
    }

    blocksSize = availableBlocks.Count;

  }


  private void setCornerNumber () {
    
    isCorner = new List<List<bool>>();

    for (int r = 0; r < R + 2; r++) {
      isCorner.Add(new List<bool>());
      for (int c = 0; c < C + 2; c++) {
        isCorner[r].Add(false);
      }
    }

    int[] dr = new int[]{1, 0, -1, 0};
    int[] dc = new int[]{0, 1, 0, -1};

    for (int r = 1; r <= R; r++) {
      for (int c = 1; c <= C; c++) {
        int nbdCount = 0;
        // Count four directions
        for (int d = 0; d < 4; d++) {
          if (blocks[r + dr[d]][c + dc[d]]) {
            nbdCount++;
          }
        }
        // Count eight directions
        if (nbdCount == 3) {
          nbdCount = 0;
          for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
              if ((i == 0) && (j == 0)) {
                continue;
              }
              if (blocks[r + i][c + j]) {
                nbdCount++;
              }
            }
          }
          if (nbdCount == 3) {
            isCorner[r][c] = true;
          }
        }
      }
    }

    int count = 0;
    for (int r = 1; r <= R; r++) {
      for (int c = 1; c <= C; c++) {
        count += (isCorner[r][c] && isAvailable[r][c]) ? 1 : 0;
      }
    }

    cornerNumber = count;
  }

  private void setDeadendNumber () {
    
    isDeadend = new List<List<bool>>();

    for (int r = 0; r < R + 2; r++) {
      isDeadend.Add(new List<bool>());
      for (int c = 0; c < C + 2; c++) {
        isDeadend[r].Add(false);
      }
    }

    int[] dr = new int[]{1, 0, -1, 0};
    int[] dc = new int[]{0, 1, 0, -1};

    for (int r = 1; r <= R; r++) {
      for (int c = 1; c <= C; c++) {
        int nbdCount = 0, emptyDir = 0;
        // Count four directions
        for (int d = 0; d < 4; d++) {
          if (blocks[r + dr[d]][c + dc[d]]) {
            nbdCount++;
            emptyDir = d;
          }
        }
        if (nbdCount == 1) {
          nbdCount = 0;
          int d = emptyDir;
          for (int i = -1; i <= 1; i += 2) {
            for (int j = -1; j <= 1; j += 2) {
              int nr = r + dr[d];
              int nc = c + dc[d];
              if (dr[d] == 0) {
                nr += i;
              }
              if (dc[d] == 0) {
                nc += j;
              }
              if (!blocks[nr][nc]) {
                nbdCount++;
              }
            }
          }
          if (nbdCount == 4) {
            isDeadend[r][c] = true;
          }
        }
      }
    }

    int count = 0;
    for (int r = 1; r <= R; r++) {
      for (int c = 1; c <= C; c++) {
        count += (isDeadend[r][c] && isAvailable[r][c]) ? 1 : 0;
      }
    }

    deadendNumber = count;
  }

}
