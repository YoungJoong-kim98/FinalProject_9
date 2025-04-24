using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDoubleJump : MonoBehaviour
{
    [SerializeField] private Sprite doubleJumpSkillIcon;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.SkillManager.UnlockDoubleJump();
            UIManager.Instance.SkillUnlockUI.Show("doubleJump", doubleJumpSkillIcon);
        }
    }
}
