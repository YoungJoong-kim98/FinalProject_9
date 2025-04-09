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
        IsNearRope();
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
        if (Mouse.current.leftButton.wasPressedThisFrame && IsNearRope())
        {
            stateMachine.ChangeState(stateMachine.RopeGrabState); // ← 로프 잡기 상태로!
            return;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && IsNearGrabbableWall())
        {
            stateMachine.ChangeState(stateMachine.GrabState);
            return;
        }
    }
    

    private bool IsGrounded() //땅인지 체크
    {
        Transform t = stateMachine.Player.transform;
        return Physics.Raycast(t.position + Vector3.up * 0.1f, Vector3.down, 0.2f, LayerMask.GetMask("Ground"));
    }

    private bool IsNearGrabbableWall() // 벽이 있는지 체크
    {
        Transform t = stateMachine.Player.transform;
        Vector3 origin = t.position + Vector3.up * 0.5f;
        Vector3 direction = t.forward;
        float distance = 1f;


        // ���� Raycast �˻�
        return Physics.Raycast(origin, direction, distance, LayerMask.GetMask("Ground"));
    }
    private bool IsNearRope()
    {
        Transform t = stateMachine.Player.transform;
        Vector3 origin = t.position + Vector3.up * 1.0f; //시작위치
        Vector3 direction = Vector3.up; //방향
        float distance = 1.5f; //길이

        Debug.DrawRay(origin, direction * distance, Color.green); // 더 보기 쉽게

        return Physics.Raycast(origin, direction, distance, LayerMask.GetMask("Rope"));
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