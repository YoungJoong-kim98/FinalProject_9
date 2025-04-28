using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallState : PlayerAirState
{
    private float _fallSpeed;       // 초당 가속도
    private float _maxFallSpeed;    // 최대 낙하 속도 제한
    private bool _wasGrounded;      // 이전 프레임 착지 여부 (연속 착지 방지)
    private readonly string[] validGrabTags = { "Rope", "Wall" };   // 잡을 수 있는 대상
    
    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        _fallSpeed = stateMachine.Player.Data.AirData.FallSpeed;
        _maxFallSpeed = stateMachine.Player.Data.AirData.MaxFallSpeed;
    }

    public override void Enter()
    {
        base.Enter();
        _wasGrounded = false;  // 착지 플래그 초기화
        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);
        Debug.Log("Fall 상태 진입");
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    // // 낙하 로직 처리(수정 전)
    // public override void Update()
    // {
    //     base.Update();
    //     
    //     Rigidbody rb = stateMachine.Player.Rigidbody;   // 플레이어 물리 가져옴
    //     Vector3 velocity = rb.velocity; // 현재 속도 가져오기
    //     
    //     // 낙하 속도 계산
    //     velocity.y -= _fallSpeed * Time.deltaTime;  // y 속도 더하기
    //     velocity.y = Mathf.Max(velocity.y, -_maxFallSpeed); // 최대 속도 제한
    //     rb.velocity = velocity; // 속도 적용
    //     
    //     float savedVelocity = velocity.y; // 착지 전 속도 저장
    //     Debug.Log($"Fall - y 속도: {velocity.y}");
    //
    //     // 착지 확인
    //     bool isGrounded = IsGrounded(1.0f, useOffset: true); // 바닥 감지
    //     if (isGrounded && !_wasGrounded) // 첫 착지 프레임만 처리
    //     {
    //         Debug.Log($"착지 - 저장 속도: {savedVelocity}");
    //         _wasGrounded = true; // 다음 프레임 대비
    //         
    //         if (savedVelocity <= -_maxFallSpeed) // 최고 속도에 도달하면 FallCrash   
    //         {
    //             Debug.Log("철푸덕");
    //             stateMachine.ChangeState(stateMachine.FallCrashState);
    //             return;
    //         }
    //         else
    //         {
    //             Debug.Log("정상 착지");
    //             HandleGroundedState();
    //             return;
    //         }
    //     }
    // }
    
    public override void Update()
    {
        base.Update();
    }
    
    // 낙하 로직 처리
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();   // AirState.PhysicsUpdate 호출
        
        Rigidbody rb = stateMachine.Player.Rigidbody;
        Vector3 velocity = rb.velocity; // 현재 속도 가져오기
        
        float clampVelY = Mathf.Clamp(velocity.y, -_maxFallSpeed, float.PositiveInfinity);

        // 착지 감지
        bool isGrounded = IsGrounded(1.0f, useOffset: true);    // 바닥 감지
        if (isGrounded && !_wasGrounded)    // 첫 착지 프레임만 처리
        {
            Debug.Log($"착지 - 저장 속도: {clampVelY}");
            _wasGrounded = true;

            if (clampVelY <= -_maxFallSpeed)   // 최고 속도에 도달하면 FallCrash
            {
                Debug.Log("철푸덕");
                stateMachine.ChangeState(stateMachine.FallCrashState);
                return;
            }
            else
            {
                Debug.Log("정상 착지");
                HandleGroundedState();
                return;
            }
        }

        // 착지 아니면 낙하 속도 계산
        velocity.y -= _fallSpeed * Time.fixedDeltaTime; // y 속도 더하기
        velocity.y = Mathf.Clamp(velocity.y, -_maxFallSpeed, float.PositiveInfinity);   // 최대 속도 제한

        rb.velocity = velocity; // 속도 적용

        // Debug.Log($"Fall - y 속도: {velocity.y}");
    }
    
    /// <summary>
    /// 떨어지는 상태시 정면 , 위 , 땅과 로프 확인 로직 값 수정시 GrabState스크립트 IsStillGrabbing 함수도 같이 수정 바람!
    /// </summary>
    /// <param name="targetTag"></param>
    /// <returns></returns>

    
    // 레이 시각화
    private void DebugDrawGrabRay()
    {
        Transform t = stateMachine.Player.transform;

        // 로프 및 벽 감지용 Ray 시각화
        Vector3 origin2 = t.position + Vector3.up * 2.0f;
        float castDistance = 1.5f;
        Vector3 diagonalDir = (t.forward + Vector3.up).normalized;

        Debug.DrawRay(origin2, diagonalDir * castDistance, Color.blue); // 로프 감지용 Ray
        Debug.DrawRay(origin2, t.forward * castDistance, Color.blue);   // 벽 감지용 Ray
    }
}
