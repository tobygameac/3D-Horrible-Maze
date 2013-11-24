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

  public Point getOffset (int h) {
    
    // Error
    if (h < 0 || h >= MAZE_H) {
      
      if (isDebugging) {
        Debug.Log("Range of parameter h in function getOffset is wrong.");
      }

      return new Point(0, 0);
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

  public Point convertCoordinates (Vector3 position) {
    Point offset = getOffset(getFloor(position.y));
    int r = (int)(position.z / BLOCK_SIZE + 0.5) - offset.r;
    int c = (int)(position.x / BLOCK_SIZE + 0.5) - offset.c;
    return new Point(r, c);
  }

  public List<Vector3> getShortestPath (Vector3 position1, Vector3 position2) {
    int floor1 = getFloor(position1.y);
    int floor2 = getFloor(position2.y);

    List<Vector3> path = new List<Vector3>();

    if (floor1 != floor2) {

      if (floor1 < floor2) {
        Point point1 = convertCoordinates(position1);
        Point point2 = basicMazes[floor1].getEndPoint();

        List<MovingAction> movingActions = basicMazes[floor1].getShortestPath(point1, point2);

        foreach (MovingAction movingAction in movingActions) {
          path.Add(new Vector3(movingAction.dc * BLOCK_SIZE, 0, movingAction.dr * BLOCK_SIZE));
        }
        
        while (floor1 < floor2) {
          
          // Up
          path.Add(new Vector3(0, WALL_HEIGHT * BLOCK_SIZE + CEILING_THICKNESS, 0));
          
          floor1++;
          point1 = basicMazes[floor1].getStartPoint();
          if (floor1 == floor2) {
            point2 = convertCoordinates(position2);
          } else {
            point2 = basicMazes[floor1].getEndPoint();
          }
          
          foreach (MovingAction movingAction in movingActions) {
            path.Add(new Vector3(movingAction.dc * BLOCK_SIZE, 0, movingAction.dr * BLOCK_SIZE));
          }
        }
      } else {
        Point point1 = convertCoordinates(position1);
        Point point2 = basicMazes[floor1].getStartPoint();

        List<MovingAction> movingActions = basicMazes[floor1].getShortestPath(point1, point2);

        foreach (MovingAction movingAction in movingActions) {
          path.Add(new Vector3(movingAction.dc * BLOCK_SIZE, 0, movingAction.dr * BLOCK_SIZE));
        }
        
        while (floor1 > floor2) {
          
          // Up
          path.Add(new Vector3(0, -1 * (WALL_HEIGHT * BLOCK_SIZE + CEILING_THICKNESS), 0));
          
          floor1--;
          point1 = basicMazes[floor1].getStartPoint();
          if (floor1 == floor2) {
            point2 = convertCoordinates(position2);
          } else {
            point2 = basicMazes[floor1].getEndPoint();
          }
          
          foreach (MovingAction movingAction in movingActions) {
            path.Add(new Vector3(movingAction.dc * BLOCK_SIZE, 0, movingAction.dr * BLOCK_SIZE));
          }
        }
      }

    } else {
      Point point1 = convertCoordinates(position1);
      Point point2 = convertCoordinates(position2);
      
      List<MovingAction> movingActions = basicMazes[floor1].getShortestPath(point1, point2);

      foreach (MovingAction movingAction in movingActions) {
        path.Add(new Vector3(movingAction.dc * BLOCK_SIZE, 0, movingAction.dr * BLOCK_SIZE));
      }
    }

    return path;
  }

}
