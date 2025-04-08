using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        Vector3 moveDir = GetMovementDirection().normalized;    // WASD �Է� ���� (ī�޶� ����)

        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;  // ���� �ӵ� (�޸��� �ݿ�)
        float jumpForce = stateMachine.Player.Data.AirData.JumpForce;   // ���� ��

        Rigidbody rb = stateMachine.Player.Rigidbody;

        // // ���� �ۼ� �κ�
        // // ���� Y �ӵ� �ʱ�ȭ
        // Vector3 currentVelocity = rb.velocity;
        // currentVelocity.y = 0f;
        // rb.velocity = currentVelocity;
        //
        // //  �̵� �������� ���ӵ� �ֱ� (Impulse)
        // Vector3 jumpDirection = moveDir * moveSpeed + Vector3.up * jumpForce;
        // rb.AddForce(jumpDirection, ForceMode.Impulse);  // �� �̰ɷ� "�ָ� ����" ���� ����
        
        // ���� ���� �ӵ� �������� (�޸��� �ӵ� ����)
        Vector3 currentVelocity = rb.velocity;
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);

        // ���� �ӵ� = ���� ���� �ӵ� + ���� ���� �� + �ణ�� ���� ����
        Vector3 jumpVelocity = horizontalVelocity + (Vector3.up * jumpForce);
        jumpVelocity += moveDir * moveSpeed * 0.5f; // �̵� �������� �޸��� ���� ��ȭ

        rb.velocity = jumpVelocity; // �ӵ� ����

        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.Update();

        if (stateMachine.Player.Rigidbody.velocity.y <= 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}

