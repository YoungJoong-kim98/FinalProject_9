using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallCrashState : PlayerBaseState
{
    private float _crashDuration = 1f;  // 쓰러진 상태 유지 시간
    private float _crashTimer;          // _crashDuration 추적
    
    public PlayerFallCrashState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        _crashTimer = 0f;   // 타이머 초기화
        stateMachine.IsMovementLocked = true; // 이동 입력 잠금
        
        // 바닥에 붙이기
        Transform t = stateMachine.Player.transform;
        if (Physics.Raycast(t.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, 2f, LayerMask.GetMask("Ground")))
        {
            t.position = hit.point + Vector3.up * 0.1f;
            Debug.Log($"Snapped to ground at: {hit.point}");
        }
        
        // 속도 및 중력 제어
        Rigidbody rb = stateMachine.Player.Rigidbody;
        rb.velocity = Vector3.zero;
        rb.useGravity = false; // 중력 끄기
        stateMachine.MovementInput = Vector2.zero;

        StartAnimation(stateMachine.Player.AnimationData.FallCrashParameterHash);
        Debug.Log("FallCrashState 상태 확인");
    }
    
    public override void Update()
    {
        base.Update();
        _crashTimer += Time.deltaTime;
        if (_crashTimer >= _crashDuration)  // 타이머가 1초 이상이면
        {
            stateMachine.IsMovementLocked = false;  // 이동 잠금 해제
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    
    // 수평 이동 차단
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        // 모든 속도 차단
        Rigidbody rb = stateMachine.Player.Rigidbody;
        rb.velocity = Vector3.zero;
        Debug.Log($"FallCrash - Velocity: {rb.velocity}");
    }
    
    public override void Exit()
    {
        base.Exit();
        stateMachine.Player.Rigidbody.useGravity = true; // 중력 복구
        StopAnimation(stateMachine.Player.AnimationData.FallCrashParameterHash);
        Debug.Log($"FallCrashState 종료, FallCrash Param: {stateMachine.Player.Animator.GetBool(stateMachine.Player.AnimationData.FallCrashParameterHash)}");
    }
}
