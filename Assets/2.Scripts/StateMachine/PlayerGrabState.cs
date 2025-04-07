using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabState : PlayerAirState
{
    private bool hasJumped = false;

    public PlayerGrabState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.GrabParameterHash); // 추후 Grab 애니메이션 추가 가능

        // 중력 약화
        stateMachine.Player.ForceReceiver.SetGravityScale(0.2f);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.GrabParameterHash);
        stateMachine.Player.ForceReceiver.SetGravityScale(1f); // 중력 원래대로
    }

    public override void Update()
    {
        base.Update();

        // 잡기 입력 해제 시 낙하로 복귀
        Debug.Log(!MouseLeftHeld());
        if (!MouseLeftHeld())
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }

        // 점프 시 탈출 (1회만 허용)
        if (!hasJumped && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            float reducedJumpForce = stateMachine.Player.Data.AirData.JumpForce * 0.5f;
            stateMachine.Player.ForceReceiver.Jump(reducedJumpForce);
            hasJumped = true;
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    private bool MouseLeftHeld()
    {
        return Mouse.current.leftButton.isPressed;
    }
}
