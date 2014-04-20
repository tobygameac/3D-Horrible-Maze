using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class MazeGenerator : MonoBehaviour {

  public bool isDebugging;

  public int MAZE_R;
  public int MAZE_C;
  public int MAZE_H;

  public int BLOCK_SIZE;

  private List<BasicMaze> basicMazes = new List<BasicMaze>();

  public GameObject playerPrefab;

  public GameObject bossPrefab;

  System.Random random = new System.Random();

  void Start () {

    for (int h = 0; h < MAZE_H; h++) {

      if (MazeData.getRandomMazeData(MAZE_R, MAZE_C) != null) {
        // Offline
        basicMazes.Add(new BasicMaze(MAZE_R, MAZE_C));
      } else {
        // Online
        generateBasicMaze();
      }

      if (isDebugging) {
        basicMazes[h].log();
      }
    }

    instantiateMaze();

    allocateItem();

    initialRandomEvent();

    // Instantiate player
    int playerFloor = 0;
    Point startPoint = basicMazes[playerFloor].getStartPoint();
    Point offset = getOffset(playerFloor);
    startPoint.r += offset.r;
    startPoint.c += offset.c;
    GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
    Vector3 playerPosition = new Vector3(startPoint.c * BLOCK_SIZE,  getBaseY(playerFloor) + player.transform.localScale.y + 0.11f, startPoint.r * BLOCK_SIZE);
    player.transform.position = playerPosition;

    // Instantiate boss
    int bossFloor = MAZE_H - 1;
    startPoint = basicMazes[bossFloor].getEndPoint();
    offset = getOffset(bossFloor);
    startPoint.r += offset.r;
    startPoint.c += offset.c;
    GameObject boss = Instantiate(bossPrefab, Vector3.zero, Quaternion.identity) as GameObject;
    Vector3 bossPosition = new Vector3(startPoint.c * BLOCK_SIZE, getBaseY(bossFloor) + boss.transform.localScale.y + 0.11f, startPoint.r * BLOCK_SIZE);
    boss.transform.position = bossPosition;

    GameState.state = GameState.PLAYING;
    Time.timeScale = 1;
  }

  // Genetic algorithm parameter
  public int NUM_OF_POPULATION = 30;
  public int TIMES_OF_ITERATION = 100;

  private BasicMaze best; // Best maze for each generation
  private BasicMaze allBest; // Best maze for all generation

  private List<BasicMaze> population = new List<BasicMaze>();
  private List<BasicMaze> crossoverPool = new List<BasicMaze>();

  private void generateBasicMaze () {
    initial();
    if (isDebugging) {
      Debug.Log("Original " + " : " + best.getFitness());
    }
    for (int times = 1; times <= TIMES_OF_ITERATION; times++) {
      reproduction();
      crossover();
      mutation();
      if (isDebugging) {
        Debug.Log("The fitness of the best maze of generation " + times + " : " + best.getFitness());
      }
    }
    basicMazes.Add(new BasicMaze(allBest));
  }

  private void initial() {
    population.Clear();

    for (int num = 0; num < NUM_OF_POPULATION; num++) {
      population.Add(new BasicMaze(MAZE_R, MAZE_C));

      if (num == 0 || (population[population.Count - 1].getFitness() > best.getFitness())) {
        best = new BasicMaze(population[population.Count - 1]);
        allBest = new BasicMaze(best);
      }
    }
  }

  private void reproduction () {
    int fitnessSum = 0;
    for (int i = 0; i < population.Count; i++) {
      fitnessSum += population[i].getFitness();
    }

    crossoverPool.Clear();

    int remain = population.Count;
    if (fitnessSum != 0) {
      for (int i = 0; i < population.Count && remain > 0; i++) {
        int need = (int)(population[i].getFitness() / (double)fitnessSum + 0.5);
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
              newMaze2.setBlock(r, c, crossoverPool[index1].getBlockState(r, c));
            } else {
              newMaze1.setBlock(r, c, crossoverPool[index2].getBlockState(r, c));
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
            population[i].setBlock(r, c, !population[i].getBlockState(r, c));
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
