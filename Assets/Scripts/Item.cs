using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SphereCollider))]

public class Item : MonoBehaviour {

  public Texture pickUpButtonTexture;

  public string skillName;

  private SoundEffectManager soundEffectManager;

  private bool showPickUpButton;

  private float showedTime;
  private float showTime = 0.3f;
  private bool small;

  void Start () {
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
    showPickUpButton = false;
  }
  
  void OnGUI () {
    if (showPickUpButton) {
      showedTime += Time.deltaTime;
      if (showedTime > showTime) {
        small = !small;
        showedTime = 0;
      }
      float width = Screen.width / 15.0f;
      float height = width;
      if (small) {
        width *= 0.95f;
        height *= 0.95f;
      }
      float startX = (Screen.width - width) / 2.0f;
      float startY = Screen.height * 0.75f;
      GUI.DrawTexture(new Rect(startX, startY, width, height), pickUpButtonTexture);
    }
  }

  void OnTriggerStay (Collider other) {
    if (other.tag == "Player") {
      showPickUpButton = true;
      if (Input.GetKey(KeyCode.Z)) {
        soundEffectManager.playPickingSound();
        if (skillName != null && skillName != "") {
          if (skillName == "Invisible") {
            other.gameObject.GetComponent<Invisible>().enabled = true;
          } else if (other.gameObject.GetComponent(skillName) == null) {
            other.gameObject.AddComponent(skillName);
          }
        }
        Destroy(gameObject);
      }
    }
  }

  void OnTriggerExit (Collider other) {
    if (other.tag == "Player") {
      showPickUpButton = false;
    }
  }
}
