using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class MazeGenerator : MonoBehaviour {

  public float getBaseY (int h) {

    // Error
    if (h < 0 || h >= MAZE_H) {

      if (isDebugging) {
        Debug.Log("Range of parameter h in function getBaseY is wrong.");
      }

      return 0;
    }

    // Wall hegiht + ceiling thickness
    return h * (WALL_HEIGHT * BLOCK_SIZE + CEILING_THICKNESS);
  }

  public int getFloor (float y) {
    // Get the floor where player stand
    int floor = 0;
    for (int h = 0; h < MAZE_H; h++) {
      if (y >= getBaseY(h)) {
        floor = h;
      } else {
        break;
      }
    }
    return floor;
  }

  public int getFloor (Vector3 position) {
    return getFloor(position.y);
  }

  public Point getOffset (int h) {
    
    // Error
    if (h < 0 || h >= MAZE_H) {
      
      if (isDebugging) {
        Debug.Log("Range of parameter h in function getOffset is wrong.");
      }

      return null;
    }

    // The offset between different floors
    int offsetR = 0;
    int offsetC = 0;

    for (int i = 1; i <= h; i++) {
      Point startPoint = basicMazes[i].getStartPoint();
      Point lastEndPoint = basicMazes[i - 1].getEndPoint();
      offsetR += lastEndPoint.r - startPoint.r;
      offsetC += lastEndPoint.c - startPoint.c;
    }

    return new Point(offsetR, offsetC);
  }

  public Point convertCoordinatesToRC (Vector3 position) {
    Point offset = getOffset(getFloor(position.y));
    int r = (int)(Mathf.Round(position.z) / BLOCK_SIZE) - offset.r;
    int c = (int)(Mathf.Round(position.x) / BLOCK_SIZE) - offset.c;
    return new Point(r, c);
  }

  public Vector3 convertCoordinatesToHRC (Vector3 position) {
    int h = getFloor(position.y);
    Point offset = getOffset(h);
    int r = (int)(Mathf.Round(position.z) / BLOCK_SIZE) - offset.r;
    int c = (int)(Mathf.Round(position.x) / BLOCK_SIZE) - offset.c;
    return new Vector3(h, r, c);
  }

  public Point getRandomAvailableBlock (int h, bool noneStartEnd = false, int maximumTried = 100) {

    // Error
    if (h < 0 || h >= MAZE_H) {
      
      if (isDebugging) {
        Debug.Log("Range of parameter h in function getOffset is wrong.");
      }

      return null;
    }

    if (noneStartEnd) {
      int tried = 0;
      Point point = basicMazes[h].getRandomAvailableBlock();
      while (basicMazes[h].isStartPoint(point) || basicMazes[h].isEndPoint(point)) {
        if (tried > maximumTried) {
          if (isDebugging) {
            Debug.Log("No none-start ans none-end point.");
          }
          break;
        }
        point = basicMazes[h].getRandomAvailableBlock();
        tried++;
      }
      return point;
    } else {
      return basicMazes[h].getRandomAvailableBlock();
    }
  }

  public bool isStartPoint (int h, int r, int c) {
    return basicMazes[h].isStartPoint(r, c);
  }

  public bool isEndPoint (int h, int r, int c) {
    return basicMazes[h].isEndPoint(r, c);
  }

  public bool isEmptyBlock (int h, int r, int c) {
    return basicMazes[h].isEmptyBlock(r, c);
  }

  public bool isAvailableBlock (int h, int r, int c) {
    return basicMazes[h].isAvailableBlock(r, c);
  }

  public List<Vector3> getShortestPath (Vector3 position1, Vector3 position2) {
    int floor1 = getFloor(position1.y);
    int floor2 = getFloor(position2.y);

    List<Vector3> path = new List<Vector3>();

    if (floor1 == floor2) {
      Point point1 = convertCoordinatesToRC(position1);
      Point point2 = convertCoordinatesToRC(position2);
      
      List<MovingAction> movingActions = basicMazes[floor1].getShortestPath(point1, point2);

      for (int i = 0; i < movingActions.Count; i++) {
        path.Add(new Vector3(movingActions[i].dc * BLOCK_SIZE, 0, movingActions[i].dr * BLOCK_SIZE));
      }

    } else if (floor1 < floor2) {
      Point point1 = convertCoordinatesToRC(position1);
      Point point2 = basicMazes[floor1].getEndPoint();

      List<MovingAction> movingActions = basicMazes[floor1].getShortestPath(point1, point2);

      for (int i = 0; i < movingActions.Count; i++) {
        path.Add(new Vector3(movingActions[i].dc * BLOCK_SIZE, 0, movingActions[i].dr * BLOCK_SIZE));
      }
      
      while (floor1 < floor2) {
        
        // Up
        path.Add(new Vector3(0, WALL_HEIGHT * BLOCK_SIZE + CEILING_THICKNESS, 0));
        
        floor1++;
        point1 = basicMazes[floor1].getStartPoint();
        if (floor1 == floor2) {
          point2 = convertCoordinatesToRC(position2);
        } else {
          point2 = basicMazes[floor1].getEndPoint();
        }

        movingActions = basicMazes[floor1].getShortestPath(point1, point2);
        
        for (int i = 0; i < movingActions.Count; i++) {
          path.Add(new Vector3(movingActions[i].dc * BLOCK_SIZE, 0, movingActions[i].dr * BLOCK_SIZE));
        }
      }

    } else if (floor1 > floor2) {
      Point point1 = convertCoordinatesToRC(position1);
      Point point2 = basicMazes[floor1].getStartPoint();

      List<MovingAction> movingActions = basicMazes[floor1].getShortestPath(point1, point2);

      for (int i = 0; i < movingActions.Count; i++) {
        path.Add(new Vector3(movingActions[i].dc * BLOCK_SIZE, 0, movingActions[i].dr * BLOCK_SIZE));
      }
      
      while (floor1 > floor2) {
        
        // Down
        path.Add(new Vector3(0, -1 * (WALL_HEIGHT * BLOCK_SIZE + CEILING_THICKNESS), 0));
        
        floor1--;
        point1 = basicMazes[floor1].getEndPoint();
        if (floor1 == floor2) {
          point2 = convertCoordinatesToRC(position2);
        } else {
          point2 = basicMazes[floor1].getStartPoint();
        }

        movingActions = basicMazes[floor1].getShortestPath(point1, point2);
        
        for (int i = 0; i < movingActions.Count; i++) {
          path.Add(new Vector3(movingActions[i].dc * BLOCK_SIZE, 0, movingActions[i].dr * BLOCK_SIZE));
        }
      }

    }

    return path;
  }

  public List<Vector3> getSimpleShortestPath (Vector3 position1, Vector3 position2) {
    int floor1 = getFloor(position1.y);
    int floor2 = getFloor(position2.y);

    List<Vector3> path = new List<Vector3>();

    if (floor1 == floor2) {
      Point point1 = convertCoordinatesToRC(position1);
      Point point2 = convertCoordinatesToRC(position2);
      
      List<MovingAction> movingActions = basicMazes[floor1].getShortestPath(point1, point2);

      for (int i = 0; i < movingActions.Count; i++) {
        path.Add(new Vector3(movingActions[i].dc * BLOCK_SIZE, 0, movingActions[i].dr * BLOCK_SIZE));
      }

    } else if (floor1 < floor2) {
      Point point1 = convertCoordinatesToRC(position1);
      Point point2 = basicMazes[floor1].getEndPoint();

      List<MovingAction> movingActions = basicMazes[floor1].getShortestPath(point1, point2);

      for (int i = 0; i < movingActions.Count; i++) {
        path.Add(new Vector3(movingActions[i].dc * BLOCK_SIZE, 0, movingActions[i].dr * BLOCK_SIZE));
      }

      // Up
      path.Add(new Vector3(0, WALL_HEIGHT * BLOCK_SIZE + CEILING_THICKNESS, 0));

    } else if (floor1 > floor2) {
      Point point1 = convertCoordinatesToRC(position1);
      Point point2 = basicMazes[floor1].getStartPoint();

      List<MovingAction> movingActions = basicMazes[floor1].getShortestPath(point1, point2);

      for (int i = 0; i < movingActions.Count; i++) {
        path.Add(new Vector3(movingActions[i].dc * BLOCK_SIZE, 0, movingActions[i].dr * BLOCK_SIZE));
      }

      // Down
      path.Add(new Vector3(0, -1 * (WALL_HEIGHT * BLOCK_SIZE + CEILING_THICKNESS), 0));
      
    }

    return path;
  }

}
