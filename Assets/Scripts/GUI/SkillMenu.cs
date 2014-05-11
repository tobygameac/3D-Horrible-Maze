using UnityEngine;
using System.Collections;

public class SkillMenu : MonoBehaviour {

  public GUISkin bloodOnHoverSkin;
  public GUISkin titleSkin;

  public Texture backgroundTexture;
  public Texture skillMenuBackgroundTexture;

  public Texture skillMenuButtonTexture;

  public Texture hasNewSkillTexture;
  private float hasNewSkillTextureAlpha;

  public Texture[] skillTextures;
  public Texture[] hotkeyTextures;

  public Texture unknownTexture;
  public Texture lockedTexture;

  private const int skillCount = 5;
  private bool[] unlocked = new bool[skillCount];
  private bool hasNewSkill;
  private string[] skillMessages = new string[skillCount];

  private SoundEffectManager soundEffectManager;

  void Start () {
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
          GameState.state = GameState.SKILL_VIEWING;
          Time.timeScale = 0.0001f;
          break;
        case GameState.SKILL_VIEWING:
          soundEffectManager.playButtonSound();
          GameState.state = GameState.PLAYING;
          Time.timeScale = 1;
          break;
      }

    }

  }

  void OnGUI () {

    Color originalColor = GUI.color;

    GUI.depth = 0;
    
    // Only show menu button
    if (GameState.state != GameState.SKILL_VIEWING) {
      if (GameState.state != GameState.PLAYING) {
        return;
      }
      int buttonWidth = Screen.height / 12;
      int buttonHeight = buttonWidth;
      if (hasNewSkill) {
        int messageWidth = buttonWidth * 2;
        int messageHeight = messageWidth / 2;
        GUI.color = new Color(1, 1, 1, hasNewSkillTextureAlpha);
        GUI.DrawTexture(new Rect(messageWidth / 10, (int)(Screen.height - messageHeight - buttonHeight - buttonWidth / 10), messageWidth, messageHeight), hasNewSkillTexture);
        GUI.color = originalColor;
      }
      GUI.DrawTexture(new Rect(buttonWidth / 10, (int)(Screen.height - buttonHeight - buttonWidth / 10), buttonWidth, buttonHeight), skillMenuButtonTexture);
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

    GUI.skin = titleSkin;
    GUI.skin.label.fontSize = (Screen.width + Screen.height) / 15;
    float titleWidth = width / 2.0f;
    float titleHeight = height / 4.0f;
    GUI.color = new Color(0.3f, 0, 0);
    GUI.Label(new Rect((width - titleWidth) / 2, 0, titleWidth, titleHeight), "Skills");
    GUI.color = originalColor;
    
    GUI.skin = bloodOnHoverSkin;

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
      GUI.color = Color.black;
      GUI.skin.label.fontSize = (Screen.width + Screen.height) / 75;
      GUI.Label(new Rect(startX + hotkeyIconWidth + 10 + skillIconWidth + 20,
                        i * (skillMessageHeight + 10) + startY + skillMessageHeight / 3,
                        skillMessageWidth, skillMessageHeight), skillMessages[i]);
      GUI.color = originalColor;
      if (!unlocked[i]) {
        // GUI.DrawTexture(new Rect(startX, i * (hotkeyIconHeight + 10) + startY, width - startX * 2, skillIconHeight), lockedTexture);
      }
    }

    GUILayout.EndArea();
  }

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
}
