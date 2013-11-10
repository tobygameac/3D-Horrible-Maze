using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class Item : MonoBehaviour {

  public string itemName;
  public string detail;
  public string skillName;

  private MessageViewer messageViewer;

  void Start () {
    messageViewer = GameObject.FindWithTag("Main").GetComponent<MessageViewer>();
  }

  void OnTriggerStay (Collider other) {
    if (other.tag == "Player") {
      messageViewer.viewMessage("Press Z to pick up " + itemName + ".", 0.1f);
      if (Input.GetKey(KeyCode.Z)) {
        if (skillName != null && skillName != "") {
          other.gameObject.AddComponent(skillName);
        }
        Destroy(gameObject);
      }
    }
  }
}
