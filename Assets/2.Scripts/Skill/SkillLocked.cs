using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLocked : MonoBehaviour
{
    [SerializeField] private Sprite allLockSkillIcon;
    private bool hasPlayed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            GameManager.Instance.SkillManager.SkillLocked();
            EventManager.OnSkillUnlocked?.Invoke("all", allLockSkillIcon);
            hasPlayed = true;
        }
    }
}
