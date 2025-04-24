using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : BaseUI
{
    [Header("UI Elements")]
    public TextMeshProUGUI timetableText;
    public TextMeshProUGUI leveltxt;

    private float elapsedTime = 0f;//경과 시간
    private float timeAccumulator = 0f;//누적 시간

    private SoundManager soundManager;//사운드 매니저 참조

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager != null)
        {
            soundManager.PlayBGM(0); // 게임 시작 시 BGM을 재생
        }
        else
        {
            Debug.LogWarning("SoundManager를 찾을 수 없음");
        }

        UpdateTimetableText(); // UI 초기화
    }

    void Update()
    {
        // 시간 누적 처리
        timeAccumulator += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        // 타임테이블 업데이트
        if (timeAccumulator >= 1f)
        {
            UpdateTimetableText();
            timeAccumulator = 0f;
        }
    }
    private void UpdateTimetableText()   // 타임테이블을 갱신 메서드
    {
        timetableText.text = GetFormattedTime(elapsedTime);
    }
    private string GetFormattedTime(float timeInSeconds) // 시간 포맷팅 유틸리티 함수
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        return string.Format("{0:D2}h:{1:D2}m:{2:D2}s", hours, minutes, seconds);
    }
  
}
