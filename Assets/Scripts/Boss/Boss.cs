using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour {

  public bool isDebugging;

  // Speed parameter
  public float movingSpeed = 0.75f;
  public float acceleration = 0.01f;

  // Probabilities for states
  public float probabilityOfWandering = 0.7f;
  public float probabilityOfGettingItem = 0.2f;
  public float probabilityOfChasingPlayer = 0.1f;

  // Maximun distance for changing to staring state
  public float staringTriggerRadius = 10.0f;
  // Maximun angle for changing to attacking state
  public float tracingTriggerAngle = 45.0f;
  public float tracingTriggerCheckDeltaAngle = 5.0f;
  // Maximun distance for changing to attacking state
  public float attackingRadius = 3.0f;

  public float mentalityRestorePercentPerSecond = 0.02f;
  public float mentalityAbsorbPercentPerSecond = 0.04f;

  public int QTELength = 10;
  private List<int> QTEvent = new List<int>();

  public float stunningTime = 5.0f;
  private float stunnedTime;

  private static System.Random random = new System.Random(); // Only need one random seed

  private MazeGenerator maze;

  private GameObject player;
  private Mentality playerMentality;
  private CharacterMotor playerCharacterMotor;
  // Virtual object for simulating rotation
  private GameObject virtualPlayer;

  // State
  private bool isMakingDecision = true;
  private bool isMoving = false;
  private GameObject itemCarring = null;

  private bool isWandering = false;
  private bool isGettingItem = false;
  private bool isChasingPlayer = false;
  private bool isStaring = false;
  private bool isTracing = false;
  private bool isStunning = false;
  private bool isAttacking = false;

  private List<Vector3> path;
  private int pathIndex;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
    player = GameObject.FindWithTag("Player");
    playerMentality = player.GetComponent<Mentality>();
    playerCharacterMotor = player.GetComponent<CharacterMotor>();
    virtualPlayer = new GameObject();
    virtualPlayer.name = "virtual player";
  }

  void Update () {

    if (isStunning) {
      stunnedTime += Time.deltaTime;
      if (stunnedTime >= stunningTime) {
        isStunning = false;
        isMakingDecision = true;
      }
      return;
    }

    if (isMoving) {
      // Do nothing when boss is moving
      return;
    }

    if (isStaring) {
      if (lookAtAndCheckIfSeenPlayer()) {
        if (playerIsStaringAtTheBoss() || playerIsInTheRadius(attackingRadius)) {
          turnToTracingState();
        }
        return;
      }
      if (!lookAtAndCheckIfSeenPlayer()) {
        // Change to making decision state
        isStaring = false;
        isMakingDecision = true;
      }
      return;
    }

    if (isTracing) {
      
      if (!lookAtAndCheckIfSeenPlayer()) {
        isTracing = false;

        if (itemCarring) {
          //putItem
        }

        isStunning = true;
        return;
      }

      if (maze.getFloor(transform.position.y) == maze.getFloor(player.transform.position.y)) {

        if (playerIsInTheRadius(attackingRadius)) {
          turnToAttackingState();
        } else if (playerIsInTheRadius(staringTriggerRadius)) {
          if (playerIsStaringAtTheBoss()) {
            float maxMentalityPoint = playerMentality.maxMentalityPoint;
            // Restore the mentality of the player
            playerMentality.gain(maxMentalityPoint * mentalityRestorePercentPerSecond * Time.deltaTime);
          }
        }

      }

      path = maze.getSimpleShortestPath(transform.position, player.transform.position);
      if (path.Count > 0) {
        // Do the first moving instruction
        StartCoroutine(move(path[0]));
      }

      return;
    }

    if (isAttacking) {
      playerCharacterMotor.canControl = false;
      float maxMentalityPoint = playerMentality.maxMentalityPoint;
      // Absorb the mentality of the player
      playerMentality.use(maxMentalityPoint * mentalityAbsorbPercentPerSecond * Time.deltaTime);

      QTE.showQTE(QTEvent);
      
      bool wrong = false;
      bool success = false;

      for (int direction = 0; direction < 4; direction++) {
        if (Input.GetKeyDown(KeyCode.UpArrow + direction)) {
          if (QTEvent[0] == direction) {
            success = true;
          } else {
            wrong = true;
          }
        }
      }
      
      if (wrong) {
        QTEvent = QTE.generateQTE(QTELength);
      }

      if (success) {
        QTEvent.RemoveAt(0);
        QTE.showQTE(QTEvent);
        if (QTEvent.Count == 0) {
          playerCharacterMotor.canControl = true;
          isAttacking = false;

          if (itemCarring) {
            //putItem
          }

          isStunning = true;
          stunnedTime = 0;
        }
      }

      return;
    }

    if (lookAtAndCheckIfSeenPlayer()) {
      turnToStaringState();
      return;
    }
    
    if (isWandering) {
      if (pathIndex >= path.Count) { // Finish
        isWandering = false;
        isMakingDecision = true;
        return;
      }
      // Do the next moving instruction
      StartCoroutine(move(path[pathIndex++]));
    }

    if (isGettingItem) {
    }

    if (isChasingPlayer) {
      path = maze.getSimpleShortestPath(transform.position, player.transform.position);
      if (path.Count > 0) {
        // Do the first moving instruction
         StartCoroutine(move(path[0]));
      } else {
        isChasingPlayer = false;
        isMakingDecision = true;
        return;
      }
    }
    
    if (isMakingDecision) {
      makeDecision();
    }

  }

  private void lookAtPlayer () {
    Vector3 targetPosition = player.transform.position;
    targetPosition.y = transform.position.y;
    transform.LookAt(targetPosition);
  }

  private bool lookAtAndCheckIfSeenPlayer () {
    // At the same floor
    if (maze.getFloor(transform.position.y) == maze.getFloor(player.transform.position.y)) {
      if (playerIsInTheRadius(staringTriggerRadius)) {
        lookAtPlayer();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
          if (hit.transform.tag == "Player") {
            return true;
          }
        }
      }
    }
    return false;
  }

  private bool playerIsStaringAtTheBoss () {
    // At the same floor
    if (maze.getFloor(transform.position.y) == maze.getFloor(player.transform.position.y)) {
      
      virtualPlayer.transform.position = player.transform.position;
      // Make sure virtual player only rotate with Y-axis
      virtualPlayer.transform.position = new Vector3(virtualPlayer.transform.position.x, transform.position.y, virtualPlayer.transform.position.z);
      // Let virtual player look at the boss
      virtualPlayer.transform.LookAt(transform.position);

      float angleDifference = Mathf.Abs(virtualPlayer.transform.eulerAngles.y - player.transform.eulerAngles.y);
      if (angleDifference <= tracingTriggerAngle || angleDifference >= 360 - tracingTriggerAngle) {
        if (virtualPlayer.transform.eulerAngles.y > player.transform.eulerAngles.y) {
          // Check all angles between player's angle and virtual player's angle
          while (virtualPlayer.transform.eulerAngles.y >= player.transform.eulerAngles.y) {
            RaycastHit hit;
            if (Physics.Raycast(virtualPlayer.transform.position, virtualPlayer.transform.forward, out hit)) {
              if (hit.transform.tag == "Boss") {
                return true;
              }
            }
            virtualPlayer.transform.eulerAngles += new Vector3(0, -tracingTriggerCheckDeltaAngle, 0);
          }
        } else {
          // Check all angles between player's angle and virtual player's angle
          while (virtualPlayer.transform.eulerAngles.y <= player.transform.eulerAngles.y) {
            RaycastHit hit;
            if (Physics.Raycast(virtualPlayer.transform.position, virtualPlayer.transform.forward, out hit)) {
              if (hit.transform.tag == "Boss") {
                return true;
              }
            }
            virtualPlayer.transform.eulerAngles += new Vector3(0, tracingTriggerCheckDeltaAngle, 0);
          }
        }
      }
    }
    return false;
  }

  private bool playerIsInTheRadius (float radius) {
    float dx = transform.position.x - player.transform.position.x;
    float dz = transform.position.z - player.transform.position.z;
    float distance = Mathf.Sqrt(dx * dx + dz * dz);
    return distance <= radius;
  }

  private IEnumerator move (Vector3 movingVector) {
    
    Vector3 startPosition = transform.position;
    Vector3 targetPosition = transform.position + movingVector;
    
    isMoving = true;

    float percent = 0;
 
    while (percent < 1) {

      if (isTracing || isStaring) {
        lookAtPlayer();
      }

      movingSpeed = movingSpeed + Time.deltaTime * acceleration;
      percent += Time.deltaTime * movingSpeed / movingVector.magnitude;
      transform.position = Vector3.Lerp(startPosition, targetPosition, percent);
      yield return null;
    }

    isMoving = false;

    yield return 0;
  }

  private void makeDecision () {

    double dice = random.NextDouble();
    if (dice < probabilityOfWandering) {

      // Get a random floor
      int randomH = random.Next(maze.MAZE_H);

      // The offset between different floors
      Point offset = maze.getOffset(randomH);

      Point target = maze.getRandomAvailableBlock(randomH);

      // Calculate the real position in the world
      int realR = (target.r + offset.r) * maze.BLOCK_SIZE;
      int realC = (target.c + offset.c) * maze.BLOCK_SIZE;
      
      Vector3 targetPosition = new Vector3(realC, maze.getBaseY(randomH) + transform.localScale.y + 0.11f, realR);
      path = maze.getShortestPath(transform.position, targetPosition);

      pathIndex = 0;
      isWandering = true;
    } else if (itemCarring == null && dice < probabilityOfWandering + probabilityOfGettingItem) {
      /*
      pathIndex = 0;
      isGettingItem = true;
      */
      return;
    } else {
      isChasingPlayer = true;
    }

    isMakingDecision = false;
  }

  private void turnToStaringState () {
    // Stop all other states
    isWandering = false;
    isGettingItem = false;
    isChasingPlayer = false;

    isStaring = true;
  }

  private void turnToTracingState () {
    isStaring = false;
    isTracing = true;
  }

  private void turnToAttackingState () {
    isTracing = false;
    isAttacking = true;
    // Generate the first event
    QTEvent = QTE.generateQTE(QTELength);
  }

}
