using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabState : PlayerAirState
{
    private bool hasJumped = false;

    public PlayerGrabState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash); // 임시 애니메이션

        //  Rigidbody 중력 약화 (drag를 사용하거나, 수동 중력 적용도 가능)
        stateMachine.Player.Rigidbody.velocity = Vector3.zero;
        stateMachine.Player.Rigidbody.useGravity = false;
        stateMachine.Player.Rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        hasJumped = false;
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.GrabParameterHash);

        // 중력 다시 활성화
        stateMachine.Player.Rigidbody.useGravity = true;

        // 고정 해제
        stateMachine.Player.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public override void Update()
    {
        base.Update();

        //  마우스 좌클릭 해제 시 잡기 종료
        if (!Mouse.current.leftButton.isPressed)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }

        //  점프 1회만 가능
        if (!hasJumped && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            float reducedJumpForce = stateMachine.Player.Data.AirData.JumpForce * 2.0f;
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
