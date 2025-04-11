using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerGroundData   // 땅에 붙어 있을 때 필요한 데이터들 묶은 클래스
{
    [field:SerializeField][field:Range(0f, 75f)] public float BaseSpeed { get; private set; } = 6f;
    [field:SerializeField][field: Range(0f, 100f)] public float BaseRotationDamping { get; private set; } = 30f;

    [field:Header("IdleData")]

    [field:Header("WalkData")]
    [field:SerializeField][field: Range(0f, 5f)] public float WalkSpeedModifier { get; private set; } = 1f;
    
    [field:Header("RunData")]
    [field:SerializeField][field:Range(0f, 75f)] public float RunMaxSpeed { get; private set; } = 12f;      // 달리기 최대 속도
    [field:SerializeField][field:Range(0f, 15f)] public float RunAcceleration { get; private set; } = 8f;   // 가속도
}

[Serializable]
public class PlayerAirData // 공중에 있을 때 필요한 데이터들 묶은 클래스
{
    [field: Header("JumpData")]
    [field: SerializeField]
    [field: Range(0f, 75f)]
    public float JumpForce { get; private set; } = 10f;
    [field: SerializeField]
    [field:Range(0f,5f)]
    public float AirControlSpeed { get; private set; } = 5f; // 공중에서의 수평 속도


    [field: Header("FallData")]
    [field: SerializeField]
    [field: Range(0f, 70f)]
    public float FallSpeed { get; private set; } = 30f; // 추락 가속도

    [field: SerializeField]
    [field: Range(0f, 100f)]
    public float MaxFallSpeed { get; private set; } = 70f; // 최대 추락 속도
}

[CreateAssetMenu(fileName ="Player", menuName = "Characters/Player")]
public class PlayerSO : ScriptableObject
{
    [field: SerializeField] public PlayerGroundData GroundData { get; private set; }
    [field: SerializeField] public PlayerAirData AirData { get; private set; }
}