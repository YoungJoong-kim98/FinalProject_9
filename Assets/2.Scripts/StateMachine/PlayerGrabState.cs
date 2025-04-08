using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabState : PlayerAirState
{
    private bool hasJumped = false;

    public PlayerGrabState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.GrabParameterHash); // ���� Grab �ִϸ��̼� �߰� ����

        // �߷� ��ȭ
        stateMachine.Player.ForceReceiver.SetGravityScale(0.2f);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.GrabParameterHash);
        stateMachine.Player.ForceReceiver.SetGravityScale(1f); // �߷� �������
    }

    public override void Update()
    {
        base.Update();

        // ��� �Է� ���� �� ���Ϸ� ����
        Debug.Log(!MouseLeftHeld());
        if (!MouseLeftHeld())
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }

        // ���� �� Ż�� (1ȸ�� ���)
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
