using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour {

  public GameObject TorchLight;
  public GameObject MainFlame;
  public GameObject BaseFlame;
  public GameObject Etincelles;
  public GameObject Fumee;
  public float MaxLightIntensity;
  public float IntensityLight;

  void Start () {
    TorchLight.light.intensity = IntensityLight;
    MainFlame.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 20;
    BaseFlame.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 15;	
    Etincelles.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 7;
    Fumee.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 12;
  }

  void Update () {
    if (IntensityLight < 0) {
      IntensityLight = 0;
    }

    if (IntensityLight > MaxLightIntensity) {
      IntensityLight = MaxLightIntensity;
    }

    TorchLight.light.intensity = IntensityLight / 2f + Mathf.Lerp(IntensityLight - 0.1f, IntensityLight + 0.1f, Mathf.Cos(Time.time * 30));
    TorchLight.light.color = new Color(Mathf.Min(IntensityLight / 1.5f, 1f), Mathf.Min(IntensityLight / 2f, 1), 0);
    MainFlame.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 20;
    BaseFlame.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 15;
    Etincelles.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 7;
    Fumee.GetComponent<ParticleSystem>().emissionRate = IntensityLight * 12;
  }

}
