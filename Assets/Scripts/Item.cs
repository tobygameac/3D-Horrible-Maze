using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SphereCollider))]

public class Item : MonoBehaviour {

  public string itemName;
  public string detail;
  public string skillName;

  void OnTriggerStay (Collider other) {
    if (other.tag == "Player") {
      MessageViewer.showMessage("Press Z to pick up " + itemName + ".", 0.1f);
      if (Input.GetKey(KeyCode.Z)) {
        if (skillName != null && skillName != "") {
          other.gameObject.AddComponent(skillName);
        }
        Destroy(gameObject);
      }
    }
  }
}
