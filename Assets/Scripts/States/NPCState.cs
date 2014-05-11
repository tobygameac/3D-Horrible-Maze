using UnityEngine;
using System.Collections;

public class NPCState : MonoBehaviour {

  public const int MAKING_DECISION = 0;
  public const int WANDERING = 1;
  public const int CHASING = 2;
  public const int STARING = 3;
  public const int TRACING = 4;
  public const int STUNNING = 5;
  public const int ATTACKING = 6;

  public int state;
}
