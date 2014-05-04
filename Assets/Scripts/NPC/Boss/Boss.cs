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
  public float mustAttackingRadius;

  public float mentalityRestorePerSecond;
  public float mentalityAbsorbPerSecond;

  public Texture[] QTEArrowTextures;
  public Texture QTEProgressBarTexture;
  public Texture QTEProgressBarBackgroundTexture;
  public Texture QTETimeLimitBarTexture;
  public Texture QTETimeLimitBarBackgroundTexture;
  public int QTELength;
  public float QTETimeLimitPerKey;
  public float minQTETimeLimitPerKey;
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
  public Texture[] fakeBossTextures;
  private int fakeBossTextureIndex;

  // Virtual object for simulating rotation
  private GameObject virtualPlayer;

  private Renderer[] childrenRenderers;
  private SphereCollider sphereCollider;

  private Vector3 startMovingPosition;

  private NPCState npcState;

  private bool isMoving = false;

  private bool isCameraMoving = false;
  public float cameraMovingTime = 0.5f;
  private float cameraMovedTime;
  private Quaternion cameraQuaternion;

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

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
    soundEffectManager.adjustSound();

    scoreboard = GameObject.FindWithTag("Main").GetComponent<Scoreboard>();

    virtualPlayer = new GameObject();
    virtualPlayer.name = "virtual player";

    childrenRenderers = GetComponentsInChildren<Renderer>();
    sphereCollider = GetComponent<SphereCollider>();
    
    npcState = GetComponent<NPCState>();
    npcState.state = NPCState.MAKING_DECISION;
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

    if (npcState.state == NPCState.STUNNING) {
      stunnedTime += Time.deltaTime;
      if (stunnedTime >= stunningTime) {
        npcState.state = NPCState.MAKING_DECISION;
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

    if (npcState.state != NPCState.ATTACKING && playerIsInTheRadius(mustAttackingRadius, true)) {
      stopMoving();
      turnToAttackingState();
      return;
    }
      

    if (isCameraMoving) {
      cameraMovedTime += Time.deltaTime;
      player.transform.rotation = Quaternion.Slerp(player.transform.rotation, cameraQuaternion, Time.deltaTime / cameraMovingTime);
      if (cameraMovedTime >= cameraMovingTime) {
        // soundEffectManager.playZombieGaspSound();
        isCameraMoving = false;
        player.transform.LookAt(lookAtPoint.transform);
        fakeBossTextureIndex = random.Next(fakeBossTextures.Length);
        for (int i = 0; i < childrenRenderers.Length; i++) {
          float r = childrenRenderers[i].material.color.r;
          float g = childrenRenderers[i].material.color.g;
          float b = childrenRenderers[i].material.color.b;
          float a = 0;
          childrenRenderers[i].material.color = new Color(r, g, b, a);
        }
      }
      return;
    }

    if (isMoving) {
      // Do nothing when boss is moving
      return;
    }

    if (npcState.state == NPCState.STARING) {
      if (lookAtAndCheckIfSeenPlayer()) {
        if (isVisible()) {
          turnToTracingState();
          return;
        }
        return;
      } else {
        npcState.state = NPCState.MAKING_DECISION;
      }
      return;
    }

    if (npcState.state == NPCState.TRACING) {
      if (!lookAtAndCheckIfSeenPlayer()) {
        audio.Stop();
        npcState.state = NPCState.MAKING_DECISION;
        return;
      }

      if (playerIsInTheRadius(attackingRadius)) {
        turnToAttackingState();
        return;
      } else if (playerIsInTheRadius(staringTriggerRadius)) {
        if (isVisible()) {
          // Restore the mentality of the player
          playerMentality.gain(mentalityRestorePerSecond * Time.deltaTime);
        }
      }

      path = maze.getSimpleShortestPath(transform.position, player.transform.position);
      if (path.Count > 0) {
        // Do the first moving instruction
        StartCoroutine(move(path[0]));
      }

      return;
    }

    if (npcState.state == NPCState.ATTACKING) {

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
          scoreboard.addScore((int)(250 * (QTELength * (1 + bonusScale))));
        }
      }

      return;
    }

    if (lookAtAndCheckIfSeenPlayer()) {
      turnToStaringState();
      return;
    }
    
    if (npcState.state == NPCState.WANDERING) {
      if (pathIndex >= path.Count) { // Finish
        npcState.state = NPCState.MAKING_DECISION;
        return;
      }
      // Do the next moving instruction
      StartCoroutine(move(path[pathIndex++]));
    }

    if (npcState.state == NPCState.CHASING) {
      path = maze.getSimpleShortestPath(transform.position, player.transform.position);
      if (path.Count > 0) {
        // Do the first moving instruction
         StartCoroutine(move(path[0]));
      } else {
        npcState.state = NPCState.MAKING_DECISION;
        return;
      }
    }
    
    if (npcState.state == NPCState.MAKING_DECISION) {
      makeDecision();
    }

  }

  void OnGUI () {
    
    if (GameState.state != GameState.PLAYING) {
      return;
    }

    if (npcState.state == NPCState.ATTACKING) {

      Color originalColor = GUI.color;
      GUI.color = new Color(1, 1, 1, maskAlpha);
      GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), attackingMaskTexture);
      GUI.color = originalColor;
      
      if (isCameraMoving) {
        return;
      }

      int fakeBossWidth = Screen.height;
      int fakeBossHeight = Screen.height;
      GUI.DrawTexture(new Rect((Screen.width - fakeBossWidth) / 2, (Screen.height - fakeBossHeight) / 2, fakeBossWidth, fakeBossHeight), fakeBossTextures[fakeBossTextureIndex]);

      GUI.depth = 1;

      if (QTEvent.Count > 0) {
        int QTEArrowSize = Screen.height / 8;
        if (QTEsmall) {
          QTEArrowSize = Screen.height / 10;
        }
        Texture QTEArrowTexture = QTEArrowTextures[QTEvent[0]];
        GUI.DrawTexture(new Rect((Screen.width - QTEArrowSize) / 2, Screen.height / 5, QTEArrowSize, QTEArrowSize), QTEArrowTexture);

        QTEArrowSize = Screen.height / 10;
        
        float timePerArrow = QTETimeLimitPerKey - 0.5f;
        if (timePerArrow < 0.3f) {
          timePerArrow = 0.3f;
        }
        
        int arrowsToShow = (int)(QTETimeUsed / timePerArrow) + 1;
        if (arrowsToShow > QTELength) {
          arrowsToShow = QTELength;
        }

        for (int i = 1; (i + QTELength - arrowsToShow) < QTEvent.Count; i++) {
          QTEArrowTexture = QTEArrowTextures[QTEvent[i]];
          GUI.DrawTexture(new Rect((Screen.width - QTEArrowSize) / 2 + i * QTEArrowSize, Screen.height / 5, QTEArrowSize, QTEArrowSize), QTEArrowTexture);
        }

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
    if (QTETimeLimitPerKey < minQTETimeLimitPerKey) {
      QTETimeLimitPerKey = minQTETimeLimitPerKey;
    }
  }

  private void lookAtPlayer () {
    Vector3 targetPosition = player.transform.position;
    targetPosition.y = transform.position.y;
    transform.LookAt(targetPosition);
  }

  private bool hasSeenPlayer () {
    if (player.layer == LayerMask.NameToLayer("Invisible")) {
      return false;
    }
    RaycastHit hit;
    if (Physics.Linecast(transform.position, player.transform.position, out hit)) {
      if (hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.name == "playerBody") {
        return true;
      }
    }
    return false;
  }

  private bool lookAtAndCheckIfSeenPlayer () {
    if (playerIsInTheRadius(staringTriggerRadius)) {
      lookAtPlayer();
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
  }

  private bool playerIsInTheRadius (float radius, bool checkY = false) {
    if (checkY) {
      return Vector3.Distance(transform.position, player.transform.position) <= radius;
    }
    if (maze.getFloor(transform.position.y) != maze.getFloor(player.transform.position.y)) {
      return false;
    }
    float dx = transform.position.x - player.transform.position.x;
    float dz = transform.position.z - player.transform.position.z;
    float distance = Mathf.Sqrt(dx * dx + dz * dz);
    return distance <= radius;
  }

  private IEnumerator move (Vector3 movingVector) {
    
    startMovingPosition = transform.position;
    Vector3 targetPosition = transform.position + movingVector;
    
    isMoving = true;

    float percent = 0;
 
    while (percent < 1 && isMoving) {

      if (npcState.state == NPCState.STARING || npcState.state == NPCState.TRACING) {
        lookAtPlayer();
      }

      movingSpeed = movingSpeed + Time.deltaTime * acceleration;
      percent += Time.deltaTime * movingSpeed / movingVector.magnitude;
      transform.position = Vector3.Lerp(startMovingPosition, targetPosition, percent);
      yield return null;
    }

    isMoving = false;

    yield return 0;
  }

  private void stopMoving () {
    if (isMoving) {
      isMoving = false;
      transform.position = startMovingPosition;
    }
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

      npcState.state = NPCState.WANDERING;
    } else {
      npcState.state = NPCState.CHASING;
    }
  }

  private void turnToStaringState () {
    npcState.state = NPCState.STARING;
  }

  private void turnToTracingState () {
    playAudio(tracingSound);
    npcState.state = NPCState.TRACING;
  }

  private void turnToAttackingState () {
    if (!audio.isPlaying) {
      playAudio(tracingSound);
    }
    npcState.state = NPCState.ATTACKING;
    playerCharacterMotor.canControl = false;
    playerMouseLook.enabled = false;
    isCameraMoving = true;
    cameraMovedTime = 0;
    cameraQuaternion = Quaternion.LookRotation(lookAtPoint.transform.position - player.transform.position);
    // Generate the first event
    perfectQTE = true;
    QTEvent = generateQTE(QTELength);
    QTETimeLimit = QTETimeLimitPerKey * QTELength;
    QTETimeUsed = 0;
  }

  private void turnToStunningState () {
    audio.Stop();
    npcState.state = NPCState.STUNNING;
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
