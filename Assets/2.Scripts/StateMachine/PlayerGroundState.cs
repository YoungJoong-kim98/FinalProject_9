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
            stateMachine.Player.Rigidbody.velocity.magnitude < 0.1f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        // 이동 중 아래로 떨어지고 있는 경우에만 Fall로 전환
        if (!IsGrounded() && stateMachine.Player.Rigidbody.velocity.y < -0.1f)
        {
            Debug.Log("GroundState에서 Fall로 전환");
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
    
    private bool IsGrounded() //땅 체크
    {
        Transform t = stateMachine.Player.transform;
        Debug.DrawRay(t.position + Vector3.up * 0.1f, Vector3.down * 0.2f, Color.red);
        return Physics.Raycast(t.position + Vector3.up * 0.1f, Vector3.down, 0.2f, LayerMask.GetMask("Ground"));
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if(stateMachine.MovementInput == Vector2.zero)  // Movement가 Canceled 되면
        {
            return;
        }

        stateMachine.ChangeState(stateMachine.IdleState);

        base.OnMovementCanceled(context);
    }
    
    protected override void OnRunCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.MovementInput != Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        GameManager.Instance.AchievementSystem.JumpCount(); //점프 횟수 카운트
        stateMachine.ChangeState(stateMachine.JumpState);
    }
}