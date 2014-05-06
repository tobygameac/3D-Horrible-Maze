using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class Key : MonoBehaviour {

  private Boss boss;

  private SoundEffectManager soundEffectManager;

  void Start () {

    boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

  }
	
  void OnTriggerEnter (Collider other) {
    if (other.tag == "Player") {
      soundEffectManager.playMentalityPotionSound();
      boss.becomeInsaneMode();
      Destroy(gameObject);
    }
  }
}
