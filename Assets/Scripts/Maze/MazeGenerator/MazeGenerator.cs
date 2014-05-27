using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class MazeGenerator : MonoBehaviour {

  public bool isDebugging;

  public Texture blackMask;

  public int MAZE_R;
  public int MAZE_C;
  public int MAZE_H;

  public int BLOCK_SIZE;

  private List<BasicMaze> basicMazes = new List<BasicMaze>();

  public GameObject playerPrefab;

  public GameObject bossPrefab;

  private static System.Random random = new System.Random(); // Only need one random seed

  // Genetic algorithm parameter
  public int NUM_OF_POPULATION = 30;
  public int MAXIMUM_TIMES_OF_ITERATION = 500;
  public double TARGET_FITNESS = 0.3;

  private BasicMaze best; // Best maze for each generation
  private BasicMaze allBest; // Best maze for all generation

  private List<BasicMaze> population;
  private List<BasicMaze> crossoverPool;

  void Start () {
    Debug.Log(GameState.difficulty);

    GameState.state = GameState.PLAYING;
    Time.timeScale = 1;

    for (int h = 0; h < MAZE_H; h++) {

      if (MazeData.getRandomMazeData(MAZE_R, MAZE_C, GameState.difficulty) != null) {
        // Offline
        basicMazes.Add(new BasicMaze(MAZE_R, MAZE_C));
      } else {
        // Online
        // Online assume difficuty = 0
        generateBasicMaze();
      }

      if (isDebugging) {
        basicMazes[h].log();
      }
    }

    instantiateMaze();

    allocateItem();

    initialRandomEvent();

    generatePlayer();

    generateBoss();

    switch (GameMode.mode) {
     case GameMode.ESCAPING:
      EscapingState.state = EscapingState.BEGINNING;
      TargetMenu.addTarget("Find the exit.");
      break;
     case GameMode.INFINITE:
      TargetMenu.addTarget("Try to survive!");
      break;
    }
  }

  void OnGUI () {
    if (GameState.state != GameState.LOADING) {
      return;
    }
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackMask);
  }

  private void generatePlayer () {
    int playerFloor = 0;
    Point playerStartPoint = basicMazes[playerFloor].getStartPoint();
    if (GameMode.mode == GameMode.ESCAPING) {
      playerFloor = 0;
      playerStartPoint = getRandomAvailableBlock(playerFloor, true);
      if (basicMazes[playerFloor].isStartPoint(playerStartPoint) || basicMazes[playerFloor].isEndPoint(playerStartPoint)) {
        // Reload if cannot find a good start point
        Application.LoadLevel(Application.loadedLevel);
      }
      if (GameState.escapingDemo) {
        playerStartPoint = new Point(10, 8);
      }
    }
    Point offset = getOffset(playerFloor);
    playerStartPoint.r += offset.r;
    playerStartPoint.c += offset.c;
    GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
    Vector3 playerPosition = new Vector3(playerStartPoint.c * BLOCK_SIZE, getBaseY(playerFloor) + player.transform.localScale.y + 0.11f, playerStartPoint.r * BLOCK_SIZE);
    player.transform.position = playerPosition;
  }

  private void generateBoss () {
    int bossFloor = MAZE_H - 1;
    Point bossStartPoint = getRandomAvailableBlock(bossFloor, true);
    Point offset = getOffset(bossFloor);
    bossStartPoint.r += offset.r;
    bossStartPoint.c += offset.c;
    GameObject boss = Instantiate(bossPrefab, Vector3.zero, Quaternion.identity) as GameObject;
    Vector3 bossPosition = new Vector3(bossStartPoint.c * BLOCK_SIZE, getBaseY(bossFloor) + boss.transform.localScale.y + 0.11f, bossStartPoint.r * BLOCK_SIZE);
    boss.transform.position = bossPosition;
  }

  private void generateBasicMaze () {
    initial();
    if (isDebugging) {
      Debug.Log("Original " + " : " + best.getFitness());
    }
    for (int times = 1; times <= MAXIMUM_TIMES_OF_ITERATION; times++) {
      reproduction();
      crossover();
      mutation();
      if (isDebugging) {
        Debug.Log("The fitness of the best maze of generation " + times + " : " + best.getFitness());
      }
      if (best.getFitness() >= TARGET_FITNESS) {
        break;
      }
    }
    basicMazes.Add(new BasicMaze(best));
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
