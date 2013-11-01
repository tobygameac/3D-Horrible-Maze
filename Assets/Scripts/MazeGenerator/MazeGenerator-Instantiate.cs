using UnityEngine;
using System.Collections;

public partial class MazeGenerator : MonoBehaviour {

  public GameObject ceilingPrefab;
  public GameObject blockPrefab;
  public GameObject wallPrefab;
  public GameObject bloodPrefab;
  public GameObject elevatorPrefab;

  private void instantiateMaze () {

    // Parent objects
    GameObject ceilings = new GameObject();
    GameObject blocks = new GameObject();
    GameObject walls = new GameObject();
    GameObject bloods = new GameObject();
    GameObject elevators = new GameObject();

    ceilings.transform.parent = transform;
    ceilings.name = "ceilings";
    blocks.transform.parent = transform;
    blocks.name = "blocks";
    walls.transform.parent = transform;
    walls.name = "walls";
    bloods.transform.parent = transform;
    bloods.name = "bloods";
    elevators.transform.parent = transform;
    elevators.name = "elevators";

    // The offset between different floors
    int offsetR = 0;
    int offsetC = 0;

    for (int h = 0; h < MAZE_H; h++) {
      Point startPoint = maze[h].getStartPoint();
      Point endPoint = maze[h].getEndPoint();

      if (h == 0) { // Move the player to the start point
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerPosition = new Vector3(startPoint.c * BLOCK_SIZE, player.transform.localScale.y + 0.11f, startPoint.r * BLOCK_SIZE);
        player.transform.position = playerPosition;
      } else { // Move the start point to the last end point, and shift the whole maze
        Point lastEndPoint = maze[h - 1].getEndPoint();
        offsetR += lastEndPoint.r - startPoint.r;
        offsetC += lastEndPoint.c - startPoint.c;
      }

      for (int r = 0; r < MAZE_R + 2; r++) {
        for (int c = 0; c < MAZE_C + 2; c++) {

          // Calculate the real position in the world
          int realR = (r + offsetR) * BLOCK_SIZE;
          int realC = (c + offsetC) * BLOCK_SIZE;

          if (maze[h].getBlock(r, c)) {

            if (r == endPoint.r && c == endPoint.c && h < MAZE_H - 1) {
              // Elevator (Elevator do not need a ceiling)
              Vector3 elevatorPosition = new Vector3(realC, h * BLOCK_HEIGHT + 0.1f, realR);
              GameObject elevator = Instantiate(elevatorPrefab, elevatorPosition, Quaternion.identity) as GameObject;
              elevator.GetComponent<Elevator>().setMovingDistance(BLOCK_HEIGHT);
              elevator.transform.localScale = new Vector3(BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE);
              elevator.transform.parent = elevators.transform;
            } else {
              // Ceiling
              Vector3 ceilingPosition = new Vector3(realC, h * BLOCK_HEIGHT + BLOCK_HEIGHT + 0.1f - 0.01f, realR);
              GameObject ceiling = Instantiate(ceilingPrefab, ceilingPosition, Quaternion.identity) as GameObject;
              ceiling.transform.parent = ceilings.transform;
              ceiling.transform.localScale = new Vector3(BLOCK_SIZE, 0.01f, BLOCK_SIZE);

              // Blood
              if (random.Next(100) < 5) { // 5% to generate blood
                Vector3 bloodPosition = new Vector3(realC, h * BLOCK_HEIGHT + BLOCK_HEIGHT + 0.1f - 0.02f, realR);
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
            Vector3 blockPosition = new Vector3(realC, h * BLOCK_HEIGHT + 0.1f, realR);
            GameObject block = Instantiate(blockPrefab, blockPosition, Quaternion.identity) as GameObject;
            block.transform.localScale = new Vector3(BLOCK_SIZE, 0.01f, BLOCK_SIZE);
            block.transform.parent = blocks.transform;

            // Blood
            if (random.Next(100) < 5) { // 5% to generate blood
              Vector3 bloodPosition = new Vector3(realC, h * BLOCK_HEIGHT + 0.11f, realR);
              GameObject blood = Instantiate(bloodPrefab, bloodPosition, Quaternion.identity) as GameObject;
              blood.transform.localScale = new Vector3(BLOCK_SIZE, 0.01f, BLOCK_SIZE);
              blood.transform.eulerAngles = new Vector3(0, random.Next(360), 0);
              blood.transform.parent = bloods.transform;
            }

          } else {
            // Wall
            Vector3 wallPosition = new Vector3(realC, h * BLOCK_HEIGHT + BLOCK_HEIGHT / 2.0f + 0.1f, realR);
            GameObject wall = Instantiate(wallPrefab, wallPosition, Quaternion.identity) as GameObject;
            wall.transform.parent = walls.transform;
            wall.transform.localScale = new Vector3(BLOCK_SIZE, BLOCK_HEIGHT, BLOCK_SIZE);
          }

        }
      }
    }
  }

}
