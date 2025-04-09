using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            PlayerController input = stateMachine.Player.Input;
            
            if (input.playerActions.Run.IsPressed())    // Shift 누르고 있으면
            {
                stateMachine.ChangeState(stateMachine.RunState);
            }
            else if (stateMachine.MovementInput != Vector2.zero)    // 이동 중이면
            {
                stateMachine.ChangeState(stateMachine.WalkState);
            }
            else  // 이동 없다면
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
            return;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && TryDetectGrabTarget(out string tag))
        {
            if (tag == "Rope")
            {
                stateMachine.ChangeState(stateMachine.RopeGrabState);
            }
            else if (tag == "Wall")
            {
                stateMachine.ChangeState(stateMachine.GrabState);
            }
            return;
        }
    }
    

    private bool IsGrounded() //땅인지 체크
    {
        Transform t = stateMachine.Player.transform;
        return Physics.Raycast(t.position + Vector3.up * 0.1f, Vector3.down, 0.2f, LayerMask.GetMask("Ground"));
    }

    private bool TryDetectGrabTarget(out string targetTag)
    {
        targetTag = null;

        Transform t = stateMachine.Player.transform;
        Vector3 origin = t.position + Vector3.up * 0.5f;
        float distance = 1.5f;

        // 로프 감지 (위쪽)
        if (Physics.Raycast(origin, Vector3.up, distance, LayerMask.GetMask("Rope")))
        {
            Debug.DrawRay(origin, Vector3.up * distance, Color.green);
            targetTag = "Rope";
            return true;
        }

        // 벽 감지 (앞쪽)
        if (Physics.Raycast(origin, t.forward, distance, LayerMask.GetMask("Ground")))
        {
            Debug.DrawRay(origin, t.forward * distance, Color.red);
            targetTag = "Wall";
            return true;
        }

        return false;
    }
    private void DebugDrawGrabRay()
    {
        Transform t = stateMachine.Player.transform;
        Vector3 origin = t.position + Vector3.up * 0.5f;
        float distance = 1.5f;

        // 위쪽 (로프용)
        Debug.DrawRay(origin, Vector3.up * distance, Color.green);

        // 앞쪽 (벽용)
        Debug.DrawRay(origin, t.forward * distance, Color.red);

        // 땅 체크용 아래 방향도 보고 싶으면 아래도 추가
        Debug.DrawRay(t.position + Vector3.up * 0.1f, Vector3.down * 0.1f, Color.yellow);
    }

}