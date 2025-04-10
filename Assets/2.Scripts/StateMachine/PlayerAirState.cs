using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
    public PlayerAirState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.AirParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AirParameterHash);
    }
    
    public override void Update()
    {
        base.Update();
    }
    
    // 공중에서 달리기 속도를 유지해 주는 메서드 (중력, 저항 등의 문제로 속도가 줄어드는 것을 방지)
    protected override void Move(Vector3 direction)
    {
        Rigidbody rb = stateMachine.Player.Rigidbody;   // 플레이어 Rigidbody 가져옴
        Vector3 currentVelocity = rb.velocity;          // 플레이어의 현재 속도 확인
        float moveSpeed = stateMachine.CurrentMoveSpeed > stateMachine.MovementSpeed // 달리기 중이면 5f, 아니면 2f
            ? stateMachine.CurrentMoveSpeed
            : GetMovementSpeed();
        Vector3 desiredVelocity = direction.normalized * moveSpeed; // WASD 방향에 현재 속도를 곱해 원하는 속도 만들기
        desiredVelocity.y = currentVelocity.y;          // y 속도는 중력에 맡김 (수평 속도만 조정)
        rb.velocity = Vector3.Lerp(rb.velocity, desiredVelocity, Time.deltaTime * 5f); // 현재 속도 -> 원하는 속도를 부드럽게 보간
        
        // 공중 감속
        stateMachine.CurrentMoveSpeed -= Time.deltaTime * 0.2f; // 초당 0.2f 감소
        stateMachine.CurrentMoveSpeed = Mathf.Max(stateMachine.CurrentMoveSpeed, stateMachine.MovementSpeed);
    }
}
