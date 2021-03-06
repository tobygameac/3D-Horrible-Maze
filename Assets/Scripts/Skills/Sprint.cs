﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterMotor))]
[RequireComponent (typeof(Vitality))]

public class Sprint : MonoBehaviour {

  public float originalForwardSpeed = 3;
  public float originalSidewaysSpeed = 2;
  public float originalBackwardsSpeed = 1;
  private float sprintForwardSpeed;
  private float sprintSidewaysSpeed;
  private float sprintBackwardsSpeed;

  public float timesOfSpeed = 1.5f;
  public float vitalityCostPerSecond = 10.0f;

  private bool isTired = false;
  public float restTime = 10.0f;
  private float restedTime;

  private CharacterMotor characterMotor;

  private Vitality vitality;

  private Footstep footstep;

  private SkillMenu skillMenu;

  private SoundEffectManager soundEffectManager;

  void Start () {
    characterMotor = GetComponent<CharacterMotor>();
    vitality = GetComponent<Vitality>();
    footstep = GetComponentInChildren<Footstep>();

    skillMenu = GameObject.FindWithTag("Main").GetComponent<SkillMenu>();
    skillMenu.unlockSkill(0);
    skillMenu.setSkillMessage(0, "Sprint");

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    sprintForwardSpeed = originalForwardSpeed * timesOfSpeed;
    sprintSidewaysSpeed = originalSidewaysSpeed * timesOfSpeed;
    sprintBackwardsSpeed = originalBackwardsSpeed * timesOfSpeed;
  }

  void Update () {
    if (GameState.state != GameState.PLAYING) {
      return;
    }
    
    if (isTired) {
      restedTime += Time.deltaTime;
      if (restedTime >= restTime) {
        isTired = false;
      }
      return;
    }

    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
      float need = vitalityCostPerSecond * Time.deltaTime;
      if (vitality.enough(need)) {
        footstep.isRunning = true;
        characterMotor.movement.maxForwardSpeed = sprintForwardSpeed;
        characterMotor.movement.maxSidewaysSpeed = sprintSidewaysSpeed;
        characterMotor.movement.maxBackwardsSpeed = sprintBackwardsSpeed;
        vitality.use(need);
      } else {
        if (!isTired) {
          // soundEffectManager.playErrorSound();
          // MessageViewer.showErrorMessage("Not enough vitality");
          isTired = true;
          restedTime = 0;
        }
        footstep.isRunning = false;
        characterMotor.movement.maxForwardSpeed = originalForwardSpeed;
        characterMotor.movement.maxSidewaysSpeed = originalSidewaysSpeed;
        characterMotor.movement.maxBackwardsSpeed = originalBackwardsSpeed;
      }
    } else {
      footstep.isRunning = false;
      characterMotor.movement.maxForwardSpeed = originalForwardSpeed;
      characterMotor.movement.maxSidewaysSpeed = originalSidewaysSpeed;
      characterMotor.movement.maxBackwardsSpeed = originalBackwardsSpeed;
    }

  }

}
