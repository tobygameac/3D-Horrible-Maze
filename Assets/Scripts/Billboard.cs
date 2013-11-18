using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

  private GameObject player;

  void Start () {
    player = GameObject.FindWithTag("Player");
  }
  
  void Update () {
    transform.LookAt(player.transform);
  }
}
