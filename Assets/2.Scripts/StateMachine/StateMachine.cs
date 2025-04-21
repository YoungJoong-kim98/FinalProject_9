using UnityEngine;

public interface IState
{
    public void Enter();    // 상태에 들어갈 때
    public void Exit();     // 상태에서 나올 때
    public void HandleInput();  // 입력 처리
    public void Update();       // 일반 업데이트
    public void PhysicsUpdate();// 물리 관련 업데이트
}

// 상태 머신이 현재 상태를 제어하며 자동으로 메서드를 호출함
public abstract class StateMachine
{
    protected IState currentState;  // State 정보가 들어옴
    public IState CurrentState => currentState; // 외부에서 읽기 전용 접근 가능하게

    public void ChangeState(IState newState)
    {
        if (CurrentState == newState) return;
        Debug.Log($"[ChangeState] {CurrentState} → {newState}");
        
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    
    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}