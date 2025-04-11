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
        
        float currentMoveSpeed = stateMachine.CurrentMoveSpeed; // 현재 속도 반영 (걷기 속도 or 달리기 가속도)
        float jumpForce = stateMachine.Player.Data.AirData.JumpForce;   // 점프 힘

        Rigidbody rb = stateMachine.Player.Rigidbody;
        
        // 현재 수평 속도 가져오기 (달리기 속도 유지)
        Vector3 currentVelocity = rb.velocity;
        currentVelocity.y = 0f; // y 속도 초기화 (낙하하며 점프할 때 y가 음수로 곱해지는 것 방지)
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
        
        // 점프 속도 = 현재 수평 속도 + 수직 점프 힘 + 방향별 가속도
        Vector3 jumpVelocity = horizontalVelocity + (Vector3.up * jumpForce);
        if (currentMoveSpeed > stateMachine.MovementSpeed) // 달리기 중일 때만 추가 가속 (현재 속도 > 2f)
        {
            jumpVelocity += moveDir * (currentMoveSpeed - stateMachine.MovementSpeed) * 0.5f; // 가속도 차이 반영 ((5f - 2f = 3f) * 0.5f)
        }

        rb.velocity = jumpVelocity; // 속도 적용

        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        
        Debug.Log($"Jump - 현재 이동 속도: {currentMoveSpeed}, 수평 속도: {horizontalVelocity.magnitude}, 적용 후 속도: {rb.velocity.magnitude}");
    }
    
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //  공중 수평 이동 제어
        Vector3 inputDir = GetMovementDirection();
        inputDir.Normalize();

        Vector3 airControlForce = inputDir * stateMachine.Player.Data.AirData.AirControlSpeed;
        Vector3 velocity = stateMachine.Player.Rigidbody.velocity;

        // y 속도는 그대로 유지하고 x,z만 수정
        Vector3 newVelocity = new Vector3(airControlForce.x, velocity.y, airControlForce.z);
        stateMachine.Player.Rigidbody.velocity = newVelocity;

        // 낙하 상태로 전환
        if (velocity.y <= 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }
}

