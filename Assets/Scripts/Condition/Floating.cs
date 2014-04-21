using UnityEngine;
using System.Collections;

public class Floating : MonoBehaviour {

  public float speed, distance;
  private float movedDistance = 0;
  private bool isMovingUp = true;
	
  void Update () {
    float deltaDistance = speed * Time.deltaTime;
    movedDistance += deltaDistance;
    if (movedDistance >= distance) {
      deltaDistance -= (movedDistance - distance);
    }
    transform.position += new Vector3(0, (isMovingUp ? -1 : 1) * deltaDistance, 0);
    if (movedDistance >= distance) {
      movedDistance = 0;
      isMovingUp = !isMovingUp;
    }
  }
}
