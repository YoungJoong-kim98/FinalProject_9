using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        Vector3 moveDir = GetMovementDirection().normalized;

        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        float jumpForce = stateMachine.Player.Data.AirData.JumpForce;

        Rigidbody rb = stateMachine.Player.Rigidbody;

        // 기존 Y 속도 초기화
        Vector3 currentVelocity = rb.velocity;
        currentVelocity.y = 0f;
        rb.velocity = currentVelocity;

        //  이동 방향으로 가속도 주기 (Impulse)
        Vector3 jumpDirection = moveDir * moveSpeed + Vector3.up * jumpForce;
        rb.AddForce(jumpDirection, ForceMode.Impulse);  // ← 이걸로 "멀리 점프" 느낌 가능

        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.Update();

        if (stateMachine.Player.Rigidbody.velocity.y <= 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}

