using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 달리는 상태 (달리기 속도와 애니메이션을 적용)
public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;   // 달리기 속도 설정
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash); // 달리기 애니메이션
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
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