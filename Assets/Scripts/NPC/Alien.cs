using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class Alien : MonoBehaviour {

  private static System.Random random = new System.Random(); // Only need one random seed

  public float slowingTime = 2.5f;

  private bool foundPlayer;
  public float tracingTime;
  private float tracedTime;
  public float minimumAttackingRadius;
  public float maximumAttackingRadius;
  private float attackingRadius;
  public float diedRadius;

  private Vector3 startMovingPosition;
  private Vector3 targetPosition;

  private MazeGenerator maze;

  private GameObject player;

  private SoundEffectManager soundEffectManager;

  private BloodSplatter bloodSplatter;

  private DebuffManager debuffManager;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();

    player = GameObject.FindWithTag("Player");
      
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
    
    bloodSplatter = GameObject.FindWithTag("Main").GetComponent<BloodSplatter>();
    
    debuffManager = GameObject.FindWithTag("Player").GetComponent<DebuffManager>();

    attackingRadius = minimumAttackingRadius + (float)random.NextDouble() * (maximumAttackingRadius - minimumAttackingRadius);
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
      debuffManager.addSlowingTime(slowingTime);
      foundPlayer = false;
      Vector3 randomEventPosition = maze.getNewEventPosition(transform.position);
      transform.position = randomEventPosition;
      attackingRadius = minimumAttackingRadius + (float)random.NextDouble() * (maximumAttackingRadius - minimumAttackingRadius);
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
        soundEffectManager.playZombieGaspSound0();
        foundPlayer = true;
        tracedTime = 0;
        startMovingPosition = transform.position;
      }
    }
    return;
  }
}
