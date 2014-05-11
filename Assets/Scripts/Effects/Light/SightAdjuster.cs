using UnityEngine;
using System.Collections;

public class SightAdjuster : MonoBehaviour {

  private Mentality mentality;
  private Camera mainCamera;
  private MotionBlur motionBlur;

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
    motionBlur = mainCamera.GetComponent<MotionBlur>();
  }

  void Update () {
    float percent = mentality.getMentalityPointPercent();
    float ambientValue = ambient * percent + baseAmbient;
    Color ambientColor = new Color(ambientValue, ambientValue, ambientValue);
    RenderSettings.ambientLight = ambientColor;
    RenderSettings.fogColor = ambientColor;
    mainCamera.backgroundColor = ambientColor;
    motionBlur.accumulation = 1 - percent;
    light.color = new Color(1, Mathf.Pow(percent, 0.3f), Mathf.Pow(percent, 0.3f));
    light.intensity = lightIntensity * percent + baseLightIntensity;
    light.range = lightRange * percent + baseLightRange;
    light.spotAngle = lightAngle * percent + baseLightAngle;
  }

}
