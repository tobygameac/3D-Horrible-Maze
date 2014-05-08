using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class Heartbeat : MonoBehaviour {

  public AudioClip[] heartbeatSounds;
  public float heartbeatStartPercent;

  private Mentality mentality;

  private float frame;

  void Start () {
    mentality = GameObject.FindWithTag("Player").GetComponent<Mentality>();
    if (heartbeatSounds.Length > 0) {
      frame = heartbeatStartPercent / heartbeatSounds.Length;
    }
  }

  void Update () {
    float percent = mentality.getMentalityPointPercent();
    int type = -1;
    for (int i = 0; i < heartbeatSounds.Length; i++) {
      if (percent <= heartbeatStartPercent - (i * frame)) {
        type = i;
      } else {
        break;
      }
    }
    if (type == -1) {
      audio.Stop();
    } else {
      audio.clip = heartbeatSounds[type];
      if (!audio.isPlaying) {
        audio.Play();
      }
    }
  }

}
