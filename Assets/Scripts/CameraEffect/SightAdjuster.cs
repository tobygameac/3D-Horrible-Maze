using UnityEngine;
using System.Collections;

public class SightAdjuster : MonoBehaviour {

  private Mentality mentality;
  private Camera camera;

  public float ambient;
  public float baseAmbient;
  public float lightIntensity;
  public float baseLightIntensity;
  public float lightRange;
  public float baseLightRange;
  public float lightAngle;
  public float baseLightAngle;

  void Start () {
    mentality = GameObject.FindWithTag("Player").GetComponent<Mentality>();
    camera = Camera.mainCamera;
  }

  void Update () {
    float percent = mentality.getMentalityPointPercent();
    float ambientValue = ambient * percent + baseAmbient;
    Color ambientColor = new Color(ambientValue, ambientValue, ambientValue);
    RenderSettings.ambientLight = ambientColor;
    RenderSettings.fogColor = ambientColor;
    camera.backgroundColor = ambientColor;
    gameObject.light.intensity = lightIntensity * percent + baseLightIntensity;
    gameObject.light.range = lightRange * percent + baseLightRange;
    gameObject.light.spotAngle = lightAngle * percent + baseLightAngle;
  }

}
