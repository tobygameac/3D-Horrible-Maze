using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class SoundEffectManager : MonoBehaviour {

  public AudioClip buttonSound;
  public AudioClip errorSound;
  public AudioClip cryingSound;
  public AudioClip bonusSound;
  public AudioClip trapSound;

  public void playButtonSound () {
    audio.PlayOneShot(buttonSound);
  }

  public void playErrorSound () {
    audio.PlayOneShot(errorSound);
  }

  public void playCryingSound () {
    audio.PlayOneShot(cryingSound);
  }

  public void playBonusSound () {
    audio.PlayOneShot(bonusSound);
  }

  public void playTrapSound () {
    audio.PlayOneShot(trapSound);
  }

}
