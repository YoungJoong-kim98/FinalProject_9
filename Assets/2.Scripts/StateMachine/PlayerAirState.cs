using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAirState : PlayerBaseState
{
    public PlayerAirState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.AirParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AirParameterHash);
    }

    public override void Update()
    {
        base.Update();
        //DebugDrawGrabRay();

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (stateMachine.IsMovementLocked)
        {
            Debug.Log("AirState - 이동 락");
            return;
        }
        Move(GetMovementDirection()); // 공중 제어
    }

    // // 공중에서 속도를 유지 or 추가해 주는 메서드
    // protected override void Move(Vector3 direction)
    // {
    //     if (stateMachine.IsMovementLocked)
    //     {
    //         return; // 이동 금지 중일 땐 처리 안 함
    //     }
    //     Rigidbody rb = stateMachine.Player.Rigidbody;   // 플레이어 Rigidbody 가져옴
    //     Vector3 currentVelocity = rb.velocity;          // 플레이어의 현재 속도 가져옴
    //     Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z); // 수평 속도 계산
    //
    //     // 수평 속도가 기본 속도 미만 = 제자리 점프 or 잡기 후 점프
    //     if (horizontalVelocity.magnitude < stateMachine.MovementSpeed)
    //     {
    //         Vector3 inputDir = direction.normalized;    // WASD 입력 방향
    //         Vector3 airControlForce = inputDir * stateMachine.Player.Data.AirData.AirControlSpeed; // 공중 제어 속도 반영
    //         currentVelocity.x = airControlForce.x;      // x축 갱신
    //         currentVelocity.z = airControlForce.z;      // z축 갱신
    //         rb.velocity = currentVelocity;              // 즉시 적용
    //     }
    //     else // 달리기 점프 등 초기 속도 유지
    //     {
    //         float moveSpeed = stateMachine.CurrentMoveSpeed > stateMachine.MovementSpeed    // 속도 체크 후 조건에 맞게 반영
    //             ? stateMachine.CurrentMoveSpeed // 달리기 속도
    //             : GetMovementSpeed();           // 기본 속도
    //         Vector3 desiredVelocity = direction.normalized * moveSpeed; // WASD 방향에 현재 속도를 곱해 원하는 속도 만들기
    //         desiredVelocity.y = currentVelocity.y;  // y는 중력에 맡김 (수평 속도만 조정)
    //         rb.velocity = Vector3.Lerp(rb.velocity, desiredVelocity, Time.deltaTime * 5f); // 현재 속도 -> 원하는 속도를 부드럽게 보간
    //
    //         // 공중 감속
    //         stateMachine.CurrentMoveSpeed -= Time.deltaTime * 0.2f; // 초당 0.2f 감소
    //         stateMachine.CurrentMoveSpeed = Mathf.Max(stateMachine.CurrentMoveSpeed, stateMachine.MovementSpeed);
    //     }
    // }
    
    // 공중에서 속도 유지 or 추가
    protected override void Move(Vector3 direction)
    {
        if (stateMachine.IsMovementLocked)
            return; // 이동 금지 중일 땐 처리 안 함

        if (IsLowHorizontalSpeed()) // 수평 속도가 기본 이동 속도보다 작은지 확인
        {
            ApplyAirControl(direction); // 방향 입력 적용
        }
        else
        {
            KeepCurrentSpeed(direction);    // 수평 속도가 충분하면 기존 속도 유지
            ApplyAirDeceleration();         // 추가 공중 감속 적용
        }
    }
    
    // 수평 속도 확인
    private bool IsLowHorizontalSpeed()
    {
        Rigidbody rb = stateMachine.Player.Rigidbody;
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        return horizontalVelocity.magnitude < stateMachine.MovementSpeed;
    }
    
    // 공중 제어 (제자리 점프, 잡기 후 점프 시 사용)
    private void ApplyAirControl(Vector3 direction)
    {
        Rigidbody rb = stateMachine.Player.Rigidbody;
        Vector3 inputDir = direction.normalized;
        Vector3 airControlForce = inputDir * stateMachine.Player.Data.AirData.AirControlSpeed;

        Vector3 currentVelocity = rb.velocity;
        currentVelocity.x = airControlForce.x;
        currentVelocity.z = airControlForce.z;

        rb.velocity = currentVelocity;
    }
    
    // 기존 수평 속도를 유지하며 이동 방향만 보간 (달리기 점프 시 사용)
    private void KeepCurrentSpeed(Vector3 direction)
    {
        Rigidbody rb = stateMachine.Player.Rigidbody;
        Vector3 currentVelocity = rb.velocity;

        float moveSpeed = stateMachine.CurrentMoveSpeed > stateMachine.MovementSpeed
            ? stateMachine.CurrentMoveSpeed
            : GetMovementSpeed();

        Vector3 desiredVelocity = direction.normalized * moveSpeed;
        desiredVelocity.y = currentVelocity.y; // y축 속도는 유지

        rb.velocity = Vector3.Lerp(currentVelocity, desiredVelocity, Time.deltaTime * 5f);
    }
    
    // 공중에서 자연스러운 감속
    private void ApplyAirDeceleration()
    {
        stateMachine.CurrentMoveSpeed -= Time.deltaTime * 0.2f;
        stateMachine.CurrentMoveSpeed = Mathf.Max(stateMachine.CurrentMoveSpeed, stateMachine.MovementSpeed);
    }
    
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        if (GameManager.Instance.SkillManager.doubleJump && stateMachine.CanDoubleJump)
        {
            stateMachine.CanDoubleJump = false;
            stateMachine.ChangeState(stateMachine.JumpState, force: true);
        }
    }
    protected override void OnGrabStarted(InputAction.CallbackContext context)
    {
        base.OnGrabStarted(context);
        if (TryGrab())
        {
            stateMachine.ChangeState(stateMachine.GrabState);
        }

    }
    protected bool TryGrab()
    {
        if (!GameManager.Instance.SkillManager.grab)
            return false;

        if (TryDetectGrabTarget(out string tag) && (tag == "Rope" || tag == "Wall"))
        {
            stateMachine.LastGrabTag = tag;  // 마지막 잡은 태그 기억하기
            Debug.Log(tag);
            stateMachine.ChangeState(stateMachine.GrabState);
            return true;
        }

        return false;
    }

    protected bool TryDetectGrabTarget(out string targetTag)
    {
        targetTag = null;

        Transform t = stateMachine.Player.transform;
        Vector3 baseOrigin = t.position + Vector3.up * 1.5f - t.forward * 0.4f;
        float distance = 1.5f;
        float radius = 0.3f;  // ← 기존보다 약간 넓게
        float offset = 0.3f;

        Vector3 diagonalDir = (t.forward + Vector3.up).normalized;
        Vector3 forwardDir = t.forward;

        Vector3[] origins = new Vector3[]
        {
        baseOrigin,
        baseOrigin + t.right * offset,
        baseOrigin - t.right * offset
        };

        foreach (var origin in origins)
        {
            // Rope 감지
            if (Physics.SphereCast(origin, radius, diagonalDir, out RaycastHit hit, distance, LayerMask.GetMask("Rope")))
            {
                Debug.DrawRay(origin, diagonalDir * distance, Color.yellow);
                targetTag = "Rope";

                Vector3 newPosition = hit.point + Vector3.down * 2.0f + hit.normal * 0.3f;

                stateMachine.Player.transform.position = newPosition;
                return true;
            }

            // Wall 감지
            if (Physics.SphereCast(origin, radius, forwardDir, out _, distance, LayerMask.GetMask("Ground")))
            {
                Debug.DrawRay(origin, forwardDir * distance, Color.cyan);
                targetTag = "Wall";
                return true;
            }
        }

        return false;
    }
    
    private void DebugDrawGrabRay()
    {
        Transform t = stateMachine.Player.transform;
        float offset = 0.3f;
        // float rayLength = 1.5f;
        float castDistance = 1.5f;
        Vector3 diagonalDir = (t.forward + Vector3.up).normalized;
        Vector3 forwardDir = t.forward;

        // // 바닥 감지 시각화
        // Vector3 groundOrigin = t.position + Vector3.up * 0.1f;
        // Debug.DrawRay(groundOrigin, Vector3.down * rayLength, Color.yellow);
        // Debug.DrawRay(groundOrigin + t.right * offset, Vector3.down * rayLength, Color.red);
        // Debug.DrawRay(groundOrigin - t.right * offset, Vector3.down * rayLength, Color.red);
        // Debug.DrawRay(groundOrigin + t.forward * offset, Vector3.down * rayLength, Color.red);
        // Debug.DrawRay(groundOrigin - t.forward * offset, Vector3.down * rayLength, Color.red);

        // 벽/로프 감지 시각화 (origin 통일)
        Vector3 baseOrigin = t.position + Vector3.up * 1.5f - t.forward * 0.4f;

        Vector3[] origins = new Vector3[]
        {
        baseOrigin,
        baseOrigin + t.right * offset,
        baseOrigin - t.right * offset
        };

        foreach (var o in origins)
        {
            Debug.DrawRay(o, diagonalDir * castDistance, Color.yellow); // Rope
            Debug.DrawRay(o, forwardDir * castDistance, Color.cyan);    // Wall
        }
    }
}
