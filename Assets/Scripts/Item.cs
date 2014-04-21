using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SphereCollider))]

public class Item : MonoBehaviour {

  public string itemName;
  public string detail;
  public string skillName;

  private SoundEffectManager soundEffectManager;

  void Start () {
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
  }

  void OnTriggerStay (Collider other) {
    if (other.tag == "Player") {
      MessageViewer.showMessage("Press Z to pick up " + itemName + ".", 0.1f);
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
}
