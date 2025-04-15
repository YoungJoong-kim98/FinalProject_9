using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLocked : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SkillManager.SkillLocked();
        }
    }
}
