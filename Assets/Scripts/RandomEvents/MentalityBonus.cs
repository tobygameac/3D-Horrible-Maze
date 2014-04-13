using UnityEngine;
using System.Collections;

public class MentalityBonus : MonoBehaviour {

  public float mentalityGain = 10;

  private SoundEffectManager soundEffectManager;

  private Mentality mentality;

  void OnTriggerEnter (Collider other) {
    if (other.tag == "Player") {
      mentality = GameObject.FindWithTag("Player").GetComponent<Mentality>();
      
      soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
      
      mentality.gain(mentalityGain);
      Destroy(gameObject);
    }
  }

}
