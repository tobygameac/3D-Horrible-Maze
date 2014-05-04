using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class MentalityBonus : MonoBehaviour {

  public float mentalityGain = 10;

  private MazeGenerator maze;

  private SoundEffectManager soundEffectManager;

  private Mentality mentality;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
      
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
    
    mentality = null;
  }

  void OnTriggerEnter (Collider other) {
    if (other.tag == "Player") {
      soundEffectManager.playMentalityPotionSound();
      if (!mentality) {
        mentality = other.GetComponent<Mentality>();
      }
      mentality.gain(mentalityGain);
      Vector3 randomEventPosition = maze.getNewEventPosition(transform.position);
      transform.position = randomEventPosition;
    }
  }

}
