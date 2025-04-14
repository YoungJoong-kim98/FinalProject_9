using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 키보드, 마우스 입력을 받아내고 FSM이나 이동 로직이 이걸 참조해서 행동함
public class PlayerController : MonoBehaviour
{
    public PlayerInputs playerInputs { get; private set; }  // 인풋 시스템에서 전체 입력을 다루는 객체
    public PlayerInputs.PlayerActions playerActions { get; private set; }   // Player 액션 맵 (이동, 점프 등 액션)
    public Vector2 MoveInput { get; private set; }
    public bool IsRunning { get; private set; }

    private void Awake()
    {
        playerInputs = new PlayerInputs();      // InputActions 생성
        playerActions = playerInputs.Player;    // 플레이어 인풋 안의 액션 맵, 그 옆 액션을 표시
    }
    
    private void Update()
    {
        MoveInput = playerActions.Movement.ReadValue<Vector2>();
        IsRunning = playerActions.Run.IsPressed();
    }

    private void OnEnable()     // 켜질 때
    {
        playerInputs.Enable();  // 이 오브젝트가 활성화되면 인풋 활성화
    }

    private void OnDisable()    // 꺼질 때
    {
        playerInputs.Disable(); // 비활성화되면 인풋 비활성화
    }
}