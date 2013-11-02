﻿using UnityEngine;
using System.Collections;

public class MouseHidder : MonoBehaviour {

  void Update () {
    Screen.showCursor = false;
    Screen.lockCursor = true;
  }

  void OnDestroy() {
    Screen.showCursor = true;
    Screen.lockCursor = false;
  }

}