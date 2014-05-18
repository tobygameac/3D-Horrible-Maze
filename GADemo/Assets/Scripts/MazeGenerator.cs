using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour {

  public int MAZE_R;
  public int MAZE_C;

  private static System.Random random = new System.Random(); // Only need one random seed

  public int NUM_OF_POPULATION;
  public int MAXIMUM_TIMES_OF_ITERATION;
  public double TARGET_FITNESS;

  private BasicMaze best;
  private BasicMaze allBest;

  private List<BasicMaze> population;
  private List<BasicMaze> crossoverPool;

  void OnGUI () {
    MAXIMUM_TIMES_OF_ITERATION = (int)GUI.HorizontalScrollbar(new Rect(0, Screen.height - 120, 100, 30), MAXIMUM_TIMES_OF_ITERATION, 10, 0, 5000);
  }

  public BasicMaze generateBasicMaze () {
    initial();
    for (int times = 1; times <= MAXIMUM_TIMES_OF_ITERATION; times++) {
      reproduction();
      crossover();
      mutation();
      if (best.getFitness() >= TARGET_FITNESS) {
        break;
      }
    }
    return best;
  }

  private void initial() {
    population = new List<BasicMaze>();
    crossoverPool = new List<BasicMaze>();
    for (int num = 0; num < NUM_OF_POPULATION; num++) {
      population.Add(new BasicMaze(MAZE_R, MAZE_C));
      if (num == 0 || (population[population.Count - 1].getFitness() > best.getFitness())) {
        best = new BasicMaze(population[population.Count - 1]);
        allBest = new BasicMaze(best);
      }
    }
  }

  private void reproduction () {
    double fitnessSum = 0;
    for (int i = 0; i < population.Count; i++) {
      fitnessSum += population[i].getFitness();
    }

    crossoverPool.Clear();

    int remain = population.Count;
    if (fitnessSum != 0) {
      for (int i = 0; i < population.Count && remain > 0; i++) {
        int need = (int)(population[i].getFitness() / fitnessSum + 0.5);
        need = need > remain ? remain : need;
        remain -= need;
        while (need > 0) {
          need--;
          crossoverPool.Add(new BasicMaze(population[i]));
        }
      }
    }

    // Choose by random
    while (remain > 0) {
      remain--;
      int index1 = random.Next(population.Count);
      int index2;
      do {
        index2 = random.Next(population.Count);
      } while (index1 == index2);

      if (population[index1].getFitness() > population[index2].getFitness()) {
        crossoverPool.Add(new BasicMaze(population[index1]));
      } else {
        crossoverPool.Add(new BasicMaze(population[index2]));
      }
    }
  }

  private void crossover () {
    population.Clear();

    while (population.Count < NUM_OF_POPULATION) {
      int index1 = random.Next(crossoverPool.Count);
      int index2;
      do {
        index2 = random.Next(crossoverPool.Count);
      } while (index1 == index2);

      if (random.Next(100) < 80) { // 80% chance to crossover
        int midR = random.Next(MAZE_R) + 1;
        int midC = random.Next(MAZE_C) + 1;
        BasicMaze newMaze1 = new BasicMaze(crossoverPool[index1]);
        BasicMaze newMaze2 = new BasicMaze(crossoverPool[index2]);
        for (int r = 1; r <= MAZE_R; r++) {
          for (int c = 1; c <= MAZE_C; c++) {
            if (r < midR || (r == midR && c < midC)) {
              newMaze2.setBlock(r, c, crossoverPool[index1].isEmptyBlock(r, c));
            } else {
              newMaze1.setBlock(r, c, crossoverPool[index2].isEmptyBlock(r, c));
            }
          }
        }
        newMaze1.setFitness();
        newMaze2.setFitness();
        population.Add(new BasicMaze(newMaze1));
        population.Add(new BasicMaze(newMaze2));
      } else {
        population.Add(new BasicMaze(crossoverPool[index1]));
        population.Add(new BasicMaze(crossoverPool[index2]));
      }
    }
  }

  private void mutation () {
    for (int i = 0; i < population.Count; i++) {
      for (int r = 1; r <= MAZE_R; r++) {
        for (int c = 1; c <= MAZE_C; c++) {
          if (random.Next(1000) < 1) { // 0.1% chance to crossover
            population[i].setBlock(r, c, !population[i].isEmptyBlock(r, c));
          }
        }
      }
      population[i].setFitness();
      if (i == 0 || (population[i].getFitness() > best.getFitness())) {
        best = new BasicMaze(population[i]);
      }
      if (best.getFitness() > allBest.getFitness()) {
        allBest = new BasicMaze(best);
      }
    }
  }

}
