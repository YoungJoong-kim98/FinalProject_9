using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallState : PlayerAirState
{
    private float _fallSpeed;       // 추락 속도 증가 값
    private float _maxFallSpeed;    // 최대 낙하 속도 제한
    private float _fallTime;        // 낙하 시간 측정

    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        _fallSpeed = stateMachine.Player.Data.AirData.FallSpeed;
        _maxFallSpeed = stateMachine.Player.Data.AirData.MaxFallSpeed;
    }

    public override void Enter()
    {
        base.Enter();
        _fallTime = 0f;
        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    public override void Update()
    {
        base.Update();
        DebugDrawGrabRay();
        if (!IsGrounded())  // 추락 가속도 적용
        {
            _fallTime += Time.deltaTime; // 낙하 시간 누적
            Rigidbody rb = stateMachine.Player.Rigidbody;
            Vector3 velocity = rb.velocity;
            velocity.y -= _fallSpeed * Time.deltaTime;  // 추락 속도 증가
            velocity.y = Mathf.Max(velocity.y, -_maxFallSpeed); // 최대 속도 설정
            rb.velocity = velocity; // 수평 속도는 AirState에서 관리
        }

        if (IsGrounded()) // Raycast로 착지 확인
        {
            // Debug.Log($"낙하 시간: {_fallTime}초"); // 착지 시 낙하 시간 출력

            float preservedSpeed = stateMachine.CurrentMoveSpeed; // 착지 전 속도 저장

            if (stateMachine.Player.Input.playerActions.Run.IsPressed()) // Shift 누르고 있으면
            {
                stateMachine.CurrentMoveSpeed = preservedSpeed; // 감소된 속도 사용
                stateMachine.ChangeState(stateMachine.RunState);
                Debug.Log($"Fall to Run - 현재 이동 속도: {stateMachine.CurrentMoveSpeed}");
            }
            else if (stateMachine.MovementInput != Vector2.zero) // 이동 입력 있으면
            {
                stateMachine.CurrentMoveSpeed = stateMachine.MovementSpeed;
                stateMachine.ChangeState(stateMachine.WalkState);
            }
            else // 입력 없으면
            {
                stateMachine.CurrentMoveSpeed = stateMachine.MovementSpeed;
                stateMachine.ChangeState(stateMachine.IdleState);
            }
            return;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && TryDetectGrabTarget(out string tag))
        {
            Debug.Log(tag);
            if (tag == "Rope" || tag == "Wall")
            {
                stateMachine.ChangeState(stateMachine.GrabState);

            }
            return;
        }
    }

    // public override void PhysicsUpdate()
    // {
    //     base.PhysicsUpdate();
    //
    //     Rigidbody rb = stateMachine.Player.Rigidbody;
    //     Vector3 velocity = rb.velocity; // 현재 속도 가져옴
    //     
    //     Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);    // 초기 수평 속도 값 가져옴
    //     
    //     if (horizontalVelocity.magnitude < stateMachine.MovementSpeed) // 걷기 속도 미만이면 제자리/잡기 후 점프
    //     {
    //         Vector3 inputDir = GetMovementDirection().normalized;   // WASD 입력 방향
    //         Vector3 airControlForce = inputDir * stateMachine.Player.Data.AirData.AirControlSpeed;  // AirControlSpeed 반영
    //         velocity.x = airControlForce.x; // x축 갱신
    //         velocity.z = airControlForce.z; // y축 갱신
    //     }
    //
    //     rb.velocity = velocity; // 최종 속도 적용
    // }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();   // AirState.PhysicsUpdate 호출
    }

    private bool IsGrounded()
    {
        Transform t = stateMachine.Player.transform;
        Vector3 origin = t.position + Vector3.up * 0.1f;
        float rayLength = 1.5f;
        LayerMask groundMask = LayerMask.GetMask("Ground");

        // 중심
        if (Physics.Raycast(origin, Vector3.down, rayLength, groundMask)) return true;

        // 좌우 앞뒤 방향을 약간 퍼뜨려서 쏘기
        float offset = 0.3f;

        Vector3[] offsets = new Vector3[]
        {
        t.right * offset,     // 오른쪽
        -t.right * offset,    // 왼쪽
        t.forward * offset,   // 앞쪽
        -t.forward * offset   // 뒤쪽
        };

        foreach (var dir in offsets)
        {
            Vector3 offsetOrigin = origin + dir;
            if (Physics.Raycast(offsetOrigin, Vector3.down, rayLength, groundMask))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 떨어지는 상태시 정면 , 위 , 땅과 로프 확인 로직 값 수정시 GrabState스크립트 IsStillGrabbing 함수도 같이 수정 바람!
    /// </summary>
    /// <param name="targetTag"></param>
    /// <returns></returns>
    private bool TryDetectGrabTarget(out string targetTag)
    {
        targetTag = null;

        Transform t = stateMachine.Player.transform;
        Vector3 origin = t.position + Vector3.up * 2.0f;
        float distance = 1.0f;
        float radius = 0.1f; // ← 필요에 따라 값 조절 가능 (0.1 ~ 0.5 추천)

        Vector3 diagonalDir = (t.forward + Vector3.up).normalized; // 로프 감지용 방향

        // 로프 감지 (위쪽 대각선 방향 - Raycast로)
        //if (Physics.Raycast(origin, diagonalDir, out RaycastHit hit, distance, LayerMask.GetMask("Rope")))
        //{
        //    Debug.DrawRay(origin, diagonalDir * distance, Color.yellow); // 디버그용
        //    targetTag = "Rope";
        //    return true;
        //}

        //sphereCast용
        if (Physics.SphereCast(origin, radius, diagonalDir, out RaycastHit hit, distance, LayerMask.GetMask("Rope")))
        {
            Debug.DrawRay(origin, diagonalDir * distance, Color.yellow); // 시각화
            targetTag = "Rope";
            return true;
        }

        // 벽 감지 (앞쪽)
        if (Physics.Raycast(origin, t.forward, distance, LayerMask.GetMask("Ground")))
        {
            Debug.DrawRay(origin, t.forward * distance, Color.yellow);
            targetTag = "Wall";
            return true;
        }

        return false;
    }


    private void DebugDrawGrabRay()
    {
        Transform t = stateMachine.Player.transform;
        Vector3 origin = t.position + Vector3.up * 0.1f;
        float rayLength = 1.5f;
        float offset = 0.3f;

        // 바닥 체크 (중앙 + 주변 4방향)
        Debug.DrawRay(origin, Vector3.down * rayLength, Color.yellow); // 중앙

        Debug.DrawRay(origin + t.right * offset, Vector3.down * rayLength, Color.red);   // 오른쪽
        Debug.DrawRay(origin - t.right * offset, Vector3.down * rayLength, Color.red);   // 왼쪽
        Debug.DrawRay(origin + t.forward * offset, Vector3.down * rayLength, Color.red); // 앞쪽
        Debug.DrawRay(origin - t.forward * offset, Vector3.down * rayLength, Color.red); // 뒤쪽

        // 로프 및 벽 감지용 Ray 시각화
        Vector3 origin2 = t.position + Vector3.up * 2.0f;
        float castDistance = 1.0f;
        Vector3 diagonalDir = (t.forward + Vector3.up).normalized;

        Debug.DrawRay(origin2, diagonalDir * castDistance, Color.blue); // 로프 감지용 Ray
        Debug.DrawRay(origin2, t.forward * castDistance, Color.blue);   // 벽 감지용 Ray
    }




}