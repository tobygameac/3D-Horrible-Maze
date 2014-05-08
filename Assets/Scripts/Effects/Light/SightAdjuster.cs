using UnityEngine;
using System.Collections;

public class SightAdjuster : MonoBehaviour {

  private Mentality mentality;
  private Camera mainCamera;

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
    mainCamera = Camera.main;
  }

  void Update () {
    float percent = mentality.getMentalityPointPercent();
    float ambientValue = ambient * percent + baseAmbient;
    Color ambientColor = new Color(ambientValue, ambientValue, ambientValue);
    RenderSettings.ambientLight = ambientColor;
    RenderSettings.fogColor = ambientColor;
    mainCamera.backgroundColor = ambientColor;
    light.intensity = lightIntensity * percent + baseLightIntensity;
    light.range = lightRange * percent + baseLightRange;
    light.spotAngle = lightAngle * percent + baseLightAngle;
  }

}
