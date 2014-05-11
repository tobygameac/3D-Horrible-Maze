using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class Exit : MonoBehaviour {

  private GameObject[] elevators;

  private SoundEffectManager soundEffectManager;

  private CharacterMotor playerCharacterMotor;
  private MouseLook2 playerMouseLook;

  void Start () {
    elevators = GameObject.FindGameObjectsWithTag("Elevator");

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    playerCharacterMotor = GameObject.FindWithTag("Player").GetComponent<CharacterMotor>();
    playerMouseLook = GameObject.FindWithTag("Player").GetComponent<MouseLook2>();

    stopAllElevator();
  }

  void stopAllElevator () {
    for (int i = 0; i < elevators.Length; i++) {
      elevators[i].GetComponent<Elevator>().enabled = false;
      elevators[i].GetComponent<BoxCollider>().enabled = false;
    }
  }

  void startAllElevator () {
    for (int i = 0; i < elevators.Length; i++) {
      elevators[i].GetComponent<Elevator>().enabled = true;
      elevators[i].GetComponent<BoxCollider>().enabled = true;
    }
  }

  void OnTriggerStay (Collider other) {
    if (other.tag == "Player") {
      switch (EscapingState.state) {
       case EscapingState.BEGINNING:
        EscapingState.state = EscapingState.EXIT_FOUND;
        TargetMenu.addTarget("Find the elevator.");
        startAllElevator();
        break;
       case EscapingState.KEY_FOUND:
        if (GameState.state != GameState.FINISHED) {
          Destroy(gameObject);
          GameState.state = GameState.FINISHED;
          GameState.win = true;
          playerCharacterMotor.enabled = false;
          playerMouseLook.enabled = false;
        }
        break;
      }
    }
  }

}
