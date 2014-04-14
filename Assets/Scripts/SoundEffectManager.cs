using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
[RequireComponent (typeof(AudioListener))]

public class SoundEffectManager : MonoBehaviour {

  public AudioClip buttonSound;
  public AudioClip errorSound;
  public AudioClip cryingSound;
  public AudioClip tracingSound;
  public AudioClip bonusSound;
  public AudioClip trapSound;
  public AudioClip QTEHitSound;
  public AudioClip QTEMissSound;

  public void playButtonSound () {
    audio.PlayOneShot(buttonSound);
  }

  public void playErrorSound () {
    audio.PlayOneShot(errorSound);
  }

  public void playCryingSound () {
    audio.PlayOneShot(cryingSound);
  }

  public void playTracingSound () {
    audio.PlayOneShot(tracingSound);
  }

  public void playBonusSound () {
    audio.PlayOneShot(bonusSound);
  }

  public void playTrapSound () {
    audio.PlayOneShot(trapSound);
  }

  public void playQTEHitSound () {
    audio.PlayOneShot(QTEHitSound);
  }

  public void playQTEMissSound () {
    audio.PlayOneShot(QTEMissSound);
  }

}
