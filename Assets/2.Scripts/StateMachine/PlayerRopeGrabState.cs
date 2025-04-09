using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRopeGrabState : PlayerAirState
{
    private bool hasJumped = false;
    private float slowFallSpeed = -0.5f;

    public PlayerRopeGrabState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        // �ִϸ��̼� (�ӽ� Fall �ִϸ��̼� ��Ȱ��)
        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);

        // ���� ����
        Rigidbody rb = stateMachine.Player.Rigidbody;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;

        // XZ ���� (���ϸ� Y�� ���� ����)
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        hasJumped = false;
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.GrabParameterHash);

        Rigidbody rb = stateMachine.Player.Rigidbody;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public override void Update()
    {
        base.Update();

        // ��Ŭ�� ���� �� ������
        if (!Mouse.current.leftButton.isPressed)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }

        // Space ������ ���� ƨ��鼭 Ż��
        if (!hasJumped && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            hasJumped = true;

            float ropeJumpForce = stateMachine.Player.Data.AirData.JumpForce * 1.0f;

            Rigidbody rb = stateMachine.Player.Rigidbody;
            rb.velocity = Vector3.zero;
            rb.drag = 1.5f;

            // ����: ���θ� (�Ǵ� ���� ���� �ݴ�������)
            Vector3 jumpDirection = Vector3.up * ropeJumpForce;
            rb.AddForce(jumpDirection, ForceMode.Impulse);

            // �̵� ��� ���� �巡�� �ʱ�ȭ
            stateMachine.IsMovementLocked = true;
            stateMachine.Player.StartCoroutine(UnlockMovementAfterDelay(0.5f));

            stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    private IEnumerator UnlockMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        stateMachine.IsMovementLocked = false;
        stateMachine.Player.Rigidbody.drag = 0f;
    }
}
