using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }
    public override void Update()
    {
        base.Update();
        DebugDrawGrabRay();
        if (IsGrounded())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && IsNearGrabbableWall())
        {
            stateMachine.ChangeState(stateMachine.GrabState);
            return;
        }
    }

    private bool IsGrounded()
    {
        Transform t = stateMachine.Player.transform;
        return Physics.Raycast(t.position + Vector3.up * 0.1f, Vector3.down, 0.2f, LayerMask.GetMask("Ground"));
    }

    private bool IsNearGrabbableWall()
    {
        Transform t = stateMachine.Player.transform;
        Vector3 origin = t.position + Vector3.up * 0.5f;
        Vector3 direction = t.forward;
        float distance = 1f;


        // 실제 Raycast 검사
        return Physics.Raycast(origin, direction, distance, LayerMask.GetMask("Ground"));
    }
    private void DebugDrawGrabRay()
    {
        Transform t = stateMachine.Player.transform;
        Vector3 origin = t.position + Vector3.up * 0.5f;
        Vector3 direction = t.forward;
        float distance = 1f;

        Debug.DrawRay(origin, direction * distance, Color.red);
    }
}