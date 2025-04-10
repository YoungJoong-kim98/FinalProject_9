using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 모든 상태가 공통으로 사용하는 기능(입력 읽기, 이동, 회전)을 제공
// 상태가 시작되면 입력을 감지하고, 매 프레임 이동 로직을 처리
public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Player.Data.GroundData;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();     // 입력 이벤트(이동, 달리기 등) 등록
    }

    public virtual void Exit()
    {
        
        RemoveInputActionsCallbacks();  // 입력 이벤트 해제
    }
    
    protected virtual void AddInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled += OnMovementCanceled;    // 이동 취소 시 호출
        input.playerActions.Run.started += OnRunStarted;                // 달리기 시작 시 호출
        input.playerActions.Run.canceled += OnRunCanceled;              // 달리기 종료 (Shift 뗄 때)
        input.playerActions.Jump.started += OnJumpStarted;              // 점프 시 호출
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled -= OnMovementCanceled;
        input.playerActions.Run.started -= OnRunStarted;
        input.playerActions.Run.canceled -= OnRunCanceled;
        input.playerActions.Jump.started -= OnJumpStarted;
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();        // 입력값 읽기 (WASD 등)
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
        Move();     // 이동 및 회전 실행
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnRunCanceled(InputAction.CallbackContext context)
    {
        
    }
    
    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {

    }
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }

    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();
    }

    protected virtual void Move()
    {
        Vector3 movementDirection = GetMovementDirection(); // 카메라 기준 이동 방향 계산

        Move(movementDirection);    // 이동

        Rotate(movementDirection);  // 회전
    }

    protected Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

    protected virtual void Move(Vector3 direction)
    {
        if (stateMachine.IsMovementLocked)
        {
            return; // 이동 금지 중일 땐 처리 안 함
        }
        float movementSpeed = GetMovementSpeed();
        // 현재 속도 가져오기
        Rigidbody rb = stateMachine.Player.Rigidbody;
        Vector3 currentVelocity = rb.velocity;

        // 새로운 속도 계산
        Vector3 desiredVelocity = direction.normalized * movementSpeed;
        desiredVelocity.y = currentVelocity.y; // y속도 유지 (중력, 점프 등)
        // 속도 적용
        rb.velocity = desiredVelocity;

    }

    protected float GetMovementSpeed()
    {
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)  // WASD 입력이 있으면 회전 시작
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion smoothedRotation = Quaternion.Slerp(
                stateMachine.Player.transform.rotation,
                targetRotation,
                stateMachine.RotationDamping * Time.deltaTime
            );

            // Rigidbody 회전 적용
            stateMachine.Player.Rigidbody.MoveRotation(smoothedRotation);
        }
    }

}
