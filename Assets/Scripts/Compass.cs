using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour {

  public Texture compassBackgroundTexture;
  public Texture compassTexture;
  public int positionX;
  public int positionY;
  public int width = 200;
  public int height = 200;

  private GameObject compass;
  private float angle = 0;
  private Vector2 pivotPoint;


  private GameObject[] items;

  void Start () {
    positionX = Screen.width - 250;
    positionY = Screen.height - 250;
    pivotPoint = new Vector2(positionX + width / 2, positionY + height / 2);
    compass = new GameObject();
    compass.name = "compass";
  }

  void Update () {
    items = GameObject.FindGameObjectsWithTag("Item");
    if (items.Length > 0) {
      Vector3 nowPosition2D = new Vector3(transform.position.x, 0, transform.position.z);
      Vector3 targetPosition2D = new Vector3(items[0].transform.position.x, 0, items[0].transform.position.z);
      float minDistance = Vector3.Distance(nowPosition2D, targetPosition2D);
      int nearestItemIndex = 0;
      for (int i = 1; i < items.Length; i++) {
        targetPosition2D = new Vector3(items[i].transform.position.x, 0, items[i].transform.position.z);
        float distance = Vector3.Distance(nowPosition2D, targetPosition2D);
        if (distance < minDistance) {
          minDistance = distance;
          nearestItemIndex = i;
        }
      }
      compass.transform.position = new Vector3(transform.position.x, items[nearestItemIndex].transform.position.y, transform.position.z);
      compass.transform.LookAt(items[nearestItemIndex].transform.position);
      angle = compass.transform.eulerAngles.y - transform.eulerAngles.y;
    } else {
      Application.LoadLevel("MainMenu");
    }
  }

  void OnGUI () {
    GUI.DrawTexture(new Rect(positionX, positionY, width, height), compassBackgroundTexture);
    GUIUtility.RotateAroundPivot(angle, pivotPoint);
    GUI.DrawTexture(new Rect(positionX, positionY, width, height), compassTexture);
  }
}
