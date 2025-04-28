using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        
    }
    
    public override void Enter()
    {
        base.Enter();
        stateMachine.CanDoubleJump = true;
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Update()
    {
        base.Update();
        
        // 장애물과 충돌 시 움직임 거의 0일 때 Idle로 전환
        if (stateMachine.MovementInput == Vector2.zero && 
            stateMachine.Player.Rigidbody.velocity.magnitude < 0.1f &&
            stateMachine.CurrentState != stateMachine.IdleState)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Rigidbody rb = stateMachine.Player.Rigidbody;
        
        // 이동 중 아래로 떨어지고 있는 경우에만 Fall로 전환
        if (!IsGrounded(1.0f, useOffset: true) && stateMachine.Player.Rigidbody.velocity.y < -3f)
        {
            // Debug.Log("GroundState에서 Fall로 전환");
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
        
        if (!IsGrounded()) return; // 접지 상태가 아니면 무시
        
        // 계단에서 y속도 보정
        if (rb.velocity.y > 0.1f)
        {
            Vector3 velocity = rb.velocity;

            if (stateMachine.MovementInput != Vector2.zero) // 이동 중일 땐 y속도 빠르게 보간
            {
                Vector3 targetVelocity = new Vector3(velocity.x, 0f, velocity.z);
                rb.velocity = Vector3.Lerp(velocity, targetVelocity, Time.deltaTime * 10f); // 목표 속도로 보간
                // Debug.Log("이동 중 계단 감속");
            }
            else // 멈췄을 때 y 속도만 감속
            {
                velocity.y = Mathf.Lerp(velocity.y, 0f, Time.deltaTime * 15f);
                rb.velocity = velocity;
                // Debug.Log("정지 중 계단 감속");
            }
        }
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if(stateMachine.MovementInput == Vector2.zero) return; // 입력 없으면 무시
        stateMachine.ChangeState(stateMachine.IdleState);
        base.OnMovementCanceled(context);
    }
    
    protected override void OnRunCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.MovementInput != Vector2.zero)
            stateMachine.ChangeState(stateMachine.WalkState);
        else
            stateMachine.ChangeState(stateMachine.IdleState);
    }
    
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        GameManager.Instance.AchievementSystem.JumpCount(); // 점프 횟수 카운트
        stateMachine.ChangeState(stateMachine.JumpState);
    }
}
