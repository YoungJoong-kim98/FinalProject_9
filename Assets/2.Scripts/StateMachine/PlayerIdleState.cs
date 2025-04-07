using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 입력이 없을 때 기본 상태 (입력이 없으면 유지, 입력이 들어오면 Walk로 전환)
public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;    // 속도 0으로 설정
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);    // Idle 애니메이션 시작
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);     // Idle 애니메이션 종료
    }

    public override void Update()
    {
        base.Update();
        
        if(stateMachine.MovementInput != Vector2.zero)  // 이동 입력이 들어오면
        {
            stateMachine.ChangeState(stateMachine.WalkState);   // Walk 상태로 전환
            return;
        }
    }
}