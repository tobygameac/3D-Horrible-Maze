using UnityEngine;
using System.Collections;

public class Blood : MonoBehaviour {

  public Material[] materials;

  void Start () {
    if (materials.Length > 0) {
      renderer.material = materials[Random.Range(0, materials.Length)];
    }
  }

}
