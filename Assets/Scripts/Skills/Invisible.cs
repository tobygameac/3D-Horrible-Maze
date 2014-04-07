using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Vitality))]

public class Invisible : MonoBehaviour {

  private Vitality vitality;

  public float vitalityCostPerSecond = 10;

  private bool isInvisible;
  
  private SkillMenu skillMenu;
  
  private SoundEffectManager soundEffectManager;

  void Start () {
    vitality = GetComponent<Vitality>();

    isInvisible = false;

    skillMenu = GameObject.FindWithTag("Main").GetComponent<SkillMenu>();
    skillMenu.unlockSkill(2);
    skillMenu.setSkillMessage(2, "hide~~~~~~");

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
  }

  void Update () {
    if (Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.Alpha2)) {
      isInvisible = true;
    }
    if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
      isInvisible = false;
    }

    if (isInvisible) {
      float need = vitalityCostPerSecond * Time.deltaTime;
      if (vitality.enough(need)) {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        vitality.use(need);
      } else {
        gameObject.layer = LayerMask.NameToLayer("Default");
        isInvisible = false;
      }
    } else {
      gameObject.layer = LayerMask.NameToLayer("Default");
    }
  }

}
