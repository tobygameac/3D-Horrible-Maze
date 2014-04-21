using UnityEngine;
using System.Collections;

public class SlowingTrap : MonoBehaviour {

  public Texture slowingTrapMaskTexture;
  private float maskAlpha;

  private float originalForwardSpeed;
  private float originalSidewaysSpeed;
  private float originalBackwardsSpeed;
  private float slowingForwardSpeed;
  private float slowingSidewaysSpeed;
  private float slowingBackwardsSpeed;

  public float timesOfSpeed = 0.25f;
  public float slowingTime = 5.0f;
  private float slowedTime;
  private bool isSlowing = false;

  private MazeGenerator maze;

  private SoundEffectManager soundEffectManager;

  private Sprint sprint;

  private CharacterMotor characterMotor;

  private Renderer[] childrenRenderers;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
      
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
    
    childrenRenderers = GetComponentsInChildren<Renderer>();

    sprint = null;
    characterMotor = null;
  }

  void Update () {
    if (isSlowing) {
      characterMotor.movement.maxForwardSpeed = slowingForwardSpeed;
      characterMotor.movement.maxSidewaysSpeed = slowingSidewaysSpeed;
      characterMotor.movement.maxBackwardsSpeed = slowingBackwardsSpeed;
      int ms = (int)((Time.time * 100) % 100);
      if (ms < 50) {
        maskAlpha = (ms * 2) / 100.0f;
      } else {
        maskAlpha = ((100 - ms) * 2) / 100.0f;
      }
      slowedTime += Time.deltaTime;
      if (slowedTime >= slowingTime) {
        if (sprint) {
          sprint.enabled = true;
        }
        isSlowing = false;
        characterMotor.movement.maxForwardSpeed = originalForwardSpeed;
        characterMotor.movement.maxSidewaysSpeed = originalSidewaysSpeed;
        characterMotor.movement.maxBackwardsSpeed = originalBackwardsSpeed;
        Vector3 randomEventPosition = maze.getNewEventPosition();
        transform.position = randomEventPosition;
      }
      return;
    }
  }

  void OnGUI () {
    if (isSlowing) {
      Color originalColor = GUI.color;
      GUI.color = new Color(1, 1, 1, maskAlpha);
      GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), slowingTrapMaskTexture);
      GUI.color = originalColor;
      return;
    }
  }

  void OnTriggerEnter (Collider other) {
    if (isSlowing) {
      return;
    }

    if (other.tag == "Player") {
      soundEffectManager.playTrapSound();
      if (!sprint) {
        sprint = other.GetComponent<Sprint>();
      }
      if (sprint) {
        sprint.enabled = false;
      }
      if (!characterMotor) {
        characterMotor = other.GetComponent<CharacterMotor>();
        originalForwardSpeed = characterMotor.movement.maxForwardSpeed;
        originalSidewaysSpeed = characterMotor.movement.maxSidewaysSpeed;
        originalBackwardsSpeed = characterMotor.movement.maxBackwardsSpeed;
        slowingForwardSpeed = originalForwardSpeed * timesOfSpeed;
        slowingSidewaysSpeed = originalSidewaysSpeed * timesOfSpeed;
        slowingBackwardsSpeed = originalBackwardsSpeed * timesOfSpeed;
      }
      for (int i = 0; i < childrenRenderers.Length; i++) {
        childrenRenderers[i].enabled = false;
      }
      isSlowing = true;
      slowedTime = 0;
    }
  }

}
