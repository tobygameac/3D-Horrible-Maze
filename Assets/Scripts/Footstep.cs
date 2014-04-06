using UnityEngine;
using System.Collections;

public class Footstep : MonoBehaviour {

  public bool isRunning = false;

  public AudioClip walkingSound;
  public AudioClip runningSound;

  void Update () {
    if (isRunning) {
      audio.clip = runningSound;
    } else {
      audio.clip = walkingSound;
    }
    float deltaH = Input.GetAxis("Horizontal");
    float deltaV = Input.GetAxis("Vertical");
    if (GameState.state == GameState.PLAYING && (deltaH != 0 || deltaV != 0)) {
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
