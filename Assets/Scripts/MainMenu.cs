﻿using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

  void OnGUI () {
    GUI.Label(new Rect(0, 0, 100, 100), "2013/11/1 6.06pm");
    GUILayout.BeginArea(new Rect(Screen.width / 2 - 50, 200, 500, 500));
    if (GUI.Button(new Rect(0, 0, 100, 50), "Start")) {
      //audio.PlayOneShot(buttonSound);
      Application.LoadLevel("OldCastle");
    }
    GUILayout.EndArea();
  }
}