using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class Footstep : MonoBehaviour {

  public bool isRunning = false;

  public AudioClip walkingSound;
  public AudioClip runningSound;
  public AudioClip jumpingSound;

  private CharacterMotor characterMotor;

  void Start () {
    characterMotor = GameObject.FindWithTag("Player").GetComponent<CharacterMotor>();
  }

  void Update () {
    if (GameState.state != GameState.PLAYING) {
      if (audio.isPlaying) {
        audio.Stop();
      }
      return;
    }
    if (characterMotor.IsJumping()) {
      audio.loop = false;
      audio.clip = jumpingSound;
    } else {
      audio.loop = true;
      if (isRunning) {
        audio.clip = runningSound;
      } else {
        audio.clip = walkingSound;
      }
    }
    float deltaH = Input.GetAxis("Horizontal");
    float deltaV = Input.GetAxis("Vertical");
    if ((deltaH != 0 || deltaV != 0 || characterMotor.IsJumping()) && characterMotor.enabled) {
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
