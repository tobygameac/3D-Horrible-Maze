﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class VitalityBonus : MonoBehaviour {

  public float vitalityGain = 10;

  private MazeGenerator maze;

  private SoundEffectManager soundEffectManager;

  private Vitality vitality;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
      
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
    
    vitality = null;
  }

  void OnTriggerEnter (Collider other) {
    if (other.tag == "Player") {
      soundEffectManager.playVitalityPotionSound();
      if (!vitality) {
        vitality = other.GetComponent<Vitality>();
      }
      vitality.gain(vitalityGain);
      Vector3 randomEventPosition = maze.getNewEventPosition(transform.position);
      transform.position = randomEventPosition;
    }
  }

}
