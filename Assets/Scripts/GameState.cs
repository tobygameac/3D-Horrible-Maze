using UnityEngine;
using System.Collections;

public static class GameState {

  public const int PLAYING = 0;
  public const int PAUSING = 1;
  public const int SKILLVIEWING = 2;
  public const int LOSING = 3;
  public const int MENUVIEWING = 4;

  public static int state;
}
