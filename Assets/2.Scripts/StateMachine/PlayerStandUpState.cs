using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandUpState : PlayerBaseState
{
    private float _standUpDuration = 1f; // StandUp 모션 길이
    private float _standUpTimer;
    
    public PlayerStandUpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        _standUpTimer = 0f;
        stateMachine.Player.Rigidbody.isKinematic = true;
        stateMachine.IsMovementLocked = true;   // 이동 입력 잠금
        stateMachine.MovementInput = Vector2.zero; // 입력 초기화
        StartAnimation(stateMachine.Player.AnimationData.StandUpParameterHash);
        Debug.Log("StandUp 상태 진입");
    }

    public override void Update()
    {
        base.Update();
        _standUpTimer += Time.deltaTime;
        if (_standUpTimer >= _standUpDuration)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!stateMachine.Player.Rigidbody.isKinematic)
        {
            // 이동 차단
            stateMachine.Player.Rigidbody.velocity = new Vector3(0, stateMachine.Player.Rigidbody.velocity.y, 0);
            // Debug.Log("StandUp - 이동 차단");
        }
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Player.Rigidbody.isKinematic = false;
        stateMachine.IsMovementLocked = false;  // 이동 잠금 해제
        stateMachine.MovementInput = Vector2.zero; // 입력 초기화
        StopAnimation(stateMachine.Player.AnimationData.StandUpParameterHash);
        Debug.Log("StandUpState 종료");
    }
}
