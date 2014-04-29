using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

  private static System.Random random = new System.Random(); // Only need one random seed

  public int baseScoreGain;
  private int scoreGain;
  public int additionEventsFrame;
  public int[] upgradeFrames;
  public float[] upgradeProbabilityPerFrame;
  public float[] upgradeScales;

  public Material baseMaterial;
  public Material[] upgradeMaterials;

  private MazeGenerator maze;

  private SoundEffectManager soundEffectManager;

  private Scoreboard scoreboard;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
      
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    scoreboard = GameObject.FindWithTag("Main").GetComponent<Scoreboard>();

    upgrade();
  }

  void OnTriggerEnter (Collider other) {
    if (other.tag == "Player") {
      soundEffectManager.playScoreSound();
      scoreboard.addScore(scoreGain);
      upgrade();
      Vector3 randomEventPosition = maze.getNewEventPosition(transform.position);
      transform.position = randomEventPosition;
      if (scoreboard.getScore() >= additionEventsFrame) {
        maze.allocateRandomEvent(1);
      }
    }
  }

  void upgrade () {
    scoreGain = baseScoreGain;
    renderer.material = baseMaterial;
    for (int i = 0; i < upgradeFrames.Length; i++) {
      if (scoreboard.getScore() < upgradeFrames[i]) {
        break;
      }
      if (random.NextDouble() < upgradeProbabilityPerFrame[i]) {
        renderer.material = upgradeMaterials[i];
        scoreGain = (int)(baseScoreGain * upgradeScales[i]);
        break;
      }
    }
  }

}
