using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        
    }
    
    // 바닥에 있을 때 호출
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    // 바닥에서 떨어질 때 호출
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    // 논리 업데이트
    public override void Update()
    {
        base.Update();
    }

    // 물리 업데이트
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        if (!IsGrounded()) // Raycast로 바닥 벗어남 감지
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }
    
    // 이동 입력이 취소될 때
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if(stateMachine.MovementInput == Vector2.zero)  // 입력이 완전히 없으면
        {
            return;
        }

        stateMachine.ChangeState(stateMachine.IdleState);   // 대기 상태로 전환

        base.OnMovementCanceled(context);
    }
    
    // 점프 입력이 시작될 때
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        stateMachine.ChangeState(stateMachine.JumpState);   // 점프 상태로 전환
    }
}