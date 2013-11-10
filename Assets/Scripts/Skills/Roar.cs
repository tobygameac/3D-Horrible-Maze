using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Vitality))]
[RequireComponent (typeof(Mentality))]

public class Roar : MonoBehaviour {

  private Vitality vitality;
  private Mentality mentality;

  public float vitalityCost = 50;
  public float mentalityGain = 10;

  void Start () {
    vitality = GetComponent<Vitality>();
    mentality = GetComponent<Mentality>();
  }

  void Update () {
    if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1)) {
      if (vitality.enough(vitalityCost)) {
        vitality.use(vitalityCost);
        mentality.gain(mentalityGain);
      }
    }
  }

  void OnGUI () {
    GUI.Label(new Rect(10, 80, 200, 20), "1 : Roar");
  }

}
