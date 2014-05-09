using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

  // True if the elevator is is up, false if elevator is down
  private bool isUp = true;

  // True if the elevator is moving
  private bool isMoving = false;

  public float waitingTime = 3;
  private float waitedTime = 0;

  public float movingTime = 5;
  private float movedTime = 0;

  private float movingDistance = 5;

  // Position
  private Vector3 startPosition, endPosition;

  private static bool firstFound;

  public AudioClip moreHorrorTheme;
  private AudioSource mainAudioSource;

  void Start () {
    mainAudioSource = GameObject.FindWithTag("Main").GetComponent<AudioSource>();

    startPosition = transform.position;
    endPosition = startPosition + Vector3.up * movingDistance;
    
    firstFound = false;
  }

  void Update () {
    if (isMoving) {
     if (!audio.isPlaying) {
      audio.Play();
     }
      movedTime += Time.deltaTime;
      if (movedTime >= movingTime) {
        // Change moving direction
        isUp = !isUp;

        // Change moving state
        isMoving = false;

        waitedTime = 0;
      } else {
        if (isUp) {
          transform.position = Vector3.Lerp(startPosition, endPosition, movedTime / movingTime);
        } else {
          transform.position = Vector3.Lerp(endPosition, startPosition, movedTime / movingTime);
        }
      }

    } else { // Waiting
     if (audio.isPlaying) {
      audio.Stop();
     }
      waitedTime += Time.deltaTime;
      if (waitedTime >= waitingTime) {
        // Change moving state
        isMoving = true;

        movedTime = 0;
      }
    }
  }

  void OnTriggerEnter (Collider other) {
    if (GameMode.mode != GameMode.ESCAPING) {
      return;
    }
    if (other.tag == "Player") {
      if (!firstFound) {
        firstFound = true;
        TargetMenu.addTarget("Find the key in the maze.");
        mainAudioSource.audio.Stop();
        mainAudioSource.audio.clip = moreHorrorTheme;
        mainAudioSource.audio.Play();
      }
    }
  }

  public void setMovingDistance (float distance) {
    movingDistance = distance;
    endPosition = startPosition + Vector3.up * movingDistance;
  }

}
