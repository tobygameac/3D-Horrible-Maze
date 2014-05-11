using UnityEngine;
using System.Collections;

public class HeadBobber : MonoBehaviour {

  private float timer;
  private Vector2 midpoint;

  public float bobbingSpeed;
  public float bobbingAmount;

  private Footstep footstep;

  void Start () {
    footstep = GameObject.FindWithTag("Player").GetComponentInChildren<Footstep>();

    timer = 0;
    midpoint = new Vector2(transform.localPosition.x, transform.localPosition.y);
  }

  void Update () {
    float waveslice = 0;
    float deltaH = Input.GetAxis("Horizontal");
    float deltaV = Input.GetAxis("Vertical");
    
    bool isRunning = footstep.isRunning;
    float scale = 1;
    if (isRunning) {
      scale = 1.5f;
    }
    if (deltaH == 0 && deltaV == 0) {
       timer = 0;
    } else {
      waveslice = Mathf.Sin(timer);
      timer = timer + bobbingSpeed * scale;
      if (timer > Mathf.PI * 2) {
        timer = timer - (Mathf.PI * 2);
      } 
    }
    
    Vector2 newPoint = midpoint;
    if (waveslice != 0) {
      float translateChange = waveslice * bobbingAmount * scale;
      float totalAxes = Mathf.Abs(deltaH) + Mathf.Abs(deltaV);
      totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
      translateChange = totalAxes * translateChange;
      newPoint.y = midpoint.y + translateChange;
      if (isRunning) {
        newPoint.x = midpoint.x + translateChange;
      }
    }
    transform.localPosition = new Vector3(newPoint.x, newPoint.y, transform.localPosition.z);
  }

}
