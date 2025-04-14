using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandUpState : PlayerBaseState
{
    private float _standUpDuration = 3f; // StandUp 모션 길이
    private float _standUpTimer;
    
    public PlayerStandUpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        _standUpTimer = 0f;
        stateMachine.IsMovementLocked = true;
        StartAnimation(stateMachine.Player.AnimationData.StandUpParameterHash);
        Debug.Log("Entered StandUpState");
    }

    public override void Update()
    {
        base.Update();
        _standUpTimer += Time.deltaTime;
        if (_standUpTimer >= _standUpDuration)
        {
            stateMachine.IsMovementLocked = false;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.StandUpParameterHash);
    }
}
