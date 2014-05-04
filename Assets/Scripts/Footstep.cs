using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterMotor))]

public class Footstep : MonoBehaviour {

  public bool isRunning = false;

  public AudioClip walkingSound;
  public AudioClip runningSound;

  private CharacterMotor characterMotor;

  void Start () {
    characterMotor = GetComponent<CharacterMotor>();
  }

  void Update () {
    if (GameState.state != GameState.PLAYING) {
      if (audio.isPlaying) {
        audio.Stop();
      }
      return;
    }

    if (isRunning) {
      audio.clip = runningSound;
    } else {
      audio.clip = walkingSound;
    }
    float deltaH = Input.GetAxis("Horizontal");
    float deltaV = Input.GetAxis("Vertical");
    if ((deltaH != 0 || deltaV != 0) && characterMotor.enabled && !characterMotor.IsJumping()) {
       if (!audio.isPlaying) {
        audio.Play();
       }
    } else {
      if (audio.isPlaying) {
        audio.Stop();
      }
    }
  }

}
