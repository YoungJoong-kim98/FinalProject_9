using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public bool run;
    public bool doubleJump;
    public bool grab;

    private void Start()
    {
        run = true;
        doubleJump = true;
        grab = true;
    }

    public void UnlockRun()
    {
        run = true;
    }
    public void UnlockDoubleJump()
    {
        doubleJump = true;
    }
    public void UnlockGrab()
    {
        grab = true;
    }
    public void SkillLocked()
    {
        run = false;
        doubleJump = false;
        grab = false;
    }
}
