using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class ZombieHead : MonoBehaviour {

  public float offScreenDot;

  private bool foundPlayer;
  public float tracingTime;
  private float tracedTime;
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
  }

  void Update () {
    if (!foundPlayer) {
      return;
    }

    tracedTime += Time.deltaTime;
    transform.LookAt(Camera.main.transform);
    transform.position = Vector3.Lerp(startMovingPosition, targetPosition, tracedTime / tracingTime);

    if (tracedTime >= tracingTime || Vector3.Distance(transform.position, player.transform.position) <= diedRadius) {
      bloodSplatter.addBlood();
      Destroy(gameObject);
    }
  }

  void OnTriggerStay (Collider other) {
    if (foundPlayer) {
      return;
    }

    if (other.tag == "Player") {
      if (maze.getFloor(transform.position) == maze.getFloor(other.transform.position)) {
        if (isVisible()) {
          soundEffectManager.playZombieGaspSound();
          foundPlayer = true;
          tracedTime = 0;
          startMovingPosition = transform.position;
          targetPosition = other.transform.position;
        }
      }
    }
  }

  private bool isVisible () {
    Vector3 fwd = player.transform.forward;
    Vector3 other = (transform.position - player.transform.position).normalized;
    
    float dotProduct = Vector3.Dot(fwd, other);
    
    if (dotProduct > offScreenDot) {
      if (player.layer == LayerMask.NameToLayer("Invisible")) {
        return false;
      }
      RaycastHit hit;
      if (Physics.Linecast(transform.position, player.transform.position, out hit)) {
        if (hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.name == "playerBody") {
          return true;
        }
      }
    }

    return false;
  }
}
