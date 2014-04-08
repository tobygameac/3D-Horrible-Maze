using UnityEngine;
using System.Collections;

public class FaceUnderTheGround : MonoBehaviour {

  private int minAttackCooldown = 30;
  private int maxAttackCooldown = 120;

  private Vector3 originalLocalPosition;
  private Vector3 originalLocalRotation;

  private float riseDistance = 0.3f;
  private float risingSpeed = 0.15f;
  private float risedDistance;
  private bool attacking;

  private float attackTime;

  private Renderer[] childrenRenderers;

  private static System.Random random = new System.Random(); // Only need one random seed

  private SoundEffectManager soundEffectManager;

  private Transform player;

  void Start () {
    soundEffectManager = GameObject.FindWithTag("Main").GetComponent<SoundEffectManager>();

    player = transform.parent;

    originalLocalPosition = transform.localPosition;
    originalLocalRotation = transform.eulerAngles;
    childrenRenderers = GetComponentsInChildren<Renderer>();
    resetAttack();
  }

  void Update () {
    if (Time.time >= attackTime) {
      if (!attacking) {
        attack();
      }
      float distance = risingSpeed * Time.deltaTime;
      risedDistance += distance;
      transform.position += new Vector3(0, distance, 0);
      if (risedDistance >= riseDistance) {
        resetAttack();
      }
    }
  }

  void resetAttack () {
    transform.parent = player;
    transform.localPosition = originalLocalPosition;
    transform.localEulerAngles = originalLocalRotation;
    for (int i = 0; i < childrenRenderers.Length; i++) {
      childrenRenderers[i].enabled = false;
    }
    attackTime = Time.time + random.Next(minAttackCooldown + maxAttackCooldown);
    risedDistance = 0;
    attacking = false;
  }

  void attack () {
    transform.parent = null;
    transform.position = new Vector3(transform.position.x, player.transform.position.y + originalLocalPosition.y, transform.position.z);
    transform.eulerAngles = new Vector3(originalLocalRotation.x, transform.eulerAngles.y, originalLocalRotation.z);
    for (int i = 0; i < childrenRenderers.Length; i++) {
      childrenRenderers[i].enabled = true;
    }
    soundEffectManager.playCryingSound();
    attacking = true;
  }

}
