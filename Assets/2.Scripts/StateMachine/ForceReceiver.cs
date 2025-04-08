// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class ForceReceiver : MonoBehaviour
// {
//     [SerializeField] private CharacterController _controller;   // 캐릭터 컨트롤러 참조
//     
//     private float _verticalVelocity;    // 세로 방향의 속도 (중력, 점프 등 적용)
//     
//     public Vector3 Movement => Vector3.up * _verticalVelocity;  // 현재 이동 벡터 (y축만 사용)
//     
//     void Start()
//     {
//         _controller = GetComponent<CharacterController>();  // 컨트롤러 초기화
//     }
//     
//     void Update()
//     {
//         if (_controller.isGrounded) // 비닥에 닿아 있다면
//         {
//             _verticalVelocity = Physics.gravity.y * Time.deltaTime; // 바닥에 붙어 있도록 유지
//         }
//         else  // 공중이면
//         {
//             _verticalVelocity += Physics.gravity.y * Time.deltaTime;    // 중력의 영향을 받도록 더해줌
//         }
//     }
//
//     // 점프 요청 시 호출 (점프 힘을 세로 속도에 추가)
//     public void Jump(float jumpForce)
//     {
//         _verticalVelocity += jumpForce; // 순간적으로 위로 속도 증가
//     }
// }
