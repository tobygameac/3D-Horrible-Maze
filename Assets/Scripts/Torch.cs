using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour {

  public GameObject TorchLight;
  public GameObject MainFlame;
  public GameObject BaseFlame;
  public GameObject Etincelles;
  public GameObject Fumee;
  public float MaxLightIntensity;
  public float intensityLight;
  public bool randomIntensity;

  private static System.Random random = new System.Random(); // Only need one random seed

  void Start () {
    if (randomIntensity) {
      intensityLight = (float)(random.NextDouble() * MaxLightIntensity);
    }
    TorchLight.light.intensity = intensityLight;
    MainFlame.GetComponent<ParticleSystem>().emissionRate = intensityLight * 20;
    BaseFlame.GetComponent<ParticleSystem>().emissionRate = intensityLight * 15;	
    Etincelles.GetComponent<ParticleSystem>().emissionRate = intensityLight * 7;
    Fumee.GetComponent<ParticleSystem>().emissionRate = intensityLight * 12;
  }

  void Update () {
    if (intensityLight < 0) {
      intensityLight = 0;
    }

    if (intensityLight > MaxLightIntensity) {
      intensityLight = MaxLightIntensity;
    }

    TorchLight.light.intensity = intensityLight / 2f + Mathf.Lerp(intensityLight - 0.1f, intensityLight + 0.1f, Mathf.Cos(Time.time * 30));
    TorchLight.light.color = new Color(Mathf.Min(intensityLight / 1.5f, 1f), Mathf.Min(intensityLight / 2f, 1), 0);
    MainFlame.GetComponent<ParticleSystem>().emissionRate = intensityLight * 20;
    BaseFlame.GetComponent<ParticleSystem>().emissionRate = intensityLight * 15;
    Etincelles.GetComponent<ParticleSystem>().emissionRate = intensityLight * 7;
    Fumee.GetComponent<ParticleSystem>().emissionRate = intensityLight * 12;
  }

}
