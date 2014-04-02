using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterMotor))]
[RequireComponent (typeof(Vitality))]
[RequireComponent (typeof(Footstep))]

public class Sprint : MonoBehaviour {

  private CharacterMotor characterMotor;
  private Vitality vitality;
  private Footstep footstep;

  private float originalForwardSpeed;
  private float originalSidewaysSpeed;
  private float originalBackwardsSpeed;
  private float sprintForwardSpeed;
  private float sprintSidewaysSpeed;
  private float sprintBackwardsSpeed;

  public float timesOfSpeed = 1.5f;
  public float vitalityCostPerSecond = 10.0f;

  private bool isTired = false;
  public float restTime = 10.0f;
  private float restedTime;

  void Start () {
    characterMotor = GetComponent<CharacterMotor>();
    vitality = GetComponent<Vitality>();
    footstep = GetComponent<Footstep>();

    originalForwardSpeed = characterMotor.movement.maxForwardSpeed;
    originalSidewaysSpeed = characterMotor.movement.maxSidewaysSpeed;
    originalBackwardsSpeed = characterMotor.movement.maxBackwardsSpeed;
    sprintForwardSpeed = originalForwardSpeed * timesOfSpeed;
    sprintSidewaysSpeed = originalSidewaysSpeed * timesOfSpeed;
    sprintBackwardsSpeed = originalBackwardsSpeed * timesOfSpeed;

    GameObject.FindWithTag("Main").GetComponent<SkillMenu>().unlockSkill(0);
  }

  void Update () {
    
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

  void OnGUI () {
    GUI.Label(new Rect(10, 60, 200, 20), "shift : Sprint");
  }
}
