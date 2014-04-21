using UnityEngine;
using System.Collections;

public partial class MazeGenerator : MonoBehaviour {

  public int WALL_HEIGHT;
  public float CEILING_THICKNESS;

  public GameObject[] ceilingPrefabs;
  public GameObject[] blockPrefabs;
  public GameObject[] wallPrefabs;
  public GameObject bloodPrefab;
  public GameObject elevatorPrefab;

  private void instantiateMaze () {
    int type = 0;
    if ((ceilingPrefabs.Length == blockPrefabs.Length) && (blockPrefabs.Length == wallPrefabs.Length)) {
      type = random.Next(ceilingPrefabs.Length);
    }

    for (int h = 0; h < MAZE_H; h++) {
      // Parent objects
      GameObject floor = new GameObject();
      floor.name = "floor" + (h + 1);

      GameObject ceilings = new GameObject();
      GameObject blocks = new GameObject();
      GameObject walls = new GameObject();
      GameObject bloods = new GameObject();
      GameObject elevators = new GameObject();

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

      Point startPoint = basicMazes[h].getStartPoint();
      Point endPoint = basicMazes[h].getEndPoint();

      // The offset between different floors
      // In order to move the start point to the last end point, and shift the whole maze
      Point offset = getOffset(h);

      float baseY = getBaseY(h);

      for (int r = 0; r < MAZE_R + 2; r++) {
        for (int c = 0; c < MAZE_C + 2; c++) {

          // Calculate the real position in the world
          int realR = (r + offset.r) * BLOCK_SIZE;
          int realC = (c + offset.c) * BLOCK_SIZE;

          if (basicMazes[h].getBlockState(r, c)) {

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
              Vector3 bloodPosition = new Vector3(realC, baseY + 0.01f, realR);
              GameObject blood = Instantiate(bloodPrefab, bloodPosition, Quaternion.identity) as GameObject;
              blood.transform.localScale = new Vector3(BLOCK_SIZE, 0.01f, BLOCK_SIZE);
              blood.transform.eulerAngles = new Vector3(0, random.Next(360), 0);
              blood.transform.parent = bloods.transform;
            }

          } else {
            // Ceiling
            Vector3 ceilingPosition = new Vector3(realC, baseY + WALL_HEIGHT * BLOCK_SIZE + CEILING_THICKNESS * 0.5f, realR);
            GameObject ceiling = Instantiate(ceilingPrefabs[type], ceilingPosition, Quaternion.identity) as GameObject;
            ceiling.transform.parent = ceilings.transform;
            ceiling.transform.localScale = new Vector3(BLOCK_SIZE, CEILING_THICKNESS, BLOCK_SIZE);

            // Wall
            for (int wallCount = 0; wallCount < WALL_HEIGHT; wallCount++) {
              Vector3 wallPosition = new Vector3(realC, baseY + wallCount * BLOCK_SIZE + BLOCK_SIZE * 0.5f, realR);
              GameObject wall = Instantiate(wallPrefabs[type], wallPosition, Quaternion.identity) as GameObject;
              wall.transform.parent = walls.transform;
              wall.transform.localScale = new Vector3(BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE);
            }

          }
        }
      }
    }
  }

}
