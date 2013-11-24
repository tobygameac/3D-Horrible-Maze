using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(SphereCollider))]

public class Boss : MonoBehaviour {

  public float speed = 0.75f;
  public float acceleration = 0.01f;

  private MazeGenerator maze;
  private GameObject player;

  private static System.Random random = new System.Random(); // Only need one random seed

  private bool isMakingDecision = true;
  private bool isMoving = false;
  private bool isFindingItem = false;
  private bool isFindingPlayer = false;
  private bool isStaringAtPlayer = false;

  private Vector3 movingVector;

  public float triggerRadius;

  void Start () {
    GetComponent<SphereCollider>().radius = triggerRadius;
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
    player = GameObject.FindWithTag("Player");
  }

  void Update () {

    if (isMoving) {

      if (movingVector.magnitude == 0) {
        isMoving = false;
        return;
      }

      float movingDistance = Time.deltaTime * speed;

      Vector3 move = Vector3.zero;

      if (movingVector.x != 0) {
        if (movingDistance <= Mathf.Abs(movingVector.x)) {
          move.x = (movingVector.x > 0 ? 1 : -1) * movingDistance;
          movingVector.x -= move.x;
        } else {
          move.x = movingVector.x;
          movingVector.x = 0;
        }
      }

      if (movingVector.y != 0) {
        if (movingDistance <= Mathf.Abs(movingVector.y)) {
          move.y = (movingVector.y > 0 ? 1 : -1) * movingDistance;
          movingVector.y -= move.y;
        } else {
          move.y = movingVector.y;
          movingVector.y = 0;
        }
      }

      if (movingVector.z != 0) {
        if (movingDistance <= Mathf.Abs(movingVector.z)) {
          move.z = (movingVector.z > 0 ? 1 : -1) * movingDistance;
          movingVector.z -= move.z;
        } else {
          move.z = movingVector.z;
          movingVector.z = 0;
        }
      }

      transform.position += move;
      
      speed += acceleration * Time.deltaTime;
      return;
    }

    List<Vector3> path = maze.getShortestPath(transform.position, player.transform.position);
    isMoving = true;

    if (path.Count > 0) {
      movingVector = path[0];
    }
    
    if (isMakingDecision) {
      makeDecision();
    }

    if (isFindingItem) {
    }

    if (isFindingPlayer) {
    }

  }

  void OnTriggerStay (Collider other) {

    if (isStaringAtPlayer && other.tag == "Player") {
    }

  }

  private void makeDecision () {

    int dice = random.Next(100);
    if (dice < 50) { // 50% Choose a random block as target
    } else if (dice < 75) { // 25% Choose an item as target
      isFindingItem = true;
    } else { // 25% Choose the player as target
      isFindingPlayer = true;
    }

    isMakingDecision = false;
  }

}
