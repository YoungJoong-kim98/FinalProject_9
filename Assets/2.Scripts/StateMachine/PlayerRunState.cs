using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 달리는 상태 (달리기 속도와 애니메이션을 적용)
public class PlayerRunState : PlayerGroundState
{
    private float _currentRunSpeed;         // 달리기 속도
    private Vector2 _lastInputDirection;    // 마지막 입력 방향 저장
    
    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Player.PlayRunDust();
        _currentRunSpeed = stateMachine.CurrentMoveSpeed;   // 이전 속도 가져오기
        if (_currentRunSpeed < stateMachine.MovementSpeed)  // 최소 속도 (MovementSpeed) 보장
        {
            _currentRunSpeed = stateMachine.MovementSpeed;
        }
        stateMachine.CurrentMoveSpeed = _currentRunSpeed; // 갱신
        _lastInputDirection = stateMachine.MovementInput.normalized;        // 초기 방향 저장
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash); // 달리기 애니메이션
        stateMachine.Player.Animator.speed = 0.5f;      // 달리기 시작 시 애니메이션 느리게
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Player.StopRunDust();
        stateMachine.MovementSpeedModifier = 1f; // 기본 속도로 복귀
        stateMachine.Player.Animator.speed = 1f; // 애니메이션 속도 초기화
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }
    
    public override void Update()
    {
        base.Update();

        Vector2 currentInput = stateMachine.MovementInput.normalized;
        
        if (currentInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);   // 이동 입력 없으면 Idle로 전환
            return;
        }
        
        if (Vector2.Dot(currentInput, _lastInputDirection) < -0.5f) // 반대 방향 체크
        {
            stateMachine.ChangeState(stateMachine.WalkState);   // 반대 방향 입력 시 Walk로 전환
            return;
        }
        
        // 가속도 적용
        if (_currentRunSpeed < groundData.RunMaxSpeed)
        {
            _currentRunSpeed += groundData.RunAcceleration * Time.deltaTime;        // 초당 RunAcceleration 만큼 증가
            _currentRunSpeed = Mathf.Min(_currentRunSpeed, groundData.RunMaxSpeed); // 최고 속도 RunMaxSpeed 만큼 제한
        }
        
        stateMachine.MovementSpeedModifier = _currentRunSpeed / stateMachine.MovementSpeed;
        stateMachine.CurrentMoveSpeed = _currentRunSpeed; // 실시간 속도 저장
        
        // Debug.Log($"현재 달리기 속도: {_currentRunSpeed}, 속도 수정자: {stateMachine.MovementSpeedModifier}");
        
        
        stateMachine.Player.Animator.speed = 1f;    // 달리는 중엔 애니메이션 속도 기본값 고정

        _lastInputDirection = currentInput; // 방향 갱신
    }
    
    protected override void OnRunCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.MovementInput != Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.WalkState); // Shift 뗐을 때 걷기 상태로 전환
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdleState); // 입력이 없으면 대기 상태로 전환
        }
    }
}