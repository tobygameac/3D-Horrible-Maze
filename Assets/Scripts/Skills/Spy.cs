using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Vitality))]

public class Spy : MonoBehaviour {

  public Texture playerTexture;
  public Texture startPointTexture;
  public Texture endPointTexture;
  public Texture availableBlockTexture;
  public Texture notAvailableBlockTexture;
  public Texture unknownBlockTexture;
  public Texture wallTexture;

  public float vitalityCost = 50;
  public float spyingTime = 10;

  public int radius;

  private float spiedTime;
  private bool isSpying = false;
  private float alpha;

  private Vitality vitality;
  
  private MazeGenerator maze;
  private int R;
  private int C;

  private GameObject player;

  private SkillMenu skillMenu;

  private SoundEffectManager soundEffectManager;

  void Start () {
    vitality = GetComponent<Vitality>();
    maze = GameObject.FindWithTag("Main").GetComponent<MazeGenerator>();
    player = GameObject.FindWithTag("Player");

    R = maze.MAZE_R;
    C = maze.MAZE_C;

    skillMenu = GameObject.FindWithTag("Main").GetComponent<SkillMenu>();
    skillMenu.unlockSkill(3);
    skillMenu.setSkillMessage(3, "I see u~~~~~~");

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();
  }

  void Update () {  
    if (GameState.state != GameState.PLAYING) {
      return;
    }

    if (isSpying) {
      spiedTime += Time.deltaTime;
      if (spiedTime >= spyingTime) {
        isSpying = false;
      }
    }

    if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)) {
      if (!isSpying) {
        if (vitality.enough(vitalityCost)) {
          vitality.use(vitalityCost);
          isSpying = true;
          spiedTime = 0;
        } else {
          // soundEffectManager.playErrorSound();
          // MessageViewer.showErrorMessage("Not enough vitality");
        }
      }
    }
  }

  void OnGUI () {
    if (!isSpying) {
      return;
    }
    GUI.color = new Color(1, 1, 1, 1 - (spiedTime / spyingTime));
    float width = (Screen.width * 0.25f);
    float height = (Screen.height * 0.25f);
    float startX = (Screen.width - width) / 2;
    float startY = 0;
    Vector3 playerHRC = maze.convertCoordinatesToHRC(player.transform.position);
    int h = maze.getFloor(player.transform.position);
    GUILayout.BeginArea(new Rect(startX, startY, width, height));
    float blockWidth = width / (C + 2);
    float blockHeight = height / (R + 2);
    for (int r = 0; r < R + 2; r++) {
      for (int c = 0; c < C + 2; c++) {
        Texture blockTexture = wallTexture;
        if (Mathf.Abs(r - playerHRC.y) <= radius && Mathf.Abs(c - playerHRC.z) <= radius) {
          if (maze.isEmptyBlock(h, r, c)) {
            if (maze.isAvailableBlock(h, r, c)) {
              if (r == playerHRC.y && c == playerHRC.z) {
                blockTexture = playerTexture;
              } else if (h != 0 && maze.isStartPoint(h, r, c)) {
                blockTexture = startPointTexture;
              } else if (h != maze.MAZE_H - 1 && maze.isEndPoint(h, r, c)) {
                blockTexture = endPointTexture;
              } else {
                blockTexture = availableBlockTexture;
              }
            } else {
              blockTexture = notAvailableBlockTexture;
            }
          }
        } else {
          blockTexture = unknownBlockTexture;
        }
        GUI.DrawTexture(new Rect(r * blockWidth, c * blockHeight, blockWidth, blockHeight), blockTexture);
      }
    }
    GUILayout.EndArea();
  }

}
