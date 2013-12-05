using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(SphereCollider))]

public class Boss : MonoBehaviour {

  public bool isDebugging;

  public float movingSpeed = 0.75f;
  public float acceleration = 0.01f;

  private MazeGenerator maze;
  private GameObject player;

  private static System.Random random = new System.Random(); // Only need one random seed

  private bool isMakingDecision = true;
  private bool isMoving = false;
  private bool isFindingItem = false;
  private bool isFindingPlayer = false;
  private bool isStaringAtPlayer = false;

  public float triggerRadius;

  void Start () {
    GetComponent<SphereCollider>().radius = triggerRadius;
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
    player = GameObject.FindWithTag("Player");
  }

  int ind;

  void Update () {

    if (!isMoving) {
      List<Vector3> path = maze.getShortestPath(transform.position, player.transform.position);
      if (path.Count > 0) {
         StartCoroutine(move(path[0]));
      }
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

  private IEnumerator move(Vector3 movingVector) {
    
    Vector3 startPosition = transform.position;
    Vector3 targetPosition = transform.position + movingVector;
    
    isMoving = true;

    float percent = 0;
 
    while (percent < 1) {
      movingSpeed = movingSpeed + Time.deltaTime * acceleration;
      percent += Time.deltaTime * movingSpeed / movingVector.magnitude;
      transform.position = Vector3.Lerp(startPosition, targetPosition, percent);
      yield return null;
    }

    isMoving = false;

    yield return 0;
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
