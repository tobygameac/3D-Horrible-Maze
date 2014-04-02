using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Vitality))]

public class Spy : MonoBehaviour {

  private Vitality vitality;
  private Camera bossCamera;

  public float vitalityCost = 50;
  public float spyingTime = 10;

  private float spiedTime;
  private bool isSpying;

  void Start () {
    vitality = GetComponent<Vitality>();
    bossCamera = GameObject.FindGameObjectWithTag("BossCamera").camera;
    isSpying = false;
    GameObject.FindWithTag("Main").GetComponent<SkillMenu>().unlockSkill(3);
  }

  void Update () {
    
    if (isSpying) {
      spiedTime += Time.deltaTime;
      if (spiedTime >= spyingTime) {
        bossCamera.enabled = false;
        isSpying = false;
      }
    }

    if (Input.GetKey(KeyCode.Keypad3) || Input.GetKey(KeyCode.Alpha3)) {
      if (!isSpying) {
        if (vitality.enough(vitalityCost)) {
          vitality.use(vitalityCost);
          isSpying = true;
          spiedTime = 0;
          bossCamera.enabled = true;
        }
      }
    }

  }

  void OnGUI () {
    GUI.Label(new Rect(10, 120, 200, 20), "3 : Spy");
  }
}
