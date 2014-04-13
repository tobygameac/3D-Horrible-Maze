using UnityEngine;
using System.Collections;

public class SlowingTrap : MonoBehaviour {

  private float originalForwardSpeed;
  private float originalSidewaysSpeed;
  private float originalBackwardsSpeed;
  private float slowingForwardSpeed;
  private float slowingSidewaysSpeed;
  private float slowingBackwardsSpeed;

  public float timesOfSpeed = 0.25f;
  public float slowingTime = 5.0f;
  private float slowedTime;
  private bool isSlowing = false;

  private MazeGenerator maze;

  private SoundEffectManager soundEffectManager;

  private Sprint sprint;

  private CharacterMotor characterMotor;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
      
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
    
    sprint = null;
    characterMotor = null;
  }

  void Update () {
    if (isSlowing) {
      slowedTime += Time.deltaTime;
      if (slowedTime >= slowingTime) {
        sprint.enabled = true;
        isSlowing = false;
        characterMotor.movement.maxForwardSpeed = originalForwardSpeed;
        characterMotor.movement.maxSidewaysSpeed = originalSidewaysSpeed;
        characterMotor.movement.maxBackwardsSpeed = originalBackwardsSpeed;
        Vector3 randomEventPosition = maze.getNewEventPosition();
        transform.position = randomEventPosition;
      }
      return;
    }
  }

  void OnTriggerEnter (Collider other) {
    if (isSlowing) {
      return;
    }

    if (other.tag == "Player") {
      soundEffectManager.playTrapSound();
      if (!sprint) {
        sprint = other.GetComponent<Sprint>();
      }
      sprint.enabled = false;
      if (!characterMotor) {
        characterMotor = other.GetComponent<CharacterMotor>();
        originalForwardSpeed = characterMotor.movement.maxForwardSpeed;
        originalSidewaysSpeed = characterMotor.movement.maxSidewaysSpeed;
        originalBackwardsSpeed = characterMotor.movement.maxBackwardsSpeed;
        slowingForwardSpeed = originalForwardSpeed * timesOfSpeed;
        slowingSidewaysSpeed = originalSidewaysSpeed * timesOfSpeed;
        slowingBackwardsSpeed = originalBackwardsSpeed * timesOfSpeed;
      }
      renderer.enabled = false;
      isSlowing = true;
      slowedTime = 0;
      characterMotor.movement.maxForwardSpeed = slowingForwardSpeed;
      characterMotor.movement.maxSidewaysSpeed = slowingSidewaysSpeed;
      characterMotor.movement.maxBackwardsSpeed = slowingBackwardsSpeed;
    }
  }

}
