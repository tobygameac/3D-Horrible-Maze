using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class ZombieHead : MonoBehaviour {

  private static System.Random random = new System.Random(); // Only need one random seed

  private bool foundPlayer;
  public float tracingTime;
  private float tracedTime;
  public float maximumAttackingCountDown;
  private float attackingCountDown;
  public float maximumAttackingRadius;
  private float attackingRadius;
  public float diedRadius;

  private Vector3 startMovingPosition;
  private Vector3 targetPosition;

  private MazeGenerator maze;

  private GameObject player;

  private CameraShaker cameraShaker;

  private SoundEffectManager soundEffectManager;

  private BloodSplatter bloodSplatter;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();

    player = GameObject.FindWithTag("Player");
      
    cameraShaker = GameObject.FindWithTag("Main").GetComponent<CameraShaker>();

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
    
    bloodSplatter = GameObject.FindWithTag("Main").GetComponent<BloodSplatter>();
    
    attackingCountDown = (float)random.NextDouble() * maximumAttackingCountDown;
    attackingRadius = (float)random.NextDouble() * maximumAttackingRadius;
  }

  void Update () {
    if (!foundPlayer) {
      if (checkVisible()) {
        attackingCountDown -= Time.deltaTime;
      }
      return;
    }

    tracedTime += Time.deltaTime;
    transform.LookAt(Camera.main.transform);
    targetPosition = Camera.main.transform.position;
    transform.position = Vector3.Lerp(startMovingPosition, targetPosition, tracedTime / tracingTime);

    if (tracedTime >= tracingTime || Vector3.Distance(transform.position, player.transform.position) <= diedRadius) {
      StartCoroutine(cameraShaker.shakeCamera());
      bloodSplatter.addBlood();
      Destroy(gameObject);
    }
  }

  private bool checkVisible () {
    if (maze.getFloor(transform.position.y) != maze.getFloor(player.transform.position.y)) {
      return false;
    }
    if (player.layer == LayerMask.NameToLayer("Invisible")) {
      return false;
    }
    if (Vector3.Distance(transform.position, player.transform.position) > attackingRadius) {
      return false;
    }
    RaycastHit hit;
    if (Physics.Raycast(player.transform.position, player.transform.forward, out hit)) {
      if (hit.transform == transform) {
        if (attackingCountDown <= 0) {
          soundEffectManager.playZombieGaspSound1();
          foundPlayer = true;
          tracedTime = 0;
          startMovingPosition = transform.position;
        }
        return true;
      }
    }
    return false;
  }
}
