using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

  // True if the elevator is is up, false if elevator is down
  private bool isUp = true;

  public float waitingTime = 5;
  private float waitedTime = 0;

  // True if the elevator is moving
  private bool isMoving = false;

  public float movingSpeed = 2;
  private float movingDistance = 5;
  private float movedDistance = 0;


  void Update () {
    if (isMoving) {
      movedDistance += movingSpeed * Time.deltaTime;
      transform.position += new Vector3(0, (isUp ? 1 : -1) * (movingSpeed * Time.deltaTime), 0);
      if (movedDistance >= movingDistance) {
        movedDistance  = 0;
        isMoving = false;
        isUp = !isUp;
      }
    } else {
      waitedTime += Time.deltaTime;
      if (waitedTime >= waitingTime) {
        waitedTime = 0;
        isMoving = true;
      }
    }
  }

  public void setMovingDistance (float distance) {
    movingDistance = distance;
  }

}
