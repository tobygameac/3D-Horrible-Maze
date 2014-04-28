using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterMotor))]
[RequireComponent (typeof(Mentality))]

public class DebuffManager : MonoBehaviour {

  public float timesOfSpeed = 0.25f;
  public float timesOfMaxMentality = 5.0f;

  public Texture slowingTrapMaskTexture;
  private float maskAlpha;

  private float originalForwardSpeed;
  private float originalSidewaysSpeed;
  private float originalBackwardsSpeed;
  private float slowingForwardSpeed;
  private float slowingSidewaysSpeed;
  private float slowingBackwardsSpeed;

  private float originalMaxMentalityPoint;
  private float blindMaxMentalityPoint;

  private float slowingTime;
  private float blindTime;

  private Sprint sprint;

  private CharacterMotor characterMotor;

  private Mentality mentality;

  void Start () {
    slowingTime = 0;
    blindTime = 0;

    characterMotor = GetComponent<CharacterMotor>();
    mentality = GetComponent<Mentality>();

    originalForwardSpeed = characterMotor.movement.maxForwardSpeed;
    originalSidewaysSpeed = characterMotor.movement.maxSidewaysSpeed;
    originalBackwardsSpeed = characterMotor.movement.maxBackwardsSpeed;
    slowingForwardSpeed = originalForwardSpeed * timesOfSpeed;
    slowingSidewaysSpeed = originalSidewaysSpeed * timesOfSpeed;
    slowingBackwardsSpeed = originalBackwardsSpeed * timesOfSpeed;

    originalMaxMentalityPoint = mentality.getMaxMentalityPoint();
    blindMaxMentalityPoint = originalMaxMentalityPoint * timesOfMaxMentality;
  }
  
  void Update () {
    if (slowingTime > 0) {
      slowingTime -= Time.deltaTime;
      if (sprint) {
        sprint.enabled = false;
      }
      characterMotor.movement.maxForwardSpeed = slowingForwardSpeed;
      characterMotor.movement.maxSidewaysSpeed = slowingSidewaysSpeed;
      characterMotor.movement.maxBackwardsSpeed = slowingBackwardsSpeed;
      int ms = (int)((Time.time * 100) % 100);
      if (ms < 50) {
        maskAlpha = (ms * 2) / 100.0f;
      } else {
        maskAlpha = ((100 - ms) * 2) / 100.0f;
      }
      if (slowingTime <= 0) {
        if (sprint) {
          sprint.enabled = true;
        }
        characterMotor.movement.maxForwardSpeed = originalForwardSpeed;
        characterMotor.movement.maxSidewaysSpeed = originalSidewaysSpeed;
        characterMotor.movement.maxBackwardsSpeed = originalBackwardsSpeed;
      }
    }

    if (blindTime > 0) {
      blindTime -= Time.deltaTime;
      mentality.setMaxMentalityPoint(blindMaxMentalityPoint);
      if (blindTime <= 0) {
        mentality.setMaxMentalityPoint(originalMaxMentalityPoint);
      }
    }
  }

  void OnGUI () {
    if (slowingTime > 0) {
      Color originalColor = GUI.color;
      GUI.color = new Color(1, 1, 1, maskAlpha);
      GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), slowingTrapMaskTexture);
      GUI.color = originalColor;
      return;
    }
  }

  public void addSlowingTime (float time) {
    slowingTime += time;
    if (!sprint) {
      sprint = GetComponent<Sprint>();
    }
  }

  public void addBlindTime (float time) {
    blindTime += time;
  }
}
