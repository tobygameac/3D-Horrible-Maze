using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
[RequireComponent (typeof(AudioListener))]

public class SoundEffectManager : MonoBehaviour {

  public AudioClip buttonSound;
  public AudioClip flipSound;
  public AudioClip errorSound;
  public AudioClip cryingSound;
  public AudioClip tracingSound;
  public AudioClip pickingSound;
  public AudioClip scoreSound;
  public AudioClip mentalityPotionSound;
  public AudioClip vitalityPotionSound;
  public AudioClip trapSound;
  public AudioClip QTEHitSound;
  public AudioClip QTEMissSound;
  public AudioClip roarSound;
  public AudioClip zombieGaspSound0;
  public AudioClip zombieGaspSound1;
  public AudioClip bloodHitSound;
  public AudioClip glassShatteredSound;

  public void adjustSound () {
    AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
    for (int i = 0; i < allAudioSources.Length; i++) {
      allAudioSources[i].volume = GameState.volume;
    }
  }

  public void playButtonSound () {
    audio.PlayOneShot(buttonSound);
  }

  public void playFlipSound () {
    audio.PlayOneShot(flipSound);
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

  public void playPickingSound () {
    audio.PlayOneShot(pickingSound);
  }

  public void playScoreSound () {
    audio.PlayOneShot(scoreSound);
  }

  public void playMentalityPotionSound () {
    audio.PlayOneShot(mentalityPotionSound);
  }

  public void playVitalityPotionSound () {
    audio.PlayOneShot(vitalityPotionSound);
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

  public void playRoarSound () {
    audio.PlayOneShot(roarSound);
  }

  public void playZombieGaspSound0 () {
    audio.PlayOneShot(zombieGaspSound0);
  }

  public void playZombieGaspSound1 () {
    audio.PlayOneShot(zombieGaspSound1);
  }

  public void playBloodHitSound () {
    audio.PlayOneShot(bloodHitSound);
  }
  
  public void playGlassShatteredSoundSound () {
    audio.PlayOneShot(glassShatteredSound);
  }

}
