using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.LightAnchor;
using static UnityEngine.UI.Image;

public class PlayerGrabState : PlayerAirState
{
    private bool hasJumped = false;
    private float slowFallSpeed = -0.1f;
    public PlayerGrabState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private Coroutine unlockCoroutine; // 이동 상태 만드는 코루틴
    private Coroutine wallCooldownCoroutine; //벽 잡기 가능 상태 만드는 코루틴

    private readonly WaitForSeconds move_unlockTime = new WaitForSeconds(1f);
    private readonly WaitForSeconds wall_unlockTime = new WaitForSeconds(1f);
    public override void Enter()
    {
        // 벽 잡기 이미 했으면 안 들어가게 하기
        if (!stateMachine.CanGrabWall)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
        //stateMachine.CanGrabWall = false;
        //if(wallCooldownCoroutine != null)
        //{
        //    stateMachine.Player.StopCoroutine(wallCooldownCoroutine);    
        //}
        //wallCooldownCoroutine = stateMachine.Player.StartCoroutine(EnableWallGrabAfterCooldown(2f)); //1초 후 다시가능
        base.Enter();


        StartAnimation(stateMachine.Player.AnimationData.GrabParameterHash);

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
        // 계속 붙어있는지 확인
        if (!IsStillGrabbing())
        {
           stateMachine.ChangeState(stateMachine.FallState);
            return;
        }

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

            stateMachine.IsMovementLocked = true; // 이동 잠금
            stateMachine.CanGrabWall = false; //잡기 잠금 

            float jumpPower = stateMachine.Player.Data.AirData.JumpForce * 0.8f;
            float directionalForce = 20.0f;

            // 방향키 입력 → 카메라 기준 방향으로 변환
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
            rb.constraints = RigidbodyConstraints.FreezeRotation; //  XZ 고정 해제!
            rb.velocity = Vector3.zero;
            rb.drag = 1.5f;
            rb.AddForce(jumpDirection, ForceMode.Impulse);

            hasJumped = true;

            if (unlockCoroutine != null) //코루틴 중복 방지 예외처리
            {
                stateMachine.Player.StopCoroutine(unlockCoroutine);
            }
            if (wallCooldownCoroutine != null)
            {
                stateMachine.Player.StopCoroutine(wallCooldownCoroutine);
            }
            unlockCoroutine = stateMachine.Player.StartCoroutine(UnlockMovementAfterDelay()); // 이동 잠금 해제 코루틴
            wallCooldownCoroutine = stateMachine.Player.StartCoroutine(EnableWallGrabAfterCooldown()); //잡기 잠금 해제 코루틴

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
        Vector3 origin = t.position + Vector3.up * 2.0f; // ← 위치 맞춤
        float distance = 1.0f;                            // ← 거리 맞춤
        float radius = 0.2f;                              // ← 감지 반지름은 줄이는 걸 추천

        Vector3 diagonalDir = (t.forward + Vector3.up).normalized;

        // 벽 또는 로프를 유지 조건으로 판단
        bool nearRope = Physics.SphereCast(origin, radius, diagonalDir, out _, distance, LayerMask.GetMask("Rope"));
        //bool nearRope_2 = Physics.Raycast(origin, diagonalDir, distance, LayerMask.GetMask("Rope")); // 레이캐스트용
        bool nearWall = Physics.Raycast(origin, t.forward, distance, LayerMask.GetMask("Ground"));

        // 시각화 (일치시킴)
        Debug.DrawRay(origin, diagonalDir * distance, Color.cyan); // Rope 감지
        Debug.DrawRay(origin, t.forward * distance, Color.cyan);   // Wall 감지

        return nearWall || nearRope;
    }


}