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
        stateMachine.IsMovementLocked = true; // 이동 차단
        
        // 3프레임 기다리고 땅에 붙이기
        stateMachine.Player.StartCoroutine(AlignToGroundAfterFrames(3));
        
        // 속도 및 중력 제어
        Rigidbody rb = stateMachine.Player.Rigidbody;
        rb.velocity = Vector3.zero; 
        rb.useGravity = false; // 중력 끄기
        stateMachine.MovementInput = Vector2.zero;  // 이동 입력 초기화
    
        StartAnimation(stateMachine.Player.AnimationData.FallCrashParameterHash);
        Debug.Log("FallCrash 상태 진입");
    }

    private IEnumerator AlignToGroundAfterFrames(int frameCount)
    {
        for (int i = 0; i < frameCount; i++)
            yield return null;

        AlignToGround();
    }
    
    // 바닥에 붙여주기
    private void AlignToGround()
    {
        Transform t = stateMachine.Player.transform;
        if (Physics.Raycast(t.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, 2f, LayerMask.GetMask("Ground")))
        {
            t.position = hit.point + Vector3.up * 0.1f;
            Debug.Log($"FallCrash - 바닥 위치 보정: {hit.point}, 직전 높이: {hit.distance}");
        }
    }
    
    public override void Update()
    {
        base.Update();
        _crashTimer += Time.deltaTime;
        if (_crashTimer >= _crashDuration)
        {
            stateMachine.ChangeState(stateMachine.StandUpState);
        }
    }
    
    // 수평 이동 차단
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Rigidbody rb = stateMachine.Player.Rigidbody;
        rb.velocity = Vector3.zero; // 속도 차단
    }
    
    public override void Exit()
    {
        base.Exit();
        stateMachine.Player.Rigidbody.useGravity = true; // 중력 복구
        StopAnimation(stateMachine.Player.AnimationData.FallCrashParameterHash);
        Debug.Log($"FallCrashState 종료");
    }
}
