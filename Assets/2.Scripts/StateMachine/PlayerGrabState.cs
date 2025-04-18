using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.LightAnchor;
using static UnityEngine.UI.Image;

public class PlayerGrabState : PlayerAirState
{
    private bool hasJumped = false; // ���� ����
    private float slowFallSpeed = -0.5f;    // ��� �� ���� ���� �ӵ�
    public PlayerGrabState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private Coroutine unlockCoroutine; // �̵� ���� ����� �ڷ�ƾ
    private Coroutine wallCooldownCoroutine; //�� ��� ���� ���� ����� �ڷ�ƾ

    private readonly WaitForSeconds move_unlockTime = new WaitForSeconds(1f);
    private readonly WaitForSeconds wall_unlockTime = new WaitForSeconds(1f);
    private Coroutine grabTimeoutCoroutine; //�Ͻ����� ��� �� �׽�Ʈ
    private readonly WaitForSeconds grabLimit = new WaitForSeconds(3f);

    public override void Enter()
    {
        // �� ��� �̹� ������ �� ���� �ϱ�
        if (!stateMachine.CanGrabWall)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
        stateMachine.CanDoubleJump = false; // �����ϰ� ���� �ٷ� �������� �� �ϰ� ����
        //stateMachine.CanGrabWall = false;
        //if(wallCooldownCoroutine != null)
        //{
        //    stateMachine.Player.StopCoroutine(wallCooldownCoroutine);    
        //}
        //wallCooldownCoroutine = stateMachine.Player.StartCoroutine(EnableWallGrabAfterCooldown(2f)); //1�� �� �ٽð���
        base.Enter();

        GameManager.Instance.AchievementSystem.GrabCount(); // ��� Ƚ�� ����
        StartAnimation(stateMachine.Player.AnimationData.GrabParameterHash);

        // ���� ����
        stateMachine.Player.Rigidbody.velocity = Vector3.zero;  // �ӵ� 0����
        stateMachine.Player.Rigidbody.useGravity = false;       // �߷� ��Ȱ��ȭ
        stateMachine.Player.Rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ; // x, z �̵� ����

        hasJumped = false;  // ���� ���� �ʱ�ȭ
        //grabTimeoutCoroutine = stateMachine.Player.StartCoroutine(GrabTimeout()); // �׽�Ʈ��
    }


    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.GrabParameterHash);

        // �߷� �ٽ� Ȱ��ȭ
        stateMachine.Player.Rigidbody.useGravity = true;

        // ���� ����
        stateMachine.Player.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        if (grabTimeoutCoroutine != null)
        {
            stateMachine.Player.StopCoroutine(grabTimeoutCoroutine);
            grabTimeoutCoroutine = null;
        }
    }

    // ��� ����/����
    public override void Update()
    {
        base.Update();
        //��� �پ��ִ��� Ȯ��
        //if (!IsStillGrabbing())
        //{
        //    stateMachine.ChangeState(stateMachine.FallState);
        //    return;
        //}

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

        // ���� �Է�
        if (!hasJumped && Keyboard.current.spaceKey.wasPressedThisFrame)
        {

            stateMachine.IsMovementLocked = true; // �̵� ���
            stateMachine.CanGrabWall = false; //��� ��� 


            float jumpPower = stateMachine.Player.Data.AirData.JumpForce * 1.5f;
            float directionalForce = 20.0f;

            // ����Ű �Է� �� ī�޶� ���� �������� ��ȯ
            Vector2 input = stateMachine.MovementInput;
            Vector3 camForward = stateMachine.MainCameraTransform.forward;
            Vector3 camRight = stateMachine.MainCameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 inputDir = camForward * input.y + camRight * input.x;


            Vector3 jumpDirection = inputDir.normalized * directionalForce + Vector3.up * jumpPower;

            Rigidbody rb = stateMachine.Player.Rigidbody;
            rb.constraints = RigidbodyConstraints.FreezeRotation; //  XZ ���� ����!
            rb.velocity = Vector3.zero;
            rb.drag = 1.5f;
            rb.AddForce(jumpDirection, ForceMode.Impulse);

            hasJumped = true;

            if (unlockCoroutine != null) //�ڷ�ƾ �ߺ� ���� ����ó��
            {
                stateMachine.Player.StopCoroutine(unlockCoroutine);
            }
            if (wallCooldownCoroutine != null)
            {
                stateMachine.Player.StopCoroutine(wallCooldownCoroutine);
            }
            unlockCoroutine = stateMachine.Player.StartCoroutine(UnlockMovementAfterDelay()); // �̵� ��� ���� �ڷ�ƾ
            wallCooldownCoroutine = stateMachine.Player.StartCoroutine(EnableWallGrabAfterCooldown()); //��� ��� ���� �ڷ�ƾ

            stateMachine.ChangeState(stateMachine.FallState);

        }

    }

    private IEnumerator UnlockMovementAfterDelay()
    {
        yield return move_unlockTime;
        stateMachine.IsMovementLocked = false;
        stateMachine.Player.Rigidbody.drag = 0f;
    }

    private IEnumerator EnableWallGrabAfterCooldown()
    {
        yield return wall_unlockTime;
        stateMachine.CanGrabWall = true;
    }

    private bool IsStillGrabbing()
    {
        Transform t = stateMachine.Player.transform;
        Vector3 origin = t.position + Vector3.up * 2.0f; // �� ��ġ ����
        float distance = 1.5f;                            // �� �Ÿ� ����
        float radius = 0.2f;                              // �� ���� �������� ���̴� �� ��õ

        Vector3 diagonalDir = (t.forward + Vector3.up).normalized;

        // �� �Ǵ� ������ ���� �������� �Ǵ�
        bool nearRope = Physics.SphereCast(origin, radius, diagonalDir, out _, distance, LayerMask.GetMask("Rope"));
        //bool nearRope_2 = Physics.Raycast(origin, diagonalDir, distance, LayerMask.GetMask("Rope")); // ����ĳ��Ʈ��
        bool nearWall = Physics.Raycast(origin, t.forward, distance, LayerMask.GetMask("Ground"));

        // �ð�ȭ (��ġ��Ŵ)
        Debug.DrawRay(origin, diagonalDir * distance, Color.cyan); // Rope ����
        Debug.DrawRay(origin, t.forward * distance, Color.cyan);   // Wall ����

        return nearRope;
    }
    private IEnumerator GrabTimeout()
    {
        yield return grabLimit;

        // �ð��� ������ ���� ��ȯ
        if (stateMachine.CurrentState is PlayerGrabState)
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }

    }

}