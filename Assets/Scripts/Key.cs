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
      TargetMenu.addTarget("Go back to the exit. RUN!!");
      boss.becomeInsaneMode();
      Destroy(gameObject);
    }
  }
}
