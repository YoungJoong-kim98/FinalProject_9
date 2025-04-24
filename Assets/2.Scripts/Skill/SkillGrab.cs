using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGrab : MonoBehaviour
{
    [SerializeField] private Sprite grabSkillIcon;

    private bool hasPlayed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            GameManager.Instance.SkillManager.UnlockGrab();
            EventManager.OnSkillUnlocked?.Invoke("grab", grabSkillIcon);
            hasPlayed = true;
        }
    }
}
