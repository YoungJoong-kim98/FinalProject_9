using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDoubleJump : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.SkillManager.UnlockDoubleJump();
        }
    }
}
