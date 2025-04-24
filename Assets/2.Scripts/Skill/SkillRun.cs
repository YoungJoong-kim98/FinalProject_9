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
            Debug.Log("달리기스킬획득");
            GameManager.Instance.SkillManager.UnlockRun();
            UIManager.Instance.SkillUnlockUI.Show("run", runSkillIcon);
        }
    }
}
