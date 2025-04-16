using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLocked : MonoBehaviour
{
    [SerializeField] private Sprite allLockSkillIcon;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SkillManager.SkillLocked();
            GameManager.Instance.SkillUnlockUI.Show("all", allLockSkillIcon);
        }
    }
}
