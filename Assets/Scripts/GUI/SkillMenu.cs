using UnityEngine;
using System.Collections;

public class SkillMenu : MonoBehaviour {

  public GUISkin skin;

  public Texture backgroundTexture;

  public Texture[] skillTextures;
  public Texture unknownTexture;

  public int iconWidth = 100;
  public int iconHeight = 100;

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
    
    if (GameState.state != GameState.SKILLVIEWING) {
      return;
    }

    GUI.skin = skin;

    // Background
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height),  backgroundTexture);

    // Skill
    GUILayout.BeginArea(new Rect(Screen.width / 2 - iconWidth / 2, 100, 1000, 1000));

    for (int i = 0; i < unlocked.Length; i++) {
      Texture iconTexture;
      if (unlocked[i]) {
        iconTexture = skillTextures[i];
      } else {
        iconTexture = unknownTexture;
      }
      GUI.DrawTexture(new Rect(0, i * (iconHeight + 10), iconWidth, iconHeight), iconTexture);
    }

    GUILayout.EndArea();
  }
}
