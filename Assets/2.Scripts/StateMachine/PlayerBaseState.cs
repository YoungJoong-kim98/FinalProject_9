using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 모든 상태가 공통으로 사용하는 기능(입력 읽기, 이동, 회전)을 제공
// 상태가 시작되면 입력을 감지하고, 매 프레임 이동 로직을 처리
public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;      // 상태 머신 참조 (상태 전환, 데이터 공유용)
    protected readonly PlayerGroundData groundData; // 지상 상태에서 사용할 데이터 (속도, 회전 감쇠 등)
    
    // 상태 머신 초기화
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;   // 상태 머신 연결
        groundData = stateMachine.Player.Data.GroundData;   // PlayerSO에서 지상 데이터 가져오기
    }
    
    /// <summary>
    /// 상태에 진입할 때 호출 (상태 초기화)
    /// </summary>
    public virtual void Enter()
    {
        AddInputActionsCallbacks();     // 입력 이벤트(이동, 달리기 등) 감지하도록 등록
    }
    
    /// <summary>
    /// 상태에서 나갈 때 호출 (정리 작업)
    /// </summary>
    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();  // 입력 이벤트 해제
    }
    
    /// <summary>
    /// 입력 이벤트를 등록하는 메서드
    /// </summary>
    protected virtual void AddInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;             // 플레이어의 입력 컨트롤러 가져오기
        input.playerActions.Movement.canceled += OnMovementCanceled;    // 이동 입력이 끝날 때 호출
        input.playerActions.Run.started += OnRunStarted;                // 달리기 시작 시 호출
        input.playerActions.Jump.started += OnJumpStarted;
    }
    
    /// <summary>
    /// 입력 이벤트를 해제하는 메서드 (메모리 누수 방지)
    /// </summary>
    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled -= OnMovementCanceled;    // 이동 이벤트 해제
        input.playerActions.Run.started -= OnRunStarted;                // 달리기 이벤트 해제
        input.playerActions.Jump.started -= OnJumpStarted;
    }
    
    /// <summary>
    /// 매 프레임 입력 처리
    /// </summary>
    public virtual void HandleInput()
    {
        ReadMovementInput();    // WASD 등 입력 값을 읽어서 stateMachine에 저장
    }
    
    /// <summary>
    /// 물리 업데이트
    /// </summary>
    public virtual void PhysicsUpdate()
    {
        
    }
    
    /// <summary>
    /// 논리 업데이트
    /// </summary>
    public virtual void Update()
    {
        Move();     // 이동 및 회전 실행
    }
    
    /// <summary>
    /// 이동 입력이 취소될 때 호출 (e.g. WASD 키 뗐을 때)
    /// </summary>
    /// <param name="context"></param>
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }
    
    /// <summary>
    /// 달리기 입력이 시작될 때 호출 (e.g. Shift 키 눌렀을 때)
    /// </summary>
    /// <param name="context"></param>
    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        
    }
    
    // 애니메이션 시작
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);  // Animator의 bool 파라미터 true로 설정
    }

    // 애니메이션 종료
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }
    
    // WASD 입력을 읽어서 상태 머신에 저장
    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();
    }
    
    // 이동과 회전을 총괄하는 메서드
    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection(); // 카메라 기준 이동 방향 계산
        
        Move(movementDirection);    // 플레이어를 해당 방향으로 이동
        
        Rotate(movementDirection);  // 이동 방향으로 플레이어 회전
    }
    
    // 카메라를 기준으로 WASD 입력을 3D 방향으로 변환
    private Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward; // 카메라의 앞 방향
        Vector3 right = stateMachine.MainCameraTransform.right;     // 카메라의 우측 방향

        forward.y = 0;  // 수평 이동만 하도록 y를 0으로
        right.y = 0;

        forward.Normalize();    // 방향 벡터 크기를 1로 맞춤
        right.Normalize();
        
        // 입력값을 카메라 방향에 맞춰 합성
        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x; 
    }
    
    // 플레이어를 실제로 이동시킴 (CharacterController 사용)
    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();   // 현재 속도 계산
        
        Vector3 velocity = direction * movementSpeed;
        velocity.y = stateMachine.Player.Rigidbody.velocity.y; // y축은 물리로 유지
        stateMachine.Player.Rigidbody.velocity = velocity; // Rigidbody로 이동
    }
    
    // 이동 속도 계산
    private float GetMovementSpeed()
    {
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }
    
    // 플레이어를 이동 방향으로 부드럽게 회전
    private void Rotate(Vector3 direction)
    {
        if(direction != Vector3.zero)   // 방향이 있을 때만 회전
        {
            Transform playerTransform = stateMachine.Player.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction); // 목표 회전각 계산
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);   // 현재 회전에서 목표 회전으로 보간
        }
    }
    
    protected bool IsGrounded() // 착지 감지
    {
        return Physics.Raycast(stateMachine.Player.transform.position, Vector3.down, 0.1f);
    }
}
