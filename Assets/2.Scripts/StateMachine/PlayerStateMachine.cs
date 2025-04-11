using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 생성 시 모든 상태를 준비하고, ChangeState로 현재 상태를 전환
public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }       // 플레이어 객체 참조

    public Vector2 MovementInput { get; set; }  // 이동 입력값 (WASD 등)
    public float MovementSpeed { get; private set; }    // 기본 이동 속도
    public float RotationDamping { get; private set; }  
    public float MovementSpeedModifier { get; set; } = 1f;  // 속도 배율 (Walk, Run 등)
    public float CurrentMoveSpeed { get; set; }  // 현재 이동 속도 저장 (RunState, JumpState 공용 변수)

    public float JumpForce { get; set; }

    public Transform MainCameraTransform { get; set; }      // 카메라 방향 참조
    
    public PlayerIdleState IdleState { get; private set; }  // 대기 상태
    public PlayerWalkState WalkState { get; private set;}   // 걷기 상태
    public PlayerRunState RunState { get; private set;}     // 뛰기 상태
    public PlayerJumpState JumpState { get; private set; } // 점프 상태
    public PlayerFallState FallState { get; private set; } //추락 상태
    public PlayerGrabState GrabState { get; private set; } //잡기 상태
    //public PlayerRopeGrabState RopeGrabState { get; private set; } // 로프 잡기 상태
    public bool CanGrabWall { get; set; } = true; // 잡기 가능 여부
    public bool IsMovementLocked { get; set; } = false; // 이동 잠금
    public bool HasJustJumpedFromGrab { get; set; }
    public PlayerStateMachine(Player player)
    {
        this.Player = player;

        MainCameraTransform = Camera.main.transform;        // 카메라 연결
        
        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);

        JumpState = new PlayerJumpState(this);
        FallState = new PlayerFallState(this);
        GrabState = new PlayerGrabState(this);
        //RopeGrabState = new PlayerRopeGrabState(this);

        MovementSpeed = player.Data.GroundData.BaseSpeed;   // 기본 속도 설정
        CurrentMoveSpeed = MovementSpeed;    // 초기값 2f
        RotationDamping = player.Data.GroundData.BaseRotationDamping;
    }
}