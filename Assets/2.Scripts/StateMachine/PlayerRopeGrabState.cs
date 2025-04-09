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

        // 애니메이션 (임시 Fall 애니메이션 재활용)
        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);

        // 물리 설정
        Rigidbody rb = stateMachine.Player.Rigidbody;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;

        // XZ 고정 (원하면 Y도 고정 가능)
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

        // 좌클릭 해제 시 떨어짐
        if (!Mouse.current.leftButton.isPressed)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }

        // Space 누르면 위로 튕기면서 탈출
        if (!hasJumped && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            hasJumped = true;

            float ropeJumpForce = stateMachine.Player.Data.AirData.JumpForce * 1.0f;

            Rigidbody rb = stateMachine.Player.Rigidbody;
            rb.velocity = Vector3.zero;
            rb.drag = 1.5f;

            // 방향: 위로만 (또는 로프 방향 반대쪽으로)
            Vector3 jumpDirection = Vector3.up * ropeJumpForce;
            rb.AddForce(jumpDirection, ForceMode.Impulse);

            // 이동 잠깐 막고 드래그 초기화
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
