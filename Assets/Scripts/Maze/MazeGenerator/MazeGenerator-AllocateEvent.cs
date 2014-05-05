using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class MazeGenerator : MonoBehaviour {

  public int[] randomEventCount;
  public GameObject[] randomEventPrefabs;

  private GameObject randomEvents;

  private List<List<List<bool>>> eventOnBlock;

  private void initialRandomEvent () {

    eventOnBlock = new List<List<List<bool>>>();

    for (int h = 0; h < MAZE_H; h++) {
      eventOnBlock.Add(new List<List<bool>>());
      for (int r = 0; r < MAZE_R + 2; r++) {
        eventOnBlock[h].Add(new List<bool>());
        for (int c = 0; c < MAZE_C + 2; c++) {
          eventOnBlock[h][r].Add(false);
        }
      }
    }

    randomEvents = new GameObject();
    randomEvents.name = "randomEvents";
    for (int i = 0; i < randomEventCount.Length; i++) {
      allocateRandomEvent(i, randomEventCount[i]);
    }
  }

  public Vector3 getNewEventPosition () {
    int randomH = random.Next(MAZE_H);
    Point point = getRandomAvailableBlock(randomH, true);
    int r = point.r;
    int c = point.c;
    int tried = 0, maximumTried = 100;
    while (itemOnBlock[randomH][r][c] || eventOnBlock[randomH][r][c]) {
      if (tried > maximumTried) {
        break;
      }
      randomH = random.Next(MAZE_H);
      point = getRandomAvailableBlock(randomH, true);
      r = point.r;
      c = point.c;
      tried++;
    }
    eventOnBlock[randomH][r][c] = true;
    Point offset = getOffset(randomH);
    int realR = (r + offset.r) * BLOCK_SIZE;
    int realC = (c + offset.c) * BLOCK_SIZE;
    float baseY = getBaseY(randomH);
    return new Vector3(realC + (random.Next(10) - 10) / 100.0f, baseY + 1.25f + (random.Next(10) - 10) / 100.0f, realR + (random.Next(10) - 10) / 100.0f);
  }

  public Vector3 getNewEventPosition (Vector3 position) {
    Vector3 HRC = convertCoordinatesToHRC(position);
    int h = (int)HRC.x;
    int r = (int)HRC.y;
    int c = (int)HRC.z;
    eventOnBlock[h][r][c] = false;
    Vector3 newPosition = getNewEventPosition();
    int tried = 0, maximumTried = 100;
    while (h == getFloor(newPosition)) {
      if (tried > maximumTried) {
        break;
      }
      newPosition = getNewEventPosition();
      tried++;
    }
    return newPosition;
  }

  public void allocateRandomEvent (int number) {
    int randomEventIndex = random.Next(randomEventPrefabs.Length);
    for (int i = 0; i < number; i++) {
      Vector3 randomEventPosition = getNewEventPosition();
      GameObject randomEvent = Instantiate(randomEventPrefabs[randomEventIndex], randomEventPosition, randomEventPrefabs[randomEventIndex].transform.rotation) as GameObject;
      randomEvent.transform.parent = randomEvents.transform;
    }
  }

  public void allocateRandomEvent (int randomEventIndex, int number) {
    for (int i = 0; i < number; i++) {
      Vector3 randomEventPosition = getNewEventPosition();
      GameObject randomEvent = Instantiate(randomEventPrefabs[randomEventIndex], randomEventPosition, randomEventPrefabs[randomEventIndex].transform.rotation) as GameObject;
      randomEvent.transform.parent = randomEvents.transform;
    }
  }

}
