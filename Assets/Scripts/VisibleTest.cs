using UnityEngine;
using System.Collections;

public class VisibleTest : MonoBehaviour {

  private GameObject player;

  public float offScreenDot = 0.5f;

  void Start () {
    player = GameObject.FindWithTag("Player");
  }

  void Update () {
   isVisible(); 
  }

  bool isVisible () {
    Vector3 fwd = player.transform.forward;
    Vector3 other = (transform.position - player.transform.position).normalized;
    
    float dotProduct = Vector3.Dot(fwd, other);
    
    if (dotProduct > offScreenDot) {
      RaycastHit hit;
      if (Physics.Linecast(transform.position, player.transform.position, out hit)) {
        if (hit.collider.gameObject.name == "body") {
          print("visible");
          return true;
        }
      }
      
    }
    print("Not visible");

    return false;
  }
}
