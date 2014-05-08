using UnityEngine;
using System.Collections;

public class SightRandomEffect : MonoBehaviour {

  public float delayTime;
  public float delayTimeOffset;
  
  private float originalIntensity;
  private float nextEffectTime;

  private SightAdjuster sightAdjuster;

  void Start () {
    sightAdjuster = GetComponent<SightAdjuster>();
    getNextEffectTime();
  }

  void getNextEffectTime () {
    nextEffectTime = Time.time + delayTime + Random.Range(-delayTimeOffset, delayTimeOffset);
  }

  void Update () {
    if (Time.time > nextEffectTime) {
      originalIntensity = light.intensity;
      sightAdjuster.enabled = false;
      int type = Random.Range(0, 3);
      switch (type) {
        case 0:
	  StartCoroutine(flick());
          break;
        case 1:
	  StartCoroutine(fade());
          break;
        case 2:
	  StartCoroutine(random());
          break;
      }
      getNextEffectTime();
    }
  }

  IEnumerator flick () {
    int flickTimes = Random.Range(1, 6) * 2;
    for (int i = 0; i < flickTimes; i++) {
      if (i % 2 == 0) { // Turn Off
        light.intensity = 0.0f;
        yield return new WaitForSeconds(0.1f);
      } else { // Turn On
        light.intensity = originalIntensity;
        yield return new WaitForSeconds(0.05f);
      }
    }
    sightAdjuster.enabled = true;
  }

  IEnumerator fade ()  {
    while (light.intensity > 0) {
      light.intensity -= 0.02f;
      yield return true;
    }
    light.intensity = originalIntensity;
    sightAdjuster.enabled = true;
  }

  IEnumerator random ()  {
    for (int i = 0; i < 10; i++) {
      light.intensity = Random.Range(0, originalIntensity);
      yield return true;
    }
    light.intensity = originalIntensity;
    sightAdjuster.enabled = true;
  }

}
