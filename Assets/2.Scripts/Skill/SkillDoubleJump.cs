using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDoubleJump : MonoBehaviour
{
    [SerializeField] private Sprite doubleJumpSkillIcon;
    private bool hasPlayed = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !hasPlayed)
        {
            GameManager.Instance.SkillManager.UnlockDoubleJump();
            EventManager.OnSkillUnlocked?.Invoke("doubleJump", doubleJumpSkillIcon);
            hasPlayed = true;
        }
    }
}
