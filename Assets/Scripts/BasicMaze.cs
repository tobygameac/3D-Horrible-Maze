using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BasicMaze {

  private static System.Random random = new System.Random(); // Only need one random seed

  private int R, C; // SIZE
  private Point startPoint = new Point();
  private Point endPoint = new Point();

  private List<List<bool>> blocks = new List<List<bool>>();

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

    // Set random value on each block
    for (int r = 1; r <= R; r++) {
      for (int c = 1; c <= C; c++) {
        blocks[r][c] = (random.Next(2) == 0) ? false : true;
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
    fitness = other.fitness;
    conerNumber = other.conerNumber;
    deadendNumber = other.deadendNumber;
    blocksSize = other.blocksSize;
  }

  public void log () {
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

  public Point getStartPoint () {
    return startPoint;
  }

  public Point getEndPoint () {
    return endPoint;
  }

  public void setBlock (int r, int c, bool b) {
    if (r >= 1 && r <= R && c >= 1 && c <= R) {
      blocks[r][c] = b;
    }
  }

  public bool inMaze (int r, int c) {
    return (r >= 1 && r <= R && c >= 1 && c <= R);
  }

  public bool inMaze (Point point) {
    int r = point.r, c = point.c;
    return (r >= 1 && r <= R && c >= 1 && c <= R);
  }

  public bool getBlock (int r, int c) {
    return inMaze(r, c) && blocks[r][c];
  }

  public bool getBlock (Point point) {
    int r = point.r, c = point.c;
    return inMaze(r, c) && blocks[r][c];
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

    // Find the start point

    int findCount = 0;

    do {
      startPoint.r = random.Next(R) + 1;
      startPoint.c = random.Next(C) + 1;
      findCount++;

      // Unable to find a start point in 100 times of loop
      if (findCount == 100) {
        blocks[startPoint.r][startPoint.c] = true;
      }

    } while (!blocks[startPoint.r][startPoint.c]);

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
    int count = 0;
    while (q.Count > 0) {
      count++;
      Point now = q.Dequeue();
      int r = now.r;
      int c = now.c;
      for (int d = 0; d < 4; d++) {
        int nr = r + dr[d], nc = c + dc[d];
        if (getBlock(nr, nc) && !visited[nr][nc]) {
          q.Enqueue(new Point(nr, nc));
          visited[nr][nc] = true;
        }
      }

      if (q.Count == 0) {
        endPoint.r = r;
        endPoint.c = c;
      }
    }

    blocksSize = count;

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
