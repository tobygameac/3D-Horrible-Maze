using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadingWithAnimation : MonoBehaviour {

  private static System.Random random = new System.Random(); // Only need one random seed

  public Texture[] textures;
  public Texture blackMask;

  private float playingTime;
  private float playedTime;
  private bool isPlaying;

  private List<Texture> drawingTextures;
  private List<Rect> textureRect;
  private List<float> textureSizeScalingSpeed;

  private string loadingLevelName;

  void Start () {
    isPlaying = false;
  }

  void OnGUI () {
    if (isPlaying) {
      GUI.depth = 0;
      playedTime += Time.deltaTime;
      if (playedTime >= playingTime) {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackMask);
        isPlaying = false;
        Application.LoadLevel(loadingLevelName);
      }
      float colorValue = 1 - (playedTime / playingTime);
      GUI.color = new Color(colorValue, colorValue, colorValue);
      for (int i = 0; i < textureRect.Count; i++) {
        GUI.DrawTexture(textureRect[i], drawingTextures[i]);
        Rect rect = textureRect[i];
        float scale = 1 + textureSizeScalingSpeed[i] * Time.deltaTime;
        float newWidth = rect.width * scale;
        float newHeight = rect.height * scale;
        float deltaWidth = newWidth - rect.width;
        float deltaHeight = newHeight - rect.height;
        textureRect[i] = new Rect(rect.xMin - deltaWidth / 2, rect.yMin - deltaHeight / 2, newWidth, newHeight);
      }
    }
  }

  public bool almostFinished () {
    return playedTime + 0.05f >= playingTime;
  }

  public void loadLevelWithAnimation (string levelName, float time = 2.0f) {
    GameState.state = GameState.LOADING;
    playingTime = time;
    loadingLevelName = levelName;
    startAnimation();
  }

  private void startAnimation (int textureCount = 10) {
    drawingTextures = new List<Texture>();
    textureRect = new List<Rect>();
    textureSizeScalingSpeed = new List<float>();

    for (int i = 0; i < textureCount; i++) {
      drawingTextures.Add(textures[random.Next(textures.Length)]);
      float width = random.Next((int)(Screen.width * 0.25f));
      float height = random.Next((int)(Screen.height * 0.25f));
      float size = (width < height) ? width : height;
      Rect rect = new Rect(random.Next((int)(Screen.width - size)), random.Next((int)(Screen.height - size)), size, size);
      textureRect.Add(rect);
      textureSizeScalingSpeed.Add((float)random.NextDouble() + 1);
    }
    
    playedTime = 0;
    isPlaying = true;
  }
}
