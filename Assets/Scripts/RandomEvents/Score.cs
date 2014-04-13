using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

  public int scoreGain = 1;

  private MazeGenerator maze;

  private SoundEffectManager soundEffectManager;

  private Scoreboard scoreboard;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
      
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    scoreboard = GameObject.FindWithTag("Main").GetComponent<Scoreboard>();
  }

  void OnTriggerEnter (Collider other) {
    if (other.tag == "Player") {
      soundEffectManager.playBonusSound();
      scoreboard.addScore(scoreGain);
      Vector3 randomEventPosition = maze.getNewEventPosition();
      transform.position = randomEventPosition;
    }
  }
}
