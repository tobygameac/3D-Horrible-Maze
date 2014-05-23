using UnityEngine;
using System.Collections;

public static class DemoState {

  public const int PLAYING = 0;
  public const int PAUSING = 1;

  public const int SIMPLE = 0;
  public const int MAZE = 1;

  public static int state;
  public static int mode;
  public static bool showAllBest;
}
