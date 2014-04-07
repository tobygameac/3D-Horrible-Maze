using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class SoundEffectManager : MonoBehaviour {

  public AudioClip buttonSound;
  public AudioClip errorSound;

  public void playButtonSound () {
    audio.PlayOneShot(buttonSound);
  }

  public void playErrorSound () {
    audio.PlayOneShot(errorSound);
  }

}
