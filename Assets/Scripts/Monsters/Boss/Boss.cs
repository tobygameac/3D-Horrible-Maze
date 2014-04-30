using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour {

  public bool isDebugging;

  public Texture attackingMaskTexture;
  private float maskAlpha;

  // Speed parameter
  public float movingSpeed;
  public float acceleration;

  // Probabilities for states
  public float probabilityOfWandering;
  public float probabilityOfChasingPlayer;

  // Maximun distance for changing to staring state
  public float staringTriggerRadius;
  // Maximun angle for changing to attacking state
  public float tracingTriggerAngle;
  public float tracingTriggerCheckDeltaAngle;
  public float offScreenDot;
  // Maximun distance for changing to attacking state
  public float attackingRadius;

  public float mentalityRestorePerSecond;
  public float mentalityAbsorbPerSecond;

  public Texture[] QTEArrowTextures;
  public Texture QTEProgressBarTexture;
  public Texture QTEProgressBarBackgroundTexture;
  public Texture QTETimeLimitBarTexture;
  public Texture QTETimeLimitBarBackgroundTexture;
  public int QTELength;
  public float QTETimeLimitPerKey;
  private float QTETimeLimit;
  private float QTETimeUsed;
  public float mentalityAbsorbPerQTEWrong;
  private List<int> QTEvent = new List<int>();
  private bool perfectQTE;
  private float QTEShowedTime;
  public float QTESwitchTime;
  private bool QTEsmall;

  public float stunningTime;
  private float stunnedTime;

  private static System.Random random = new System.Random(); // Only need one random seed

  private MazeGenerator maze;

  private GameObject lookAtPoint;

  private GameObject player;
  private Mentality playerMentality;
  private CharacterMotor playerCharacterMotor;
  private MouseLook2 playerMouseLook;
  private GameObject fakeBoss;
  // Virtual object for simulating rotation
  private GameObject virtualPlayer;

  private Renderer[] childrenRenderers;
  private SphereCollider sphereCollider;

  // State
  private bool isMakingDecision = true;
  private bool isMoving = false;

  private bool isWandering = false;
  private bool isChasingPlayer = false;
  private bool isStaring = false;
  private bool isTracing = false;
  private bool isStunning = false;
  private bool isAttacking = false;
  private bool isCameraMoving = false;
  public float cameraMovingTime = 0.5f;
  private float cameraMovedTime;
  private Vector3 cameraMovingVector;

  public AudioClip tracingSound;
  private SoundEffectManager soundEffectManager;

  private Scoreboard scoreboard;

  private List<Vector3> path;
  private int pathIndex;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
    lookAtPoint = GameObject.FindWithTag("LookAtPoint");
    player = GameObject.FindWithTag("Player");
    playerMentality = player.GetComponent<Mentality>();
    playerCharacterMotor = player.GetComponent<CharacterMotor>();
    playerMouseLook = player.GetComponent<MouseLook2>();
    fakeBoss = GameObject.FindWithTag("FakeBoss");
    fakeBoss.renderer.enabled = false;

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
    soundEffectManager.adjustSound();

    scoreboard = GameObject.FindWithTag("Main").GetComponent<Scoreboard>();

    virtualPlayer = new GameObject();
    virtualPlayer.name = "virtual player";

    childrenRenderers = GetComponentsInChildren<Renderer>();
    sphereCollider = GetComponent<SphereCollider>();
  }

  void Update () {

    if (GameState.state == GameState.LOSING) {
      audio.Stop();
      Destroy(gameObject);
      return;
    }

    if (GameState.state != GameState.PLAYING) {
      return;
    }

    if (isStunning) {
      stunnedTime += Time.deltaTime;
      if (stunnedTime >= stunningTime) {
        isStunning = false;
        isMakingDecision = true;
        for (int i = 0; i < childrenRenderers.Length; i++) {
          float r = childrenRenderers[i].material.color.r;
          float g = childrenRenderers[i].material.color.g;
          float b = childrenRenderers[i].material.color.b;
          float a = 1;
          childrenRenderers[i].material.color = new Color(r, g, b, a);
        }
        collider.enabled = true;
      }
      return;
    }

    if (isCameraMoving) {
      cameraMovedTime += Time.deltaTime;
      player.transform.eulerAngles += cameraMovingVector * (Time.deltaTime / cameraMovingTime);
      if (cameraMovedTime >= cameraMovingTime) {
        isCameraMoving = false;
        player.transform.LookAt(lookAtPoint.transform);
        fakeBoss.renderer.enabled = true;
        for (int i = 0; i < childrenRenderers.Length; i++) {
          float r = childrenRenderers[i].material.color.r;
          float g = childrenRenderers[i].material.color.g;
          float b = childrenRenderers[i].material.color.b;
          float a = 0;
          childrenRenderers[i].material.color = new Color(r, g, b, a);
        }
      }
    }

    if (isMoving) {
      // Do nothing when boss is moving
      return;
    }

    if (isStaring) {
      if (lookAtAndCheckIfSeenPlayer()) {
        if (isVisible()) {
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
        isMakingDecision = true;
        audio.Stop();
        return;
      }

      if (maze.getFloor(transform.position.y) == maze.getFloor(player.transform.position.y)) {

        if (playerIsInTheRadius(attackingRadius)) {
          turnToAttackingState();
          return;
        } else if (playerIsInTheRadius(staringTriggerRadius)) {
          if (isVisible()) {
            // Restore the mentality of the player
            playerMentality.gain(mentalityRestorePerSecond * Time.deltaTime);
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

      QTEShowedTime += Time.deltaTime;
      if (QTEShowedTime >= QTESwitchTime) {
        QTEShowedTime = 0;
        QTEsmall = !QTEsmall;
      }

      maskAlpha = ((int)(Time.time * 100) % 100) / 100.0f;
      // Absorb the mentality of the player
      playerMentality.use(mentalityAbsorbPerSecond * Time.deltaTime);

      bool wrong = false;
      bool success = false;

      QTETimeUsed += Time.deltaTime;
      if (QTETimeUsed >= QTETimeLimit) {
        wrong = true;
      }

      KeyCode[] secondHotkeys = new KeyCode[4];
      secondHotkeys[0] = KeyCode.W;
      secondHotkeys[1] = KeyCode.S;
      secondHotkeys[2] = KeyCode.D;
      secondHotkeys[3] = KeyCode.A;

      for (int direction = 0; direction < 4 && !wrong; direction++) {
        if (Input.GetKeyDown(KeyCode.UpArrow + direction) || Input.GetKeyDown(secondHotkeys[direction])) {
          if (QTEvent[0] == direction) {
            soundEffectManager.playQTEHitSound();
            success = true;
          } else {
            soundEffectManager.playQTEMissSound();
            wrong = true;
          }
        }
      }
      
      if (wrong) {
        perfectQTE = false;
        playerMentality.use(mentalityAbsorbPerQTEWrong);
        QTEvent = generateQTE(QTELength);
        QTETimeUsed = 0;
      }

      if (success) {
        QTEvent.RemoveAt(0);
        if (QTEvent.Count == 0) {
          turnToStunningState();
          float bonusScale = 1 - (QTETimeUsed / QTETimeLimit) + (perfectQTE ? 1 : 0);
          scoreboard.addScore((int)(100 * (QTELength * (1 + bonusScale))));
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

  void OnGUI () {
    
    if (GameState.state != GameState.PLAYING) {
      return;
    }

    if (isAttacking) {
      Color originalColor = GUI.color;
      GUI.color = new Color(1, 1, 1, maskAlpha);
      GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), attackingMaskTexture);
      GUI.color = originalColor;

      GUI.depth = 1;

      if (QTEvent.Count > 0) {
        int QTEArrowSize = Screen.height / 8;
        if (QTEsmall) {
          QTEArrowSize = Screen.height / 10;
        }
        Texture QTEArrowTexture = QTEArrowTextures[QTEvent[0]];
        GUI.DrawTexture(new Rect((Screen.width - QTEArrowSize) / 2, Screen.height / 5, QTEArrowSize, QTEArrowSize), QTEArrowTexture);

        int QTETimeLimitBarWidth = Screen.width / 5;
        int QTETimeLimitBarHeight = Screen.height / 32;

        GUI.DrawTexture(new Rect((Screen.width - QTETimeLimitBarWidth) / 2, Screen.height / 24, QTETimeLimitBarWidth, QTETimeLimitBarHeight), QTETimeLimitBarBackgroundTexture);
        
        GUILayout.BeginArea(new Rect((Screen.width - QTETimeLimitBarWidth) / 2, Screen.height / 24, (int)(QTETimeLimitBarWidth * (1 - (QTETimeUsed / (float)QTETimeLimit))), QTETimeLimitBarHeight));
        GUI.DrawTexture(new Rect(0, 0, QTETimeLimitBarWidth, QTETimeLimitBarHeight), QTETimeLimitBarTexture);
        GUILayout.EndArea();

        int QTEProgressBarWidth = Screen.width / 5;
        int QTEProgressBarHeight = Screen.height / 16;

        GUI.DrawTexture(new Rect((Screen.width - QTEProgressBarWidth) / 2, Screen.height / 8, QTEProgressBarWidth, QTEProgressBarHeight), QTEProgressBarBackgroundTexture);
        
        GUILayout.BeginArea(new Rect((Screen.width - QTEProgressBarWidth) / 2, Screen.height / 8, (int)(QTEProgressBarWidth * QTEvent.Count / (float)QTELength), QTEProgressBarHeight));
        GUI.DrawTexture(new Rect(0, 0, QTEProgressBarWidth, QTEProgressBarHeight), QTEProgressBarTexture);
        GUILayout.EndArea();
      }

      return;
    }

  }

  public List<int> generateQTE (int QTEventLength) {
    List<int> QTEvent = new List<int>();
    for (int i = 0; i < QTEventLength; i++) {
      QTEvent.Add(random.Next(4));
    }
    return QTEvent;
  }

  public void addQTELength (int addedLength) {
    QTELength += addedLength;
  }

  public void addQTEWrongMentality (float addedMentality) {
    mentalityAbsorbPerQTEWrong += addedMentality;
  }

  public void addQTETimeLimit (float addedTimeLimit) {
    QTETimeLimitPerKey += addedTimeLimit;
    if (QTETimeLimitPerKey < 0.3f) {
      QTETimeLimitPerKey = 0.3f;
    }
  }

  private void lookAtPlayer () {
    Vector3 targetPosition = player.transform.position;
    targetPosition.y = transform.position.y;
    transform.LookAt(targetPosition);
  }

  private bool hasSeenPlayer () {
    RaycastHit hit;
    if (Physics.Linecast(transform.position, player.transform.position, out hit)) {
      if (hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.name == "playerBody") {
        return true;
      }
    }
    return false;
  }

  private bool lookAtAndCheckIfSeenPlayer () {
        /*
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
        */
    lookAtPlayer();
    if (playerIsInTheRadius(staringTriggerRadius)) {
      return hasSeenPlayer();
    }
    return false;
  }

  private bool isVisible () {
    Vector3 fwd = player.transform.forward;
    Vector3 other = (transform.position - player.transform.position).normalized;
    
    float dotProduct = Vector3.Dot(fwd, other);
    
    if (dotProduct > offScreenDot) {
      return hasSeenPlayer();
    }

    return false;
    /*
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
    */
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
    } else {
      isChasingPlayer = true;
    }

    isMakingDecision = false;
  }

  private void turnToStaringState () {
    // Stop all other states
    isWandering = false;
    isChasingPlayer = false;

    isStaring = true;
  }

  private void turnToTracingState () {
    playAudio(tracingSound);
    isStaring = false;
    isTracing = true;
  }

  private void turnToAttackingState () {
    lookAtPlayer();
    isTracing = false;
    isAttacking = true;
    playerCharacterMotor.canControl = false;
    playerMouseLook.enabled = false;
    isCameraMoving = true;
    cameraMovedTime = 0;
    virtualPlayer.transform.position = player.transform.position;
    virtualPlayer.transform.LookAt(lookAtPoint.transform);
    cameraMovingVector = virtualPlayer.transform.eulerAngles - player.transform.eulerAngles;
    Vector3 anotherMovingVector = player.transform.eulerAngles - virtualPlayer.transform.eulerAngles;
    if (anotherMovingVector.magnitude < cameraMovingVector.magnitude) {
      cameraMovingVector = anotherMovingVector;
    }
    // Generate the first event
    perfectQTE = true;
    QTEvent = generateQTE(QTELength);
    QTETimeLimit = QTETimeLimitPerKey * QTELength;
    QTETimeUsed = 0;
  }

  private void turnToStunningState () {
    fakeBoss.renderer.enabled = false;
    audio.Stop();
    isStaring = false;
    isTracing = false;
    isAttacking = false;
    isStunning = true;
    stunnedTime = 0;
    playerCharacterMotor.canControl = true;
    playerMouseLook.enabled = true;
    for (int i = 0; i < childrenRenderers.Length; i++) {
      float r = childrenRenderers[i].material.color.r;
      float g = childrenRenderers[i].material.color.g;
      float b = childrenRenderers[i].material.color.b;
      float a = 0.4f;
      childrenRenderers[i].material.color = new Color(r, g, b, a);
    }
    sphereCollider.enabled = false;
  }

  private void playAudio (AudioClip audioClip) {
    audio.Stop();
    audio.clip = audioClip;
    audio.Play();
  }

}
