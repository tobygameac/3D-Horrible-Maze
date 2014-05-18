using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BasicMaze {

  private static System.Random random = new System.Random(); // Only need one random seed
  
  private int R;
  private int C;

  private List<List<bool>> blocks;
  private List<List<bool>> isPattern;

  private double fitness;
  private int patternNumber;

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

    isPattern = new List<List<bool>>();
    // +2 because of the border on both side
    for (int r = 0; r < R + 2; r++) {
      isPattern.Add(new List<bool>());
      for (int c = 0; c < C + 2; c++) {
        isPattern[r].Add(false);
      }
    }

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
    blocks = new List<List<bool>>();
    for (int r = 0; r < other.blocks.Count; r++) {
      blocks.Add(new List<bool>());
      for (int c = 0; c < other.blocks[r].Count; c++) {
        blocks[r].Add(other.blocks[r][c]);
      }
    }
    isPattern = new List<List<bool>>();
    for (int r = 0; r < other.isPattern.Count; r++) {
      isPattern.Add(new List<bool>());
      for (int c = 0; c < other.isPattern[r].Count; c++) {
        isPattern[r].Add(other.isPattern[r][c]);
      }
    }
    fitness = other.fitness;
    patternNumber = other.patternNumber;
  }

  public void setBlock (int r, int c, bool b) {
    if (inMaze(r, c)) {
      blocks[r][c] = b;
    }
  }

  public bool inMaze (int r, int c) {
    return (r >= 1 && r <= R && c >= 1 && c <= C);
  }
  
  public bool isEmptyBlock (int r, int c) {
    return inMaze(r, c) && blocks[r][c];
  }

  public bool isPatternBlock (int r, int c) {
    return inMaze(r, c) && isPattern[r][c];
  }

  public double getFitness () {
    return fitness;
  }

  public void setFitness () {
    setPatternNumber();
    fitness = patternNumber * 1.0f;
  }

  private void setPatternNumber () {
    
    patternNumber = 0;

    for (int r = 0; r < blocks.Count; r++) {
      for (int c = 0; c < blocks[0].Count; c++) {
        isPattern[r][c] = false;
      }
    }

    if (PatternAdjuster.pattern == null || PatternAdjuster.pattern.Count == 0) {
      return;
    }

    int pr = PatternAdjuster.pattern.Count;
    int pc = PatternAdjuster.pattern[0].Count;

    for (int r = 0; r + pr - 1 < blocks.Count; r++) {
      for (int c = 0; c + pc - 1 < blocks[0].Count; c++) {
        bool ok = true;
        for (int dr = 0; ok && dr < pr; dr++) {
          for (int dc = 0; ok && dc < pc; dc++) {
            ok = (PatternAdjuster.pattern[dr][dc] == blocks[r + dr][c + dc]);
          }
        }
        if (ok) {
          for (int dr = 0; dr < pr; dr++) {
            for (int dc = 0; dc < pc; dc++) {
              isPattern[r + dr][c + dc] = true;
            }
          }
          patternNumber++;
        }
      }
    }

  }

}
