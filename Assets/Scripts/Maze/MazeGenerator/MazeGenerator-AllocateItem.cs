using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class MazeGenerator : MonoBehaviour {

  public GameObject[] itemPrefabs;

  private void allocateItem () {
    
    List<List<Point>> possiblePoints = new List<List<Point>>();

    for (int h = 0; h < basicMazes.Count; h++) {
      
      List<List<bool>> visited = new List<List<bool>>();
      
      for (int r = 0; r < MAZE_R + 2; r++) {
        visited.Add(new List<bool>());
        for (int c = 0; c < MAZE_C + 2; c++) {
          visited[r].Add(false);
        }
      }

      Point startPoint = basicMazes[h].getStartPoint();
      Point endPoint = basicMazes[h].getEndPoint();

      visited[startPoint.r][startPoint.c] = true;

      // Find all possible point

      List<Point> points = new List<Point>();
      Queue<Point> q = new Queue<Point>();
      q.Enqueue(new Point(startPoint.r, startPoint.c));

      // Move dirction
      int[] dr = new int[4]{1, 0, -1, 0};
      int[] dc = new int[4]{0, 1, 0, -1};

      // BFS
      while (q.Count > 0) {
        Point now = q.Dequeue();

        // No item at start point and end point
        if (!now.equal(startPoint) && !now.equal(endPoint)) {
          points.Add(new Point(now));
        }

        int r = now.r;
        int c = now.c;
        for (int d = 0; d < 4; d++) {
          int nr = r + dr[d], nc = c + dc[d];
          if (basicMazes[h].getBlockState(nr, nc) && !visited[nr][nc]) {
            q.Enqueue(new Point(nr, nc));
            visited[nr][nc] = true;
          }
        }
      }

      possiblePoints.Add(points);
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


    for (int h = 0; h < basicMazes.Count && remainItemIndex.Count > 0; h++) {


      // Get a random item
      int randomItemIndex = random.Next(remainItemIndex.Count);
      int itemIndex = remainItemIndex[randomItemIndex];

      // Remove item from container
      remainItemIndex.RemoveAt(randomItemIndex);

      // Get a random point
      int randomPointIndex = random.Next(possiblePoints[h].Count);
      Point point = possiblePoints[h][randomPointIndex];
      int r = point.r, c = point.c;

      // Remove item from container
      possiblePoints[h].RemoveAt(randomPointIndex);

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
      int randomPointIndex = random.Next(possiblePoints[randomH].Count);
      Point point = possiblePoints[randomH][randomPointIndex];
      int r = point.r, c = point.c;

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
