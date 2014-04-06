using UnityEngine;
using System.Collections;

public class SkillMenu : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;
  public Texture skillMenuBackgroundTexture;

  public Texture skillMenuButtonTexture;

  public Texture[] skillTextures;
  public Texture[] hotkeyTextures;

  public Texture unknownTexture;

  private bool[] unlocked = new bool[5];

  public void unlockSkill (int skillIndex) {
    if (skillIndex >= 0 && skillIndex < unlocked.Length) {
      unlocked[skillIndex] = true;
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
          GameState.state = GameState.SKILLVIEWING;
          Time.timeScale = 0.0001f;
          break;
        case GameState.SKILLVIEWING:
          GameState.state = GameState.PLAYING;
          Time.timeScale = 1;
          break;
      }

    }

  }

  void OnGUI () {
    
    // Only show menu button
    if (GameState.state != GameState.SKILLVIEWING) {
      GUI.DrawTexture(new Rect(10, Screen.height - 75, 50, 50), skillMenuButtonTexture);
      return;
    }

    GUI.skin = skin;

    // Background
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

    // Menu background
    int width = Screen.height - 100;
    int height = width;

    int skillIconWidth = width / 8;
    int skillIconHeight = height / 8;
    int hotkeyIconWidth = skillIconWidth * 2;
    int hotkeyIconHeight = skillIconHeight;

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
    }

    GUILayout.EndArea();
  }
}
