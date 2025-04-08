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
    
    // // 기존 작성
    // public override void Update()
    // {
    //     base.Update();
    //
    //     if (IsGrounded())
    //     {
    //         stateMachine.ChangeState(stateMachine.IdleState);
    //         return;
    //     }
    //     if (Mouse.current.leftButton.wasPressedThisFrame)
    //     {
    //         stateMachine.ChangeState(stateMachine.GrabState);
    //         return;
    //     }
    // }
    
    public override void Update()
    {
        base.Update();

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
        return Physics.Raycast(origin, t.forward, 1f, LayerMask.GetMask("Grabbable"));
    }
}