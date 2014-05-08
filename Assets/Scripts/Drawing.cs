using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drawing : MonoBehaviour {

  private static System.Random random = new System.Random(); // Only need one random seed

  public Material[] brokenGlassMaterials;

  private Transform body;

  private MazeGenerator maze;

  private GameObject player;

  private SoundEffectManager soundEffectManager;

  public float minimumCheckRadius;
  public float maximumCheckRadius;
  private float checkRadius;
  public float maximumBrokenCountDown;
  private float brokenCountDown;
  private bool broken;

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();

    player = GameObject.FindWithTag("Player");

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    body = transform.GetChild(0);
    
    checkRadius = (float)random.NextDouble() * (maximumCheckRadius - minimumCheckRadius) + minimumCheckRadius;
    brokenCountDown = (float)random.NextDouble() * maximumBrokenCountDown;
    broken = false;
  }

  void Update () {
    if (broken) {
      return;
    }

    if (!broken) {
      if (checkVisible()) {
        brokenCountDown -= Time.deltaTime;
      }
      if (brokenCountDown <= 0) {
        broken = true;
        addBrokenGlass();
      }
      return;
    }
  }

  private bool checkVisible () {
    if (maze.getFloor(transform.position.y) != maze.getFloor(player.transform.position.y)) {
      return false;
    }
    if (Vector3.Distance(transform.position, player.transform.position) > checkRadius) {
      return false;
    }
    RaycastHit hit;
    if (Physics.Raycast(player.transform.position, player.transform.forward, out hit)) {
      if (hit.transform == transform) {
        return true;
      }
    }
    return false;
  }

  private void addBrokenGlass () {
    soundEffectManager.playGlassShatteredSoundSound();
    List<Material> newMaterials = new List<Material>();
    for (int i = 0; i < body.renderer.materials.Length; i++) {
      newMaterials.Add(body.renderer.materials[i]);
    }
    newMaterials.Add(brokenGlassMaterials[random.Next(brokenGlassMaterials.Length)]);
    body.renderer.materials = newMaterials.ToArray();
  }
}
