using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallState : PlayerAirState
{
    private float _fallSpeed = 1f;  // 추락 속도 초당 1f 증가
    private float _maxFallSpeed = 15f;  // 최대 낙하 속도 제한
    private float _fallTime;    // 낙하 시간 측정
    
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
            Debug.Log($"낙하 시간: {_fallTime}초"); // 착지 시 낙하 시간 출력
            
            float preservedSpeed = stateMachine.CurrentMoveSpeed; // 착지 전 속도 저장
            
            if (stateMachine.Player.Input.playerActions.Run.IsPressed()) // Shift 누르고 있으면
            {
                stateMachine.CurrentMoveSpeed = preservedSpeed; // 감소된 속도 사용
                stateMachine.ChangeState(stateMachine.RunState);
                Debug.Log($"Fall to Run - 현재 이동 속도: {stateMachine.CurrentMoveSpeed}");
            }
            else if (stateMachine.MovementInput != Vector2.zero) // 이동 입력 있으면
            {
                stateMachine.CurrentMoveSpeed = stateMachine.MovementSpeed; // 2f
                stateMachine.ChangeState(stateMachine.WalkState);
            }
            else // 입력 없으면
            {
                stateMachine.CurrentMoveSpeed = stateMachine.MovementSpeed; // 2f
                stateMachine.ChangeState(stateMachine.IdleState);
            }
            return;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && TryDetectGrabTarget(out string tag))
        {
            if (tag == "Rope")
            {
                stateMachine.ChangeState(stateMachine.GrabState);
            }
            else if (tag == "Wall")
            {
                stateMachine.ChangeState(stateMachine.GrabState);
            }
            return;
        }
    }


    private bool IsGrounded() //땅인지 체크
    {
        Transform t = stateMachine.Player.transform;
        return Physics.Raycast(t.position + Vector3.up * 0.1f, Vector3.down, 0.2f, LayerMask.GetMask("Ground"));
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
        Vector3 origin = t.position + Vector3.up * 0.5f;
        float distance = 1.0f;
        Vector3 grab = new Vector3(0f, 1.5f, 1f);
        // 로프 감지 (위쪽)
        if (Physics.Raycast(origin+ grab, Vector3.up, distance, LayerMask.GetMask("Rope")))
        {
            Debug.DrawRay(origin, Vector3.up * distance, Color.green);
            targetTag = "Rope";
            return true;
        }

        // 벽 감지 (앞쪽)
        if (Physics.Raycast(origin, t.forward, distance, LayerMask.GetMask("Ground")))
        {
            Debug.DrawRay(origin, t.forward * distance, Color.red);
            targetTag = "Wall";
            return true;
        }

        return false;
    }
    private void DebugDrawGrabRay()
    {
        Transform t = stateMachine.Player.transform;
        Vector3 origin = t.position + Vector3.up * 0.5f;
        float distance = 1.0f;
        Vector3 grab = new Vector3(0f, 1.5f, 1f);
        // 위쪽 (로프용)
        Debug.DrawRay(origin+ grab, Vector3.up * distance, Color.green);

        // 앞쪽 (벽용)
        Debug.DrawRay(origin, t.forward * distance, Color.red);

        // 땅 체크용 아래 방향도 보고 싶으면 아래도 추가
        Debug.DrawRay(t.position + Vector3.up * 0.1f, Vector3.down * 0.1f, Color.yellow);
    }

}