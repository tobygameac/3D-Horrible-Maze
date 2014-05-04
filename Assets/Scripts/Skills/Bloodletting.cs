using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Mentality))]

public class Bloodletting : MonoBehaviour {

  private static System.Random random = new System.Random(); // Only need one random seed

  public float mentalityCost = 10.0f;

  private Mentality mentality;

  private SkillMenu skillMenu;

  private MazeGenerator maze;

  private GameObject bloodPrefab;

  private SoundEffectManager soundEffectManager;

  void Start () {
    mentality = GetComponent<Mentality>();
    
    skillMenu = GameObject.FindWithTag("Main").GetComponent<SkillMenu>();
    skillMenu.unlockSkill(4);

    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();

    bloodPrefab =  GameObject.FindGameObjectWithTag("BloodFromPlayer");

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
  }

  void Update () {
    if (GameState.state != GameState.PLAYING) {
      return;
    }
    if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4)) {
      if (mentality.enough(mentalityCost)) {
        soundEffectManager.playBloodHitSound();
        mentality.use(mentalityCost);
        float baseY = maze.getBaseY(maze.getFloor(transform.position.y));
        Vector3 bloodPosition = new Vector3(2 * transform.forward.x + transform.position.x, baseY + 0.001f, 2 * transform.forward.z + transform.position.z);
        GameObject blood = Instantiate(bloodPrefab, bloodPosition, Quaternion.identity) as GameObject;
        blood.transform.localScale = new Vector3(maze.BLOCK_SIZE, 0.01f, maze.BLOCK_SIZE);
        blood.transform.eulerAngles = new Vector3(0, random.Next(360), 0);
      } else {
        soundEffectManager.playErrorSound();
        MessageViewer.showErrorMessage("Not enough mentality");
      }
    }
    string skillMessage = "Blooddd~~";
    skillMenu.setSkillMessage(4, skillMessage);
  }

}
