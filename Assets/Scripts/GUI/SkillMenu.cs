using UnityEngine;
using System.Collections;

public class SkillMenu : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;
  public Texture skillMenuBackgroundTexture;

  public Texture skillMenuButtonTexture;

  public Texture hasNewSkillTexture;
  private float hasNewSkillTextureAlpha;

  public Texture[] skillTextures;
  public Texture[] hotkeyTextures;

  public Texture unknownTexture;
  public Texture lockedTexture;

  public int additionalQTELengthPerSkill = 1;

  private const int skillCount = 5;
  private bool[] unlocked = new bool[skillCount];
  private bool hasNewSkill;
  private string[] skillMessages = new string[skillCount];

  private Boss boss;

  private SoundEffectManager soundEffectManager;

  public void unlockSkill (int skillIndex) {
    if (skillIndex >= 0 && skillIndex < unlocked.Length) {
      unlocked[skillIndex] = true;
      hasNewSkill = true;
      if (Application.loadedLevelName == "OldCastle") {
        boss.addQTELength(additionalQTELengthPerSkill);
      }
    }
  }

  public void setSkillMessage (int skillIndex, string message) {
    if (skillIndex >= 0 && skillIndex < unlocked.Length) {
      skillMessages[skillIndex] = message;
    }
  }

  void Start () {
    if (Application.loadedLevelName == "OldCastle") {
      boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();
    }

    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    for (int i = 0; i < skillMessages.Length; i++) {
      if (skillMessages[i] == null || skillMessages[i] == "") {
        skillMessages[i] = "Unknown skill";
      }
    }
  }

  void Update () {
    int ms = (int)((Time.time * 100) % 100);
    if (ms < 50) {
      hasNewSkillTextureAlpha = (ms * 2) / 100.0f;
    } else {
      hasNewSkillTextureAlpha = ((100 - ms) * 2) / 100.0f;
    }
    
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
      int buttonWidth = Screen.height / 9;
      int buttonHeight = buttonWidth;
      if (hasNewSkill) {
        int messageWidth = buttonWidth * 2;
        int messageHeight = messageWidth / 2;
        Color originalColor = GUI.color;
        GUI.color = new Color(1, 1, 1, hasNewSkillTextureAlpha);
        GUI.DrawTexture(new Rect(messageWidth / 10, (int)(Screen.height - messageHeight - buttonHeight * 1.5), messageWidth, messageHeight), hasNewSkillTexture);
        GUI.color = originalColor;
      }
      GUI.DrawTexture(new Rect(buttonWidth / 10, (int)(Screen.height - buttonHeight * 1.5), buttonWidth, buttonHeight), skillMenuButtonTexture);
      return;
    }

    hasNewSkill = false;

    // Background
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

    // Menu background
    int width = Screen.height - 100;
    int height = width;

    int skillIconWidth = width / 10;
    int skillIconHeight = height / 10;
    int hotkeyIconWidth = width / 5;
    int hotkeyIconHeight = height / 10;
    int skillMessageWidth = width;
    int skillMessageHeight = height / 10;

    GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));

    GUI.DrawTexture(new Rect(0, 0, width, height), skillMenuBackgroundTexture);

    int startX = width / 6;
    int startY = height / 4;

    for (int i = 0; i < unlocked.Length; i++) {
      if (unlocked[i]) {
        GUI.DrawTexture(new Rect(startX, i * (hotkeyIconHeight + 10) + startY, hotkeyIconWidth, hotkeyIconHeight), hotkeyTextures[i]);
      }
      Texture skillTexture;
      if (unlocked[i]) {
        skillTexture = skillTextures[i];
      } else {
        skillTexture = unknownTexture;
      }
      GUI.DrawTexture(new Rect(startX + hotkeyIconWidth + 10, i * (skillIconHeight + 10) + startY, skillIconWidth, skillIconHeight), skillTexture);
      Color originalColor = GUI.color;
      GUI.color = Color.black;
      GUI.Label(new Rect(startX + hotkeyIconWidth + 10 + skillIconWidth + 20,
                        i * (skillMessageHeight + 10) + startY + skillMessageHeight / 3,
                        skillMessageWidth, skillMessageHeight), skillMessages[i]);
      GUI.color = originalColor;
      if (!unlocked[i]) {
        GUI.DrawTexture(new Rect(startX, i * (hotkeyIconHeight + 10) + startY, width - startX * 2, skillIconHeight), lockedTexture);
      }
    }

    GUILayout.EndArea();
  }
}
