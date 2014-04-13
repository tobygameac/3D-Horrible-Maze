using UnityEngine;
using System.Collections;

public class VitalityBonus : MonoBehaviour {

  public float vitalityGain = 10;

  private SoundEffectManager soundEffectManager;

  private Vitality vitality;

  void OnTriggerEnter (Collider other) {
    if (other.tag == "Player") {
      vitality = GameObject.FindWithTag("Player").GetComponent<Vitality>();

      soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

      vitality.gain(vitalityGain);
      Destroy(gameObject);
    }
  }

}
