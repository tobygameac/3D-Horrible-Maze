using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class MazeGenerator : MonoBehaviour {

  public GameObject[] itemPrefabs;

  private List<List<List<bool>>> itemOnBlock;

  private void allocateItem () {
    
    itemOnBlock = new List<List<List<bool>>>();

    for (int h = 0; h < MAZE_H; h++) {
      itemOnBlock.Add(new List<List<bool>>());
      for (int r = 0; r < MAZE_R + 2; r++) {
        itemOnBlock[h].Add(new List<bool>());
        for (int c = 0; c < MAZE_C + 2; c++) {
          itemOnBlock[h][r].Add(false);
        }
      }
    }

    // List for index of item
    List<int> remainItemIndex = new List<int>();
    
    for (int i = 0; i < itemPrefabs.Length; i++) {
      remainItemIndex.Add(i);
    }

    // Parent objects
    List<GameObject> itemParents = new List<GameObject>();
    for (int h = 0; h < MAZE_H; h++) {
      GameObject floor = GameObject.Find("floor" + (h + 1));
      GameObject items = new GameObject();
      items.transform.parent = floor.transform;
      items.name = "items";
      itemParents.Add(items);
    }


    for (int h = 0; h < MAZE_H && remainItemIndex.Count > 0; h++) {

      // Get a random item
      int randomItemIndex = random.Next(remainItemIndex.Count);
      int itemIndex = remainItemIndex[randomItemIndex];

      // Remove item from container
      remainItemIndex.RemoveAt(randomItemIndex);

      // Get a random point
      Point point = getRandomAvailableBlock(h, true);
      int r = point.r, c = point.c;
      itemOnBlock[h][r][c] = true;

      // The offset between different floors
      Point offset = getOffset(h);

      // Calculate the real position in the world
      int realR = (r + offset.r) * BLOCK_SIZE;
      int realC = (c + offset.c) * BLOCK_SIZE;

      // Instantiate item
      Vector3 itemPosition = new Vector3(realC, getBaseY(h) + itemPrefabs[itemIndex].transform.localScale.y, realR);
      GameObject item = Instantiate(itemPrefabs[itemIndex], itemPosition, Quaternion.identity) as GameObject;
      item.transform.parent = itemParents[h].transform;
    }

    // Allocate remain items
    while (remainItemIndex.Count > 0) {

      // Get a random item
      int randomItemIndex = random.Next(remainItemIndex.Count);
      int itemIndex = remainItemIndex[randomItemIndex];

      // Remove item from container
      remainItemIndex.RemoveAt(randomItemIndex);

      // Get a random floor
      int randomH = random.Next(MAZE_H);

      // Get a random point
      Point point = getRandomAvailableBlock(randomH, true);
      int r = point.r, c = point.c;

      int tried = 0, maximumTried = 100;
      while (itemOnBlock[randomH][r][c]) {
        if (tried > maximumTried) {
          break;
        }
        randomH = random.Next(MAZE_H);
        point = getRandomAvailableBlock(randomH, true);
        r = point.r;
        c = point.c;
        tried++;
      }

      itemOnBlock[randomH][r][c] = true;

      // The offset between different floors
      Point offset = getOffset(randomH);

      // Calculate the real position in the world
      int realR = (r + offset.r) * BLOCK_SIZE;
      int realC = (c + offset.c) * BLOCK_SIZE;

      // Instantiate item
      Vector3 itemPosition = new Vector3(realC, getBaseY(randomH) + itemPrefabs[itemIndex].transform.localScale.y, realR);
      GameObject item = Instantiate(itemPrefabs[itemIndex], itemPosition, Quaternion.identity) as GameObject;
      item.transform.parent =  itemParents[randomH].transform;;
    }
  }
}
