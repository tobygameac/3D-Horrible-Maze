using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class MazeGenerator : MonoBehaviour {

  public int WALL_HEIGHT;
  public float CEILING_THICKNESS;

  public GameObject[] ceilingPrefabs;
  public GameObject[] blockPrefabs;
  public GameObject[] wallPrefabs;
  public GameObject bloodPrefab;
  public GameObject elevatorPrefab;
  public GameObject drawingPrefab;
  public GameObject torchPrefab;
  public GameObject zombieHeadPrefab;

  // Escaping mode only
  public GameObject keyPrefab;
  public GameObject exitPrefab;

  private void instantiateMaze () {

    for (int h = 0; h < MAZE_H; h++) {
      int type = 0;
      if ((ceilingPrefabs.Length == blockPrefabs.Length) && (blockPrefabs.Length == wallPrefabs.Length)) {
        type = random.Next(ceilingPrefabs.Length);
      }
      // Parent objects
      GameObject floor = new GameObject();
      floor.name = "floor" + (h + 1);

      GameObject ceilings = new GameObject();
      GameObject blocks = new GameObject();
      GameObject walls = new GameObject();
      GameObject bloods = new GameObject();
      GameObject elevators = new GameObject();
      GameObject drawings = new GameObject();
      GameObject torches = new GameObject();
      GameObject zombieHeads = new GameObject();

      ceilings.transform.parent = floor.transform;
      ceilings.name = "ceilings";
      blocks.transform.parent = floor.transform;
      blocks.name = "blocks";
      walls.transform.parent = floor.transform;
      walls.name = "walls";
      bloods.transform.parent = floor.transform;
      bloods.name = "bloods";
      elevators.transform.parent = floor.transform;
      elevators.name = "elevators";
      drawings.transform.parent = floor.transform;
      drawings.name = "drawings";
      torches.transform.parent = floor.transform;
      torches.name = "torches";
      zombieHeads.transform.parent = floor.transform;
      zombieHeads.name = "zombie heads";

      Point startPoint = basicMazes[h].getStartPoint();
      Point endPoint = basicMazes[h].getEndPoint();

      // The offset between different floors
      // In order to move the start point to the last end point, and shift the whole maze
      Point offset = getOffset(h);

      float baseY = getBaseY(h);

      List<List<bool>> isDecorated = new List<List<bool>>();
      for (int r = 0; r < MAZE_R + 2; r++) {
        isDecorated.Add(new List<bool>());
        for (int c = 0; c < MAZE_C + 2; c++) {
          isDecorated[r].Add(false);
        }
      }

      if (GameMode.mode == GameMode.ESCAPING) {
        if (h == 0) {
          int r = startPoint.r;
          int c = startPoint.c;
          int realR = (r + offset.r) * BLOCK_SIZE;
          int realC = (c + offset.c) * BLOCK_SIZE;
          Vector3 exitPosition = new Vector3(realC, baseY, realR);
          GameObject exit = Instantiate(exitPrefab, exitPosition, Quaternion.identity) as GameObject;
          exit.transform.localScale = new Vector3(BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE);
          exit.transform.parent = floor.transform;
        }

        if (h == MAZE_H - 1) {
          int r = endPoint.r;
          int c = endPoint.c;
          int realR = (r + offset.r) * BLOCK_SIZE;
          int realC = (c + offset.c) * BLOCK_SIZE;
          Vector3 keyPosition = new Vector3(realC, baseY + 1.25f + (random.Next(10) - 10) / 100.0f, realR);
          GameObject key = Instantiate(keyPrefab, keyPosition, Quaternion.identity) as GameObject;
          key.transform.parent = floor.transform;
        }
      }

      for (int r = 0; r < MAZE_R + 2; r++) {
        for (int c = 0; c < MAZE_C + 2; c++) {

          // Calculate the real position in the world
          int realR = (r + offset.r) * BLOCK_SIZE;
          int realC = (c + offset.c) * BLOCK_SIZE;

          if (basicMazes[h].isEmptyBlock(r, c)) {

            if (r == endPoint.r && c == endPoint.c && h < MAZE_H - 1) {
              // Elevator (Elevator do not need a ceiling)
              Vector3 elevatorPosition = new Vector3(realC, baseY, realR);
              GameObject elevator = Instantiate(elevatorPrefab, elevatorPosition, Quaternion.identity) as GameObject;

              elevator.GetComponent<Elevator>().setMovingDistance(WALL_HEIGHT * BLOCK_SIZE + CEILING_THICKNESS);

              elevator.transform.localScale = new Vector3(BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE);
              elevator.transform.parent = elevators.transform;
            } else {
              // Ceiling
              Vector3 ceilingPosition = new Vector3(realC, baseY + WALL_HEIGHT * BLOCK_SIZE + CEILING_THICKNESS * 0.5f, realR);
              GameObject ceiling = Instantiate(ceilingPrefabs[type], ceilingPosition, Quaternion.identity) as GameObject;
              ceiling.transform.parent = ceilings.transform;
              ceiling.transform.localScale = new Vector3(BLOCK_SIZE, CEILING_THICKNESS, BLOCK_SIZE);

              // Blood
              if (random.Next(100) < 5) { // 5% to generate blood
                Vector3 bloodPosition = new Vector3(realC, ceilingPosition.y - 0.01f, realR);
                GameObject blood = Instantiate(bloodPrefab, bloodPosition, Quaternion.identity) as GameObject;
                blood.transform.localScale = new Vector3(BLOCK_SIZE, 0.01f, BLOCK_SIZE);
                blood.transform.eulerAngles = new Vector3(0, random.Next(360), 0);
                blood.transform.parent = bloods.transform;
              }
            }

            // Start point just need a ceiling
            if (r == startPoint.r && c == startPoint.c && h > 0) {
              continue;
            }

            // Block
            Vector3 blockPosition = new Vector3(realC, baseY, realR);
            GameObject block = Instantiate(blockPrefabs[type], blockPosition, Quaternion.identity) as GameObject;
            block.transform.localScale = new Vector3(BLOCK_SIZE, 0.01f, BLOCK_SIZE);
            block.transform.parent = blocks.transform;

            // Blood
            if (random.Next(100) < 5) { // 5% to generate blood
              Vector3 bloodPosition = new Vector3(realC, baseY + 0.001f, realR);
              GameObject blood = Instantiate(bloodPrefab, bloodPosition, Quaternion.identity) as GameObject;
              blood.transform.localScale = new Vector3(BLOCK_SIZE, 0.001f, BLOCK_SIZE);
              blood.transform.eulerAngles = new Vector3(0, random.Next(360), 0);
              blood.transform.parent = bloods.transform;
            }

          } else {
            // Ceiling
            Vector3 ceilingPosition = new Vector3(realC, baseY + WALL_HEIGHT * BLOCK_SIZE + CEILING_THICKNESS * 0.5f, realR);
            GameObject ceiling = Instantiate(ceilingPrefabs[type], ceilingPosition, Quaternion.identity) as GameObject;
            ceiling.transform.localScale = new Vector3(BLOCK_SIZE, CEILING_THICKNESS, BLOCK_SIZE);
            ceiling.transform.parent = ceilings.transform;

            int[] dr = new int[4]{1, 0, -1, 0};
            int[] dc = new int[4]{0, 1, 0, -1};
            int[] yRotation = new int[4]{180, 270, 0, 90};

            // Wall
            for (int wallCount = 0; wallCount < WALL_HEIGHT; wallCount++) {
              Vector3 wallPosition = new Vector3(realC, baseY + wallCount * BLOCK_SIZE + BLOCK_SIZE * 0.5f, realR);
              GameObject wall = Instantiate(wallPrefabs[type], wallPosition, Quaternion.identity) as GameObject;
              wall.transform.localScale = new Vector3(BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE);
              wall.transform.parent = walls.transform;
              for (int d = 0; d < 4; d++) {
                if (basicMazes[h].isAvailableBlock(r + dr[d], c + dc[d])) {
                  if (random.Next(100) < 10) { // 10% to generate blood
                    float bloodC = realC + (BLOCK_SIZE / 2 + 0.01f) * dc[d];
                    float bloodR = realR + (BLOCK_SIZE / 2 + 0.01f) * dr[d];
                    Vector3 bloodPosition = new Vector3(bloodC, baseY + wallCount * BLOCK_SIZE + BLOCK_SIZE * 0.5f, bloodR);
                    GameObject blood = Instantiate(bloodPrefab, bloodPosition, Quaternion.identity) as GameObject;
                    float scale = (random.Next(30) + 70) / 100.0f;
                    blood.transform.localScale = new Vector3(BLOCK_SIZE * scale, 0.001f, BLOCK_SIZE * scale);
                    blood.transform.eulerAngles = new Vector3(90, yRotation[d], 0);
                    blood.transform.parent = bloods.transform;
                  }
                }
              }
              if (wallCount >= WALL_HEIGHT - 2) {
                for (int d = 0; d < 4; d++) {
                  if (basicMazes[h].isAvailableBlock(r + dr[d], c + dc[d])) {
                    if (isDecorated[r + dr[d]][c + dc[d]]) {
                      continue;
                    }
                    int bonusProbability = 0;
                    if (basicMazes[h].isDeadendBlock(r + dr[d], c + dc[d])) {
                      bonusProbability += 4;
                    }
                    if (random.Next(100) < 1 + bonusProbability) { // 1% to generate drawing
                      float drawingC = realC + (BLOCK_SIZE / 2 + 0.01f) * dc[d];
                      float drawingR = realR + (BLOCK_SIZE / 2 + 0.01f) * dr[d];
                      Vector3 drawingPosition = new Vector3(drawingC, baseY + wallCount * BLOCK_SIZE + BLOCK_SIZE * 0.5f, drawingR);
                      GameObject drawing = Instantiate(drawingPrefab, drawingPosition, Quaternion.identity) as GameObject;
                      float scale = (random.Next(30) + 70) / 100.0f;
                      drawing.transform.localScale = new Vector3(BLOCK_SIZE * scale, BLOCK_SIZE * scale, 0.01f);
                      drawing.transform.eulerAngles = new Vector3(0, yRotation[d], 180);
                      drawing.transform.parent = drawings.transform;
                      isDecorated[r + dr[d]][c + dc[d]] = true;
                    } else if (wallCount == 1) {
                      if (random.Next(100) < 10) { // 10% to generate torch
                        float torchC = realC + (BLOCK_SIZE / 2) * dc[d];
                        float torchR = realR + (BLOCK_SIZE / 2) * dr[d];
                        Vector3 torchPosition = new Vector3(torchC, baseY + wallCount * BLOCK_SIZE + BLOCK_SIZE * 0.5f, torchR);
                        GameObject torch = Instantiate(torchPrefab, torchPosition, Quaternion.identity) as GameObject;
                        torch.transform.localScale = new Vector3(BLOCK_SIZE / 2, BLOCK_SIZE / 2, 0.01f);
                        torch.transform.eulerAngles = new Vector3(-30, yRotation[d], 0);
                        torch.transform.parent = torches.transform;
                        isDecorated[r + dr[d]][c + dc[d]] = true;
                      } else if (random.Next(100) < 1) { // 1% to generate zombie head
                        if (h != 0) {
                          float zombieHeadC = realC + (BLOCK_SIZE / 2) * dc[d];
                          float zombieHeadR = realR + (BLOCK_SIZE / 2) * dr[d];
                          Vector3 zombieHeadPosition = new Vector3(zombieHeadC, baseY + wallCount * BLOCK_SIZE + BLOCK_SIZE * 0.5f, zombieHeadR);
                          GameObject zombieHead = Instantiate(zombieHeadPrefab, zombieHeadPosition, Quaternion.identity) as GameObject;
                          zombieHead.transform.localScale = new Vector3(BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE);
                          zombieHead.transform.eulerAngles = new Vector3(0, random.Next(360), 0);
                          zombieHead.transform.parent = zombieHeads.transform;
                          isDecorated[r + dr[d]][c + dc[d]] = true;
                        }
                      }
                    }
                  }
                }
              }
            }

          }
        }
      }
    }
  }

}
