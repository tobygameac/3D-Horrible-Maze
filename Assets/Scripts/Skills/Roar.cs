using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Vitality))]
[RequireComponent (typeof(Mentality))]

public class Roar : MonoBehaviour {

  public float vitalityCost = 50;
  public float mentalityGain = 25;

  private SkillMenu skillMenu;

  private SoundEffectManager soundEffectManager;

  private Vitality vitality;
  private Mentality mentality;

  void Start () {
    vitality = GetComponent<Vitality>();
    mentality = GetComponent<Mentality>();

    skillMenu = GameObject.FindWithTag("Main").GetComponent<SkillMenu>();
    skillMenu.unlockSkill(1);
    skillMenu.setSkillMessage(1, "Roar");

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
  }

  void Update () {
    if (GameState.state != GameState.PLAYING) {
      return;
    }

    if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1)) {
      if (vitality.enough(vitalityCost)) {
        soundEffectManager.playRoarSound();
        vitality.use(vitalityCost);
        mentality.gain(mentalityGain);
      } else {
        // soundEffectManager.playErrorSound();
        // MessageViewer.showErrorMessage("Not enough vitality");
      }
    }
  }

}
