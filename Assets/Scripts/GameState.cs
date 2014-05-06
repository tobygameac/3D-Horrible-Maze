﻿using UnityEngine;
using System.Collections;

public static class GameState {

  public const int PLAYING = 0;
  public const int LOADING = 1;
  public const int PAUSING = 2;
  public const int SKILL_VIEWING = 3;
  public const int LOSING = 4;
  public const int MENU_VIEWING = 5;

  public static int state;
  public static int difficulty;
  public static bool userStudy;
  public static float volume;
}
