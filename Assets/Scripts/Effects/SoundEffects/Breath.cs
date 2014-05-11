using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class Breath : MonoBehaviour {

  public AudioClip[] breathSounds;
  public float breathStartPercent;

  private Vitality vitality;

  private float frame;

  void Start () {
    vitality = GameObject.FindWithTag("Player").GetComponent<Vitality>();
    if (breathSounds.Length > 0) {
      frame = breathStartPercent / breathSounds.Length;
    }
  }

  void Update () {
    float percent = vitality.getVitalityPointPercent();
    int type = -1;
    for (int i = 0; i < breathSounds.Length; i++) {
      if (percent <= breathStartPercent - (i * frame)) {
        type = i;
      } else {
        break;
      }
    }
    if (type == -1) {
      audio.Stop();
    } else {
      audio.clip = breathSounds[type];
      if (!audio.isPlaying) {
        audio.Play();
      }
    }
  }

}
