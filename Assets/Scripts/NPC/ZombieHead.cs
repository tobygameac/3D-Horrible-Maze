using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class ZombieHead : MonoBehaviour {

  private MazeGenerator maze;

  private GameObject player;

  private SoundEffectManager soundEffectManager;

  private Vector3 startMovingPosition;
  private Vector3 targetPosition;
  private bool foundPlayer;
  public float tracingTime;
  private float tracedTime;
  public float diedRadius;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();

    player = GameObject.FindWithTag("Player");
      
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
  }

  void Update () {
    if (!foundPlayer) {
      return;
    }

    tracedTime += Time.deltaTime;
    transform.LookAt(Camera.main.transform);
    transform.position = Vector3.Lerp(startMovingPosition, targetPosition, tracedTime / tracingTime);

    if (tracedTime >= tracingTime || Vector3.Distance(transform.position, player.transform.position) <= diedRadius) {
      soundEffectManager.playBloodHitSound();
      Destroy(gameObject);
    }
  }

  void OnTriggerEnter (Collider other) {
    if (foundPlayer) {
      return;
    }

    if (other.tag == "Player") {
      if (maze.getFloor(transform.position) == maze.getFloor(other.transform.position)) {
        soundEffectManager.playZombieGaspSound();
        foundPlayer = true;
        tracedTime = 0;
        startMovingPosition = transform.position;
        targetPosition = other.transform.position;
      }
    }
  }
}
