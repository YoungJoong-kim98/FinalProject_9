using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGrab : MonoBehaviour
{
    [SerializeField] private Sprite grabSkillIcon;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SkillManager.UnlockGrab();
            GameManager.Instance.SkillUnlockUI.Show("grab", grabSkillIcon);
        }
    }
}
