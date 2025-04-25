using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRun : MonoBehaviour
{
    [SerializeField] private Sprite runSkillIcon;

    private bool hasPlayed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            Debug.Log("달리기스킬획득");
            GameManager.Instance.SkillManager.UnlockRun();
            EventManager.OnSkillUnlocked?.Invoke("run", runSkillIcon);
            hasPlayed = true;
        }
    }
}
