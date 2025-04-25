using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }
    
    public override void Enter()
    {
        ApplyJumpVelocity(); // 점프 속도 적용
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }
    
    // 점프 속도 계산
    private void ApplyJumpVelocity()
    {
        Vector3 moveDir = GetMovementDirection().normalized;    // WASD 입력 방향 (카메라 기준)
        float currentMoveSpeed = stateMachine.CurrentMoveSpeed; // 현재 속도 반영 (걷기 속도 or 달리기 가속도)
        float jumpForce = stateMachine.Player.Data.AirData.JumpForce;
        Rigidbody rb = stateMachine.Player.Rigidbody;

        // 현재 수평 속도 가져오기 (달리기 속도 유지)
        Vector3 currentVelocity = rb.velocity;
        currentVelocity.y = 0f; // y 속도 초기화 (낙하하며 점프할 때 y가 음수로 곱해지는 것 방지)
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);

        // 점프 속도 = 현재 수평 속도 + 수직 점프 힘 + 방향별 가속도
        Vector3 jumpVelocity = horizontalVelocity + Vector3.up * jumpForce;
        if (currentMoveSpeed > stateMachine.MovementSpeed)  // 달리기 중일 때만 추가 가속
        {
            jumpVelocity += moveDir * (currentMoveSpeed - stateMachine.MovementSpeed) * 0.5f;   // 가속도 차이 반영
        }

        rb.velocity = jumpVelocity;
    }
    
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }
    
    // public override void PhysicsUpdate()
    // {
    //     base.PhysicsUpdate();   // AirState.PhysicsUpdate 호출
    //     
    //     Rigidbody rb = stateMachine.Player.Rigidbody;
    //     
    //     // 낙하 상태로 전환
    //     if (rb.velocity.y < 0)
    //     {
    //         stateMachine.ChangeState(stateMachine.FallState);
    //         return;
    //     }
    //     
    //     // 조기 착지 감지 (계단)
    //     if (IsGrounded(1.5f) && rb.velocity.y <= 5.0f)
    //     {
    //         Debug.Log($"JumpState - 계단 감지: y 속도 {rb.velocity.y}");
    //         HandleGroundedState();
    //         return;
    //     }
    // }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate(); // AirState.PhysicsUpdate 호출
    
        Rigidbody rb = stateMachine.Player.Rigidbody;
    
        // 낙하 상태로 전환
        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
        
        Transform t = stateMachine.Player.transform;
        Vector3 rayOrigin = t.position + Vector3.up * 0.1f + t.forward * 0.5f;
        
        // 앞 계단 감지해 조기 착지
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 2.5f, LayerMask.GetMask("Ground")))
        {
            Debug.DrawRay(rayOrigin, Vector3.down * 2.5f, Color.magenta);
    
            float heightDiff = rayOrigin.y - hit.point.y;
            float velY = rb.velocity.y;
    
            if (heightDiff < 1.0f && velY <= 3.0f)
            {
                Debug.LogWarning($"계단 감지 - 높이차: {heightDiff:F2}, 수직속도: {velY:F2}");
                HandleGroundedState();
                return;
            }
        }
    }
    
    //protected override void OnGrabStarted(InputAction.CallbackContext context)
    //{
    //    base.OnGrabStarted(context);
    //    Debug.Log("클릭됨");
    //    stateMachine.ChangeState(stateMachine.GrabState);

    //}
}

