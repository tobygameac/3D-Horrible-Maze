using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {

  private Transform cameraTransform;
  private Vector3 originalPosition;

  void Update () {
    if (cameraTransform == null) {
      cameraTransform = Camera.main.transform;
      if (cameraTransform != null) {
        originalPosition = cameraTransform.localPosition;
      }
    }
  }

  public IEnumerator shakeCamera (int numberOfShakes = 5, float shakingDistance = 0.5f, float shakingSpeed = 75.0f, float decreasingPercent = 0.3f) {

    if (cameraTransform == null) {
      yield return 0;
    }

    float lastShakingTime = Time.time;

    while (numberOfShakes > 0) {

      float timer = (Time.time - lastShakingTime) * shakingSpeed;
      float shakingValue = Mathf.Sin(timer) * shakingDistance;

      cameraTransform.localPosition = new Vector3(originalPosition.x + shakingValue, originalPosition.y, originalPosition.z);
      
      if (timer > Mathf.PI * 2) {
        lastShakingTime = Time.time;
        shakingDistance *= decreasingPercent;
        numberOfShakes--;
      }
      yield return null;
    }
    
    cameraTransform.localPosition = originalPosition;
    
    yield return 0;
  }
}
