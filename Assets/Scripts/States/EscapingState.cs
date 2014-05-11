using UnityEngine;
using System.Collections;

public static class EscapingState {
  public const int BEGINNING = 0;
  public const int EXIT_FOUND = 1;
  public const int ELEVATOR_FOUND = 2;
  public const int KEY_FOUND = 3;

  public static int state;
}
