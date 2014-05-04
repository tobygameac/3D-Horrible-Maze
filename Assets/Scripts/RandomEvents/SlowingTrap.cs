using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class SlowingTrap : MonoBehaviour {

  public float slowingTime = 2.5f;

  private MazeGenerator maze;

  private SoundEffectManager soundEffectManager;

  private DebuffManager debuffManager;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
      
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    debuffManager = GameObject.FindWithTag("Player").GetComponent<DebuffManager>();
  }

  void OnTriggerEnter (Collider other) {
    if (other.tag == "Player") {
      soundEffectManager.playTrapSound();
      debuffManager.addSlowingTime(slowingTime);
      Vector3 randomEventPosition = maze.getNewEventPosition(transform.position);
      transform.position = randomEventPosition;
    }
  }

}
