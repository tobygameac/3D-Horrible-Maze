using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterMotor))]
[RequireComponent (typeof(Vitality))]

public class Sprint : MonoBehaviour {

  private CharacterMotor characterMotor;
  private Vitality vitality;

  private float originalForwardSpeed;
  private float originalSidewaysSpeed;
  private float originalBackwardsSpeed;
  private float sprintForwardSpeed;
  private float sprintSidewaysSpeed;
  private float sprintBackwardsSpeed;

  public float timesOfSpeed = 1.5f;
  public float vitalityPerSecond = 10.0f;

  void Start () {
    characterMotor = GetComponent<CharacterMotor>();
    vitality = GetComponent<Vitality>();

    originalForwardSpeed = characterMotor.movement.maxForwardSpeed;
    originalSidewaysSpeed = characterMotor.movement.maxSidewaysSpeed;
    originalBackwardsSpeed = characterMotor.movement.maxBackwardsSpeed;
    sprintForwardSpeed = originalForwardSpeed * timesOfSpeed;
    sprintSidewaysSpeed = originalSidewaysSpeed * timesOfSpeed;
    sprintBackwardsSpeed = originalBackwardsSpeed * timesOfSpeed;
  }

  void Update () {
    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
      float need = vitalityPerSecond * Time.deltaTime;
      if (vitality.enough(need)) {
        characterMotor.movement.maxForwardSpeed = sprintForwardSpeed;
        characterMotor.movement.maxSidewaysSpeed = sprintSidewaysSpeed;
        characterMotor.movement.maxBackwardsSpeed = sprintBackwardsSpeed;
        vitality.use(need);
      } else {
        characterMotor.movement.maxForwardSpeed = originalForwardSpeed;
        characterMotor.movement.maxSidewaysSpeed = originalSidewaysSpeed;
        characterMotor.movement.maxBackwardsSpeed = originalBackwardsSpeed;
      }
    } else {
      characterMotor.movement.maxForwardSpeed = originalForwardSpeed;
      characterMotor.movement.maxSidewaysSpeed = originalSidewaysSpeed;
      characterMotor.movement.maxBackwardsSpeed = originalBackwardsSpeed;
    }
  }
}
