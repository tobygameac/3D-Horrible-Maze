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
    if (isRunning) {
      audio.clip = runningSound;
    } else {
      audio.clip = walkingSound;
    }
    float deltaH = Input.GetAxis("Horizontal");
    float deltaV = Input.GetAxis("Vertical");
    if (GameState.state == GameState.PLAYING && (deltaH != 0 || deltaV != 0) && characterMotor.enabled) {
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
