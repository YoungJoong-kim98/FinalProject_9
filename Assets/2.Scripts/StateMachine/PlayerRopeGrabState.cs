//using System.Collections;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerRopeGrabState : PlayerAirState
//{
//    private bool hasJumped = false;
//    private float slowFallSpeed = -0.1f;

//    public PlayerRopeGrabState(PlayerStateMachine stateMachine) : base(stateMachine) { }

//    public override void Enter()
//    {
//        base.Enter();

//        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);

//        var rb = stateMachine.Player.Rigidbody;
//        rb.velocity = Vector3.zero;
//        rb.useGravity = false;
//        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

//        hasJumped = false;
//    }

//    public override void Exit()
//    {
//        base.Exit();

//        StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);

//        var rb = stateMachine.Player.Rigidbody;
//        rb.useGravity = true;
//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//    }

//    public override void Update()
//    {
//        base.Update();

//        // 천천히 낙하
//        Vector3 velocity = stateMachine.Player.Rigidbody.velocity;
//        velocity.y = slowFallSpeed;
//        stateMachine.Player.Rigidbody.velocity = velocity;

//        // 좌클릭 해제 시 탈출
//        if (!Mouse.current.leftButton.isPressed)
//        {
//            stateMachine.ChangeState(stateMachine.FallState);
//            return;
//        }

//        // 점프하면 앞으로 튕기기
//        if (!hasJumped && Keyboard.current.spaceKey.wasPressedThisFrame)
//        {
//            stateMachine.IsMovementLocked = true;

//            float forwardForce = 5f;
//            float upwardForce = stateMachine.Player.Data.AirData.JumpForce * 1.2f;

//            Vector3 forwardDir = stateMachine.Player.transform.forward;
//            Vector3 jumpDirection = forwardDir * forwardForce + Vector3.up * upwardForce;

//            var rb = stateMachine.Player.Rigidbody;
//            rb.velocity = Vector3.zero;
//            rb.AddForce(jumpDirection, ForceMode.Impulse);

//            hasJumped = true;

//            stateMachine.Player.StartCoroutine(UnlockMovementAfterDelay(1f));
//            stateMachine.ChangeState(stateMachine.FallState);
//        }
//    }

//    private IEnumerator UnlockMovementAfterDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        stateMachine.IsMovementLocked = false;
//        stateMachine.Player.Rigidbody.drag = 0f;
//    }
//}
