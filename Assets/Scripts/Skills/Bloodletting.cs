using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Mentality))]

public class Bloodletting : MonoBehaviour {

  private MazeGenerator maze;

  private GameObject bloodPrefab;

  private Mentality mentality;

  private float cooldown = 5.0f;
  private float cooldownNow;

  private static System.Random random = new System.Random(); // Only need one random seed

  void Start () {
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
    bloodPrefab =  GameObject.FindGameObjectWithTag("BloodFromPlayer");
    mentality = GetComponent<Mentality>();
  }

  void Update () {
    if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4)) {
      if (cooldownNow <= 0) {
        float baseY = maze.getBaseY(maze.getFloor(transform.position.y));
        Vector3 bloodPosition = new Vector3(transform.position.x, baseY + 0.001f, transform.position.z);
        GameObject blood = Instantiate(bloodPrefab, bloodPosition, Quaternion.identity) as GameObject;
        blood.transform.localScale = new Vector3(maze.BLOCK_SIZE, 0.01f, maze.BLOCK_SIZE);
        blood.transform.eulerAngles = new Vector3(0, random.Next(360), 0);
        cooldownNow = cooldown;
      }
    }
    if (cooldownNow > 0) {
      cooldownNow -= Time.deltaTime;
    }
  }

  void OnGUI () {
    string skillMessage = "4 : Bloodletting";
    if (cooldownNow > 0) {
      skillMessage += " CD " + (int)cooldownNow + "s";
    }
    GUI.Label(new Rect(10, 140, 200, 20), skillMessage);
  }

}
