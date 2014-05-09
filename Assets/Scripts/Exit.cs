using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class Exit : MonoBehaviour {

  private Boss boss;

  private GameObject[] elevators;

  private SoundEffectManager soundEffectManager;

  private CharacterMotor playerCharacterMotor;
  private MouseLook2 playerMouseLook;

  private bool firstFound;

  void Start () {
    boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();

    elevators = GameObject.FindGameObjectsWithTag("Elevator");

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    playerCharacterMotor = GameObject.FindWithTag("Player").GetComponent<CharacterMotor>();
    playerMouseLook = GameObject.FindWithTag("Player").GetComponent<MouseLook2>();

    firstFound = false;

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
      if (boss.isInsaneMode()) {
        if (GameState.state != GameState.FINISHED) {
          GameState.state = GameState.FINISHED;
          GameState.win = true;
          playerCharacterMotor.enabled = false;
          playerMouseLook.enabled = false;
        }
      } else {
        if (!firstFound) {
          firstFound = true;
          TargetMenu.addTarget("Find the elevator.");
          startAllElevator();
        }
      }
    }
  }

}
