using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // 점프 상태 진입 시 호출
    public override void Enter()
    {
        stateMachine.JumpForce = stateMachine.Player.Data.AirData.JumpForce;    // 점프 힘 설정 (PlayerSO에서 가져옴)
        stateMachine.Player.ForceReceiver.Jump(stateMachine.JumpForce);         // ForceReceiver에 점프 요청

        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);    // 점프 시작
    }

    // 점프 상태에서 나갈 때 호출
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);     // 점프 종료
    }

    // 물리 업데이트
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (stateMachine.Player.Controller.velocity.y <= 0) // 플레이어가 최고점에 도달한 후 내려가기 시작하면 (y 속도가 0 이하)
        {
            stateMachine.ChangeState(stateMachine.FallState);   // 추락 상태로 전환
            return;
        }
    }
}
