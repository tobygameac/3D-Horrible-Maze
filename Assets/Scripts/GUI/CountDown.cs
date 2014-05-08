using UnityEngine;
using System.Collections;

public class CountDown : MonoBehaviour {

  public Texture[] countTextures;
  public AudioClip countAudio;

  private float countTime;
  private float audioPlayTime;
  private bool audioPlayed;
  private bool countToZero;

  private SoundEffectManager soundEffectManager;

  void Start () {
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    countTime = 0;
  }

  void Update () {
    if (countTime > 0 || (countToZero && countTime > -0.1f)) {
      countTime -= Time.deltaTime;
    }
  }

  void OnGUI () {
    if (countTime > 0 || (countToZero && countTime > -0.1f)) {

      if ((int)countTime >= countTextures.Length) {
        return;
      }

      if (countTime < audioPlayTime && !audioPlayed) {
        soundEffectManager.play(countAudio);
        audioPlayed = true;
      }

      float width = Screen.width * 0.5f;
      float height = Screen.height * 0.5f;
      float size = (width < height) ? width : height;
      float startX = (Screen.width - size) / 2.0f;
      float startY = (Screen.height - size) / 2.0f;
      GUI.DrawTexture(new Rect(startX, startY, size, size), countTextures[(int)countTime]);
    }
  }

  public void startCountDown (float time, float audioTime, bool countZero = false) {
    countTime = time;
    audioPlayTime = audioTime;
    countToZero = countZero;
    audioPlayed = false;
  }

}
