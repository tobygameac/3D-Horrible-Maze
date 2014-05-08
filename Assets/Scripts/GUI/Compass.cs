using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Compass : MonoBehaviour {

  public Texture compassBackgroundTexture;
  public Texture compassPointerTexture;

  private int positionOnScreenX;
  private int positionOnScreenY;

  // Virtual object for calculating position
  private GameObject virtualCompass;

  // Compass rotating parameter
  private float angle = 0;
  private Vector2 pivotPoint;

  private MazeGenerator maze;
  private int mazeH;

  // Base Y position of all floors
  private List<float> baseY = new List<float>();

  void Start () {
    virtualCompass = new GameObject();
    virtualCompass.name = "virtual compass";

    // Get base Y position of all floors
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
    mazeH = maze.MAZE_H;
    for (int h = 0; h < mazeH; h++) {
      baseY.Add(maze.getBaseY(h));
    }
  }

  void Update () {

    // Get the floor where player stand
    int floorOfPlayer = maze.getFloor(transform.position.y) + 1;

    GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

    List<GameObject> targetAtFloor = new List<GameObject>();

    // Get all items at the same floor
    for (int i = 0; i < items.Length; i++) {
      Transform parentTransform = items[i].transform;
      while (parentTransform.parent != null && parentTransform.name.IndexOf("floor") == -1) {
        parentTransform = parentTransform.parent;
      }
      if (parentTransform.name == ("floor" + floorOfPlayer)) {
        targetAtFloor.Add(items[i]);
      }
    }

    // Get all elevators at the same floor
    GameObject[] elevators = GameObject.FindGameObjectsWithTag("Elevator");
    for (int i = 0; i < elevators.Length; i++) {
      Transform parentTransform = elevators[i].transform;
      while (parentTransform.parent != null && parentTransform.name.IndexOf("floor") == -1) {
        parentTransform = parentTransform.parent;
      }
      if (parentTransform.name == ("floor" + floorOfPlayer)) {
        targetAtFloor.Add(elevators[i]);
      }
    }

    // If there's nothing to point, then point to the elevator of previous floor
    if (targetAtFloor.Count == 0) {
      for (int i = 0; i < elevators.Length; i++) {
        Transform parentTransform = elevators[i].transform;
        while (parentTransform.parent != null && parentTransform.name.IndexOf("floor") == -1) {
          parentTransform = parentTransform.parent;
        }
        if (parentTransform.name == ("floor" + (floorOfPlayer - 1))) {
          targetAtFloor.Add(elevators[i]);
        }
      }
    }

    Vector3 nowPosition2D = new Vector3(transform.position.x, 0, transform.position.z);

    Vector3 targetPosition2D = new Vector3(targetAtFloor[0].transform.position.x, 0, targetAtFloor[0].transform.position.z);
    float minDistance = Vector3.Distance(nowPosition2D, targetPosition2D);
    Transform nearestTransform = targetAtFloor[0].transform;
    for (int i = 1; i < targetAtFloor.Count; i++) {
      targetPosition2D = new Vector3(targetAtFloor[i].transform.position.x, 0, targetAtFloor[i].transform.position.z);
      float distance = Vector3.Distance(nowPosition2D, targetPosition2D);
      if (distance < minDistance) {
        minDistance = distance;
        nearestTransform = targetAtFloor[i].transform;
      }
    }
    virtualCompass.transform.position = new Vector3(transform.position.x, nearestTransform.position.y, transform.position.z);
    virtualCompass.transform.LookAt(nearestTransform.position);
    angle = virtualCompass.transform.eulerAngles.y - transform.eulerAngles.y;
  }

  void OnGUI () {
    if (GameState.state != GameState.PLAYING) {
      return;
    }
    GUI.depth = 0;

    int width = Screen.width / 6;
    int height = width;
    positionOnScreenX = Screen.width - width - width / 4;
    positionOnScreenY = Screen.height - height - height / 4;
    pivotPoint = new Vector2(positionOnScreenX + width / 2, positionOnScreenY + height / 2);
    GUI.DrawTexture(new Rect(positionOnScreenX, positionOnScreenY, width, height), compassBackgroundTexture);
    GUIUtility.RotateAroundPivot(angle, pivotPoint);
    GUI.DrawTexture(new Rect(positionOnScreenX, positionOnScreenY, width, height), compassPointerTexture);
  }
}
