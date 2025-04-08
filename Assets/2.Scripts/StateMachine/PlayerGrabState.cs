using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabState : PlayerAirState
{
    private bool hasJumped = false;

    public PlayerGrabState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash); // �ӽ� �ִϸ��̼�

        //  Rigidbody �߷� ��ȭ (drag�� ����ϰų�, ���� �߷� ���뵵 ����)
        stateMachine.Player.Rigidbody.velocity = Vector3.zero;
        stateMachine.Player.Rigidbody.useGravity = false;
        stateMachine.Player.Rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        hasJumped = false;
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.GrabParameterHash);

        // �߷� �ٽ� Ȱ��ȭ
        stateMachine.Player.Rigidbody.useGravity = true;

        // ���� ����
        stateMachine.Player.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public override void Update()
    {
        base.Update();

        //  ���콺 ��Ŭ�� ���� �� ��� ����
        if (!Mouse.current.leftButton.isPressed)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }

        //  ���� 1ȸ�� ����
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
