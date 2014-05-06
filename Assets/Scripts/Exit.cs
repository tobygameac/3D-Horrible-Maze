using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class Exit : MonoBehaviour {

  private Boss boss;

  private SoundEffectManager soundEffectManager;

  private CharacterMotor playerCharacterMotor;
  private MouseLook2 playerMouseLook;

  void Start () {
    boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    playerCharacterMotor = GameObject.FindWithTag("Player").GetComponent<CharacterMotor>();
    playerMouseLook = GameObject.FindWithTag("Player").GetComponent<MouseLook2>();
  }
	
  void OnTriggerStay (Collider other) {
    if (other.tag == "Player") {
      if (boss.isInsaneMode()) {
        if (GameState.state != GameState.LOSING) {
          GameState.state = GameState.LOSING;
          playerCharacterMotor.enabled = false;
          playerMouseLook.enabled = false;
        }
      } else {
        MessageViewer.showMessage("Find the key", 0.5f);
      }
    }
  }

}
