using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerGroundData   // 땅에 붙어 있을 때 필요한 데이터들 묶은 클래스
{
    [field:SerializeField][field:Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f; // 기본 이동 속도
    [field:SerializeField][field: Range(0f, 25f)] public float BaseRotationDamping { get; private set; } = 1f;  // 회전 감쇠 값 (부드러운 회전 속도)

    [field:Header("IdleData")]

    [field:Header("WalkData")]
    [field:SerializeField][field: Range(0f, 2f)] public float WalkSpeedModifier { get; private set; } = 0.225f; // 걷기 속도 수정자

    [field:Header("RunData")]
    [field:SerializeField][field: Range(0f, 2f)] public float RunSpeedModifier { get; private set; } = 1f;      // 달리기 속도 수정자
}

[Serializable]
public class PlayerAirData  // 공중 상태에서 필요한 데이터들을 묶은 클래스
{
    [field: Header("JumpData")]
    [field:SerializeField][field: Range(0f, 25f)] public float JumpForce { get; private set; } = 5f;    // 점프 힘
}

[CreateAssetMenu(fileName ="Player", menuName = "Characters/Player")]
public class PlayerSO : ScriptableObject
{
    [field: SerializeField] public PlayerGroundData GroundData { get; private set; }    // 지상 관련 설정 데이터
    [field: SerializeField] public PlayerAirData AirData { get; private set; }          // 공중 관련 설정 데이터
}