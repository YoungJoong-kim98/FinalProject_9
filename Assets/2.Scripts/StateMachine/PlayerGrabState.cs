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
        // 벽 잡기 이미 했으면 안 들어가게 하기
        if (!stateMachine.CanGrabWall)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }

        base.Enter();
        stateMachine.CanGrabWall = false;

        stateMachine.Player.StartCoroutine(EnableWallGrabAfterCooldown(0.5f)); // 1초 후 다시 가능

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

        // 중력 다시 활성화
        stateMachine.Player.Rigidbody.useGravity = true;

        // 고정 해제
        stateMachine.Player.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public override void Update()
    {
        base.Update();
        // 수동으로 아주 천천히 낙하
        Vector3 velocity = stateMachine.Player.Rigidbody.velocity;

        // y속도는 일정한 값으로 유지 (감속이 아닌 "고정 속도 낙하")
        velocity.y = slowFallSpeed;
        stateMachine.Player.Rigidbody.velocity = velocity;
        //  마우스 좌클릭 해제 시 잡기 종료
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

            // 방향키 입력 → 카메라 기준 방향으로 변환
            Vector2 input = stateMachine.MovementInput;
            Vector3 camForward = stateMachine.MainCameraTransform.forward;
            Vector3 camRight = stateMachine.MainCameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 inputDir = camForward * input.y + camRight * input.x;

            // 입력이 없으면 현재 바라보는 방향으로
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
