using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class ZombieHead : MonoBehaviour {

  private static System.Random random = new System.Random(); // Only need one random seed

  public float offScreenDot;

  private bool foundPlayer;
  public float tracingTime;
  private float tracedTime;
  public float maximumAttackingRadius;
  private float attackingRadius;
  public float diedRadius;

  private Vector3 startMovingPosition;
  private Vector3 targetPosition;

  private MazeGenerator maze;

  private GameObject player;

  private SoundEffectManager soundEffectManager;

  private BloodSplatter bloodSplatter;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();

    player = GameObject.FindWithTag("Player");
      
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
    
    bloodSplatter = GameObject.FindWithTag("Main").GetComponent<BloodSplatter>();
    
    attackingRadius = (float)random.NextDouble() * maximumAttackingRadius;
  }

  void Update () {
    if (!foundPlayer) {
      checkVisible();
      return;
    }

    tracedTime += Time.deltaTime;
    transform.LookAt(Camera.main.transform);
    targetPosition = Camera.main.transform.position;
    transform.position = Vector3.Lerp(startMovingPosition, targetPosition, tracedTime / tracingTime);

    if (tracedTime >= tracingTime || Vector3.Distance(transform.position, player.transform.position) <= diedRadius) {
      bloodSplatter.addBlood();
      Destroy(gameObject);
    }
  }

  private void checkVisible () {
    if (maze.getFloor(transform.position.y) != maze.getFloor(player.transform.position.y)) {
      return;
    }
    if (player.layer == LayerMask.NameToLayer("Invisible")) {
      return;
    }
    if (Vector3.Distance(transform.position, player.transform.position) > attackingRadius) {
      return;
    }
    RaycastHit hit;
    if (Physics.Raycast(player.transform.position, player.transform.forward, out hit)) {
      if (hit.transform == transform) {
        soundEffectManager.playZombieGaspSound();
        foundPlayer = true;
        tracedTime = 0;
        startMovingPosition = transform.position;
      }
    }
    return;
  }
}
