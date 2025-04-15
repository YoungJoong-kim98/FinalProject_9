using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRun : MonoBehaviour
{
    [SerializeField] private Sprite runSkillIcon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SkillManager.UnlockRun();
            GameManager.Instance.SkillUnlockUI.Show("run", runSkillIcon);
        }
    }
}
