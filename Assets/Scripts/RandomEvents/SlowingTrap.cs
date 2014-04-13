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

  private Sprint sprint;

  private CharacterMotor characterMotor;

  private SoundEffectManager soundEffectManager;

  void Update () {
    if (isSlowing) {
      slowedTime += Time.deltaTime;
      if (slowedTime >= slowingTime) {
        characterMotor.movement.maxForwardSpeed = originalForwardSpeed;
        characterMotor.movement.maxSidewaysSpeed = originalSidewaysSpeed;
        characterMotor.movement.maxBackwardsSpeed = originalBackwardsSpeed;
        sprint.enabled = true;
        Destroy(gameObject);
      }
      return;
    }
  }

  void OnTriggerEnter (Collider other) {
    if (isSlowing) {
      return;
    }

    if (other.tag == "Player") {
      sprint = GameObject.FindWithTag("Player").GetComponent<Sprint>();
      sprint.enabled = false;

      characterMotor = GameObject.FindWithTag("Player").GetComponent<CharacterMotor>();

      soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

      soundEffectManager.playTrapSound();

      originalForwardSpeed = characterMotor.movement.maxForwardSpeed;
      originalSidewaysSpeed = characterMotor.movement.maxSidewaysSpeed;
      originalBackwardsSpeed = characterMotor.movement.maxBackwardsSpeed;
      slowingForwardSpeed = originalForwardSpeed * timesOfSpeed;
      slowingSidewaysSpeed = originalSidewaysSpeed * timesOfSpeed;
      slowingBackwardsSpeed = originalBackwardsSpeed * timesOfSpeed;
      renderer.enabled = false;
      isSlowing = true;
      slowedTime = 0;
      characterMotor.movement.maxForwardSpeed = slowingForwardSpeed;
      characterMotor.movement.maxSidewaysSpeed = slowingSidewaysSpeed;
      characterMotor.movement.maxBackwardsSpeed = slowingBackwardsSpeed;
    }
  }

}
