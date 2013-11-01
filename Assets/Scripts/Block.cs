using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
  public const int WALL = 0;
  public const int NOTHING = 0;
  public const int GENERAL = 1;
  public const int TREASURE = 2;
  public const int EXIT = 3;
  public const int UPABLE = 5;
  public const int DOWNABLE = 6;

  public Material[] materials;

  private int type;

  void Start () {
    if (type >= 0 && type < materials.Length) {
      renderer.material = materials[type];
    } else {
      renderer.material = materials[0];
    }
  }

  public void setType (int type) {
    if (type >= 0 && type < materials.Length) {
      this.type = type;
      renderer.material = materials[type];
    } else {
      this.type = 0;
      renderer.material = materials[type];
    }
  }

}
