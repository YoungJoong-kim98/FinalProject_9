using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // 추락 상태에 진입할 때 호출
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);    // 추락 시작
    }

    // 추락 상태 끝날 때 호출
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);     // 추락 종료
    }
    
    // 논리 업데이트
    public override void Update()
    {
        base.Update();

        if (stateMachine.Player.Controller.isGrounded)  // 플레이어가 바닥에 착지하면
        {
            stateMachine.ChangeState(stateMachine.IdleState);   // 대기 상태로 전환
            return;
        }
    }
}
