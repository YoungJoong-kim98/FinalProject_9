using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallState : PlayerAirState
{
    private float _fallSpeed = 1f;  // 추락 속도 초당 1f 증가
    private float _maxFallSpeed = 15f;  // 최대 낙하 속도 제한
    private float _fallTime;    // 낙하 시간 측정
    
    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        _fallSpeed = stateMachine.Player.Data.AirData.FallSpeed;
        _maxFallSpeed = stateMachine.Player.Data.AirData.MaxFallSpeed;
    }

    public override void Enter()
    {
        base.Enter();
        _fallTime = 0f;
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
        
        if (!IsGrounded())  // 추락 가속도 적용
        {
            _fallTime += Time.deltaTime; // 낙하 시간 누적
            Rigidbody rb = stateMachine.Player.Rigidbody;
            Vector3 velocity = rb.velocity;
            velocity.y -= _fallSpeed * Time.deltaTime;  // 추락 속도 증가
            velocity.y = Mathf.Max(velocity.y, -_maxFallSpeed); // 최대 속도 설정
            rb.velocity = velocity; // 수평 속도는 AirState에서 관리
        }
        
        if (IsGrounded()) // Raycast로 착지 확인
        {
            Debug.Log($"낙하 시간: {_fallTime}초"); // 착지 시 낙하 시간 출력
            
            float preservedSpeed = stateMachine.CurrentMoveSpeed; // 착지 전 속도 저장
            
            if (stateMachine.Player.Input.playerActions.Run.IsPressed()) // Shift 누르고 있으면
            {
                stateMachine.CurrentMoveSpeed = preservedSpeed; // 감소된 속도 사용
                stateMachine.ChangeState(stateMachine.RunState);
                Debug.Log($"Fall to Run - 현재 이동 속도: {stateMachine.CurrentMoveSpeed}");
            }
            else if (stateMachine.MovementInput != Vector2.zero) // 이동 입력 있으면
            {
                stateMachine.CurrentMoveSpeed = stateMachine.MovementSpeed; // 2f
                stateMachine.ChangeState(stateMachine.WalkState);
            }
            else // 입력 없으면
            {
                stateMachine.CurrentMoveSpeed = stateMachine.MovementSpeed; // 2f
                stateMachine.ChangeState(stateMachine.IdleState);
            }
            return;
        }
        
        if (Mouse.current.leftButton.wasPressedThisFrame && IsNearGrabbableWall()) // 공중 잡기
        {
            stateMachine.ChangeState(stateMachine.GrabState);
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
        Vector3 direction = t.forward;
        float distance = 1f;


        // Raycast
        return Physics.Raycast(origin, direction, distance, LayerMask.GetMask("Ground"));
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