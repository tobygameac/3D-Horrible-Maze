using UnityEngine;
using System.Collections;

public class Lurker : MonoBehaviour {

  private Renderer[] childrenRenderers;

  private MazeGenerator maze;

  private GameObject player;

  public float showingRadius;

  public float risingTime;
  private float risedTime;

  public float risingY;
  private float originalY;

  private bool isHiding;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();

    childrenRenderers = GetComponentsInChildren<Renderer>();

    player = GameObject.FindWithTag("Player");

    originalY = transform.position.y;
    isHiding = false;
  }

  void Update () {
    risedTime += Time.deltaTime;
    
    if (risedTime > risingTime) {
      risedTime = risingTime;
    }

    bool hidingOriginal = isHiding;
    
    if (checkVisible()) {
      isHiding = false;
    } else {
      isHiding = true;
    }

    if (hidingOriginal != isHiding) {
      risedTime = 0;
    }

    float percent = (risedTime / risingTime);

    if (isHiding) {
      percent = 1 - percent;
    }

    transform.position = new Vector3(transform.position.x, originalY + risingY * percent, transform.position.z);

    for (int i = 0; i < childrenRenderers.Length; i++) {
      float r = childrenRenderers[i].material.color.r;
      float g = childrenRenderers[i].material.color.g;
      float b = childrenRenderers[i].material.color.b;
      childrenRenderers[i].material.color = new Color(r, g, b, percent);
    }
  }

  private bool checkVisible () {
    if (maze.getFloor(transform.position.y) != maze.getFloor(player.transform.position.y)) {
      return false;
    }
    if (player.layer == LayerMask.NameToLayer("Invisible")) {
      return false;
    }
    if (Vector3.Distance(transform.position, player.transform.position) > showingRadius) {
      return false;
    }
    return true;
  }
}
