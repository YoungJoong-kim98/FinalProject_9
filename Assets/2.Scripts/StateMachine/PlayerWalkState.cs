using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 기본 이동 상태 (Run 키 입력되면 Run으로 전이. 걷기 속도와 애니메이션을 적용, 입력에 따라 상태 전환)
public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;      // 걷기 속도 설정
        base.Enter();
        stateMachine.CurrentMoveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);    // 걷기 애니메이션
    }
    
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        base.OnRunStarted(context);
        if(GameManager.Instance.SkillManager.run)
        {
            stateMachine.ChangeState(stateMachine.RunState);    // 달리기 입력 시 Run으로 전환
        }
    }
}