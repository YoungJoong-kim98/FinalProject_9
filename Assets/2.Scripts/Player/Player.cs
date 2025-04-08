using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 모든 시스템을 초기화하고, 매 프레임 상태 머신을 실행
// Awake에서 상태 머신을 만들고 Idle 상태로 시작 → Update와 FixedUpdate로 상태를 계속 갱신
// 플레이어의 모든 하위 시스템 (입력, 애니메이션, 이동 등)을 관리하는 중심 클래스 (입력뿐 아니라 플레이어에게 필요한 여러 컴포넌트들을 관리)
public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerSO Data { get; private set; }  // 플레이어 데이터 (속도, 점프력 등 설정)
    
    [field:Header("Animations")]
    [field:SerializeField] public PlayerAnimationData AnimationData { get; private set; }   // 애니메이션 데이터

    public Animator Animator { get; private set; }              // 애니메이션 제어
    public PlayerController Input { get; private set; }         // 입력 처리
    public Rigidbody Rigidbody { get; private set; }
    
    private PlayerStateMachine stateMachine;                    // FSM의 핵심 컨트롤러

    private void Awake()
    {
        AnimationData.Initialize();                         // 애니메이션 파라미터 해시값 초기화
        Animator = GetComponentInChildren<Animator>();      // 애니메이터 연결
        Input = GetComponent<PlayerController>();           // 입력 시스템 연결
        Rigidbody = GetComponent<Rigidbody>();              // Rigidbody 연결
        
        stateMachine = new PlayerStateMachine(this);        // 상태 머신 생성
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 잠금
        stateMachine.ChangeState(stateMachine.IdleState);   // 초기 상태를 Idle로 설정
    }
    
    private void Update()
    {
        stateMachine.HandleInput();     // 입력 처리 (키보드, 마우스 입력 감지)
        stateMachine.Update();          // 논리 업데이트 (상태 전환 등)
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();   // 물리 업데이트 (이동, 회전 등)
    }
}