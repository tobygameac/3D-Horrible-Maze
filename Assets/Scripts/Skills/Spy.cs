using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Vitality))]

public class Spy : MonoBehaviour {

  public float vitalityCost = 50;
  public float spyingTime = 10;

  private float spiedTime;
  private bool isSpying = false;

  private Vitality vitality;
  private Camera bossCamera;

  private SkillMenu skillMenu;

  private SoundEffectManager soundEffectManager;

  void Start () {
    vitality = GetComponent<Vitality>();

    bossCamera = GameObject.FindGameObjectWithTag("BossCamera").camera;

    skillMenu = GameObject.FindWithTag("Main").GetComponent<SkillMenu>();
    skillMenu.unlockSkill(3);
    skillMenu.setSkillMessage(3, "I see u~~~~~~");

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
  }

  void Update () {  
    if (isSpying) {
      spiedTime += Time.deltaTime;
      if (spiedTime >= spyingTime) {
        bossCamera.enabled = false;
        isSpying = false;
      }
    }

    if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)) {
      if (!isSpying) {
        if (vitality.enough(vitalityCost)) {
          vitality.use(vitalityCost);
          isSpying = true;
          spiedTime = 0;
          bossCamera.enabled = true;
        } else {
          soundEffectManager.playErrorSound();
          MessageViewer.showErrorMessage("Not enough vitality");
        }
      }
    }
  }

}
