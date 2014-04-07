using UnityEngine;
using System.Collections;

public class SkillMenu : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;
  public Texture skillMenuBackgroundTexture;

  public Texture skillMenuButtonTexture;

  public Texture hasNewSkillTexture;

  public Texture[] skillTextures;
  public Texture[] hotkeyTextures;

  public Texture unknownTexture;

  private const int skillCount = 5;
  private bool[] unlocked = new bool[skillCount];
  private bool hasNewSkill;
  private string[] skillMessages = new string[skillCount];

  private SoundEffectManager soundEffectManager;

  public void unlockSkill (int skillIndex) {
    if (skillIndex >= 0 && skillIndex < unlocked.Length) {
      unlocked[skillIndex] = true;
      hasNewSkill = true;
    }
  }

  public void setSkillMessage (int skillIndex, string message) {
    if (skillIndex >= 0 && skillIndex < unlocked.Length) {
      skillMessages[skillIndex] = message;
    }
  }

  void Start () {

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    for (int i = 0; i < skillMessages.Length; i++) {
      if (skillMessages[i] == "") {
        skillMessages[i] = "Unknown skill";
      }
    }
  }

  void Update () {

    if (Input.GetKeyDown(KeyCode.K)) {

      switch (GameState.state) {
        case GameState.PAUSING:
          GameState.state = GameState.PAUSING;
          Time.timeScale = 0.0001f;
          break;
        case GameState.PLAYING:
          soundEffectManager.playButtonSound();
          GameState.state = GameState.SKILLVIEWING;
          Time.timeScale = 0.0001f;
          break;
        case GameState.SKILLVIEWING:
          soundEffectManager.playButtonSound();
          GameState.state = GameState.PLAYING;
          Time.timeScale = 1;
          break;
      }

    }

  }

  void OnGUI () {

    GUI.skin = skin;
    GUI.depth = 0;
    
    // Only show menu button
    if (GameState.state != GameState.SKILLVIEWING) {
      if (hasNewSkill) {
        GUI.DrawTexture(new Rect(10, Screen.height - 130, 100, 50), hasNewSkillTexture);
      }
      GUI.DrawTexture(new Rect(10, Screen.height - 75, 50, 50), skillMenuButtonTexture);
      return;
    }

    hasNewSkill = false;

    // Background
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

    // Menu background
    int width = Screen.height - 100;
    int height = width;

    int skillIconWidth = width / 8;
    int skillIconHeight = height / 8;
    int hotkeyIconWidth = width / 4;
    int hotkeyIconHeight = height / 8;
    int skillMessageWidth = width;
    int skillMessageHeight = height / 8;

    GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));

    GUI.DrawTexture(new Rect(0, 0, width, height), skillMenuBackgroundTexture);

    for (int i = 0; i < unlocked.Length; i++) {
      Texture skillTexture;
      if (unlocked[i]) {
        skillTexture = skillTextures[i];
      } else {
        skillTexture = unknownTexture;
      }
      GUI.DrawTexture(new Rect(width / 7, i * (hotkeyIconHeight + 10) + height / 7, hotkeyIconWidth, hotkeyIconHeight), hotkeyTextures[i]);
      GUI.DrawTexture(new Rect(width / 7 + hotkeyIconWidth + 10, i * (skillIconHeight + 10) + height / 7, skillIconWidth, skillIconHeight), skillTexture);
      Color originalColor = GUI.color;
      GUI.color = Color.black;
      GUI.Label(new Rect(width / 7 + hotkeyIconWidth + 10 + skillIconWidth + 20,
                        i * (skillMessageHeight + 10) + height / 7 + skillMessageHeight / 3,
                        skillMessageWidth, skillMessageHeight), skillMessages[i]);

      GUI.color = originalColor;
    }

    GUILayout.EndArea();
  }
}
