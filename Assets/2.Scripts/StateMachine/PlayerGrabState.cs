using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.LightAnchor;

public class PlayerGrabState : PlayerAirState
{
    private bool hasJumped = false;
    private float slowFallSpeed = -0.2f;
    public PlayerGrabState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // �� ��� �̹� ������ �� ���� �ϱ�
        if (!stateMachine.CanGrabWall)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }

        base.Enter();
        stateMachine.CanGrabWall = false;

        stateMachine.Player.StartCoroutine(EnableWallGrabAfterCooldown(0.5f)); // 1�� �� �ٽ� ����

        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);

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
        // �������� ���� õõ�� ����
        Vector3 velocity = stateMachine.Player.Rigidbody.velocity;

        // y�ӵ��� ������ ������ ���� (������ �ƴ� "���� �ӵ� ����")
        velocity.y = slowFallSpeed;
        stateMachine.Player.Rigidbody.velocity = velocity;
        //  ���콺 ��Ŭ�� ���� �� ��� ����
        if (!Mouse.current.leftButton.isPressed)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }

        if (!hasJumped && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            stateMachine.IsMovementLocked = true;

            float jumpPower = stateMachine.Player.Data.AirData.JumpForce * 1.2f;
            float directionalForce = 10.0f;

            // ����Ű �Է� �� ī�޶� ���� �������� ��ȯ
            Vector2 input = stateMachine.MovementInput;
            Vector3 camForward = stateMachine.MainCameraTransform.forward;
            Vector3 camRight = stateMachine.MainCameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 inputDir = camForward * input.y + camRight * input.x;

            // �Է��� ������ ���� �ٶ󺸴� ��������
            if (inputDir == Vector3.zero)
            {
                inputDir = -stateMachine.Player.transform.forward;
            }

            Vector3 jumpDirection = inputDir.normalized * directionalForce + Vector3.up * jumpPower;

            Rigidbody rb = stateMachine.Player.Rigidbody;
            rb.velocity = Vector3.zero;
            rb.drag = 1.5f;
            rb.AddForce(jumpDirection, ForceMode.Impulse);

            hasJumped = true;

            stateMachine.Player.StartCoroutine(UnlockMovementAfterDelay(1f));
            stateMachine.ChangeState(stateMachine.FallState);
        }

    }
    private IEnumerator UnlockMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        stateMachine.IsMovementLocked = false;
        stateMachine.Player.Rigidbody.drag = 0f;
    }

    private IEnumerator EnableWallGrabAfterCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        stateMachine.CanGrabWall = true;
    }

}
