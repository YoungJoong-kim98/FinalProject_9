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
        Vector3 moveDir = GetMovementDirection().normalized;    // WASD 입력 방향 (카메라 기준)

        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;  // 현재 속도 (달리기 반영)
        float jumpForce = stateMachine.Player.Data.AirData.JumpForce;   // 점프 힘

        Rigidbody rb = stateMachine.Player.Rigidbody;

        // // 기존 작성 부분
        // // 기존 Y 속도 초기화
        // Vector3 currentVelocity = rb.velocity;
        // currentVelocity.y = 0f;
        // rb.velocity = currentVelocity;
        //
        // //  이동 방향으로 가속도 주기 (Impulse)
        // Vector3 jumpDirection = moveDir * moveSpeed + Vector3.up * jumpForce;
        // rb.AddForce(jumpDirection, ForceMode.Impulse);  // ← 이걸로 "멀리 점프" 느낌 가능
        
        // 현재 수평 속도 가져오기 (달리기 속도 유지)
        Vector3 currentVelocity = rb.velocity;
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);

        // 점프 속도 = 기존 수평 속도 + 수직 점프 힘 + 약간의 방향 가속
        Vector3 jumpVelocity = horizontalVelocity + (Vector3.up * jumpForce);
        jumpVelocity += moveDir * moveSpeed * 0.5f; // 이동 방향으로 달리기 관성 강화

        rb.velocity = jumpVelocity; // 속도 적용

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

