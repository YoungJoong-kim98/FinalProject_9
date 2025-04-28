using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystem : MonoBehaviour
{

    // 커스터마이징용과 업적
    public int jumpCount;//업적,커스텀
    public int grabCount;//잡기
    public int jumpPlatform;//장애물

    public bool researcherStage;//연구원스테이지
    public bool fallingCrash;//추락
    public bool playTime1HourUnlocked;//1시간 플레이
    public bool playTime3HourUnlocked;//3시간 플레이
    public bool customizationChangedUnlocked;//커마변경
    public bool idle30MinutesUnlocked;//30분 부동

    public float playTime; // 플레이 시간 (초 단위)
    private float idleStartTime;
    private Vector3 lastPlayerPosition;
    private float idleCheckInterval = 1f;
    private float idleTimer = 0f;


    private void Start()
    {
        jumpCount = 0;
        jumpPlatform = 0;
        researcherStage = false;
        fallingCrash = false;
        grabCount = 0;

        playTime = 0;
        playTime1HourUnlocked = false;
        playTime3HourUnlocked = false;
        customizationChangedUnlocked = false;
        idle30MinutesUnlocked = false;

        if (GameManager.Instance.Player != null)
        {
            lastPlayerPosition = GameManager.Instance.Player.transform.position;
        }
    }

    private void Update()
    {
        playTime += Time.deltaTime;
        CheckIdleStatus();
        CheckAchievements();
    }

    private void CheckIdleStatus()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleCheckInterval)
        {
            if (GameManager.Instance.Player != null)
            {
                var currentPosition = GameManager.Instance.Player.transform.position;
                if (Vector3.Distance(currentPosition, lastPlayerPosition) < 0.01f)
                {
                    // 30분 이상 움직이지 않음
                    if (!idle30MinutesUnlocked && playTime >= 1800f)
                    {
                        idle30MinutesUnlocked = true;
                        Debug.Log("30분 이상 가만히 있음 - 멍때리기 달성");
                    }
                }
                else
                {
                    // 움직임이 감지되면 다시 시작
                    lastPlayerPosition = currentPosition;
                    idleTimer = 0f;
                }
            }
        }
    }

    private void CheckAchievements()
    {
        if (jumpCount >= 10000)
        {
            Debug.Log("점프 1만회 달성!");
        }

        if (grabCount >= 20)
        {
            Debug.Log("20회 매달리기 - 원숭이 손!");
        }

        if (!playTime1HourUnlocked && playTime >= 3600f)
        {
            playTime1HourUnlocked = true;
            Debug.Log("1시간 플레이 - 슬슬 익숙해진다");
        }

        if (!playTime3HourUnlocked && playTime >= 10800f)
        {
            playTime3HourUnlocked = true;
            Debug.Log("3시간 플레이 - 포기할 순 없다");
        }

        if (!customizationChangedUnlocked )
        {
            customizationChangedUnlocked = true;
            Debug.Log("커스터마이징 변경 완료 - 스타일은 나의 힘!");
        }
    }

    public void JumpCount()
    {
        jumpCount++;
        UpdateAchievements();
    }

    public void GrabCount()
    {
        grabCount++;
        UpdateAchievements();
    }

    public void JumpPlatformCount()
    {
        jumpPlatform++;
        UpdateAchievements();
    }

    private void UpdateAchievements()
    {
        var customizingUI = UIManager.Instance.GetCurrentCustomizingUI();
        if (customizingUI != null)
        {
            customizingUI.RefreshCharacterSlots();
        }
    }
    public AchievementData ToData() // 업적 데이터로 반환
    {
        return new AchievementData
        {
            jumpCount = this.jumpCount,
            jumpPlatform = this.jumpPlatform,
            researcherStage = this.researcherStage,
            fallingCrash = this.fallingCrash,
            grabCount = this.grabCount,
            playTime1HourUnlocked = this.playTime1HourUnlocked,
            playTime3HourUnlocked = this.playTime3HourUnlocked,
            customizationChangedUnlocked = this.customizationChangedUnlocked,
            idle30MinutesUnlocked = this.idle30MinutesUnlocked,
            playTime = this.playTime
        };
    }

    public void LoadFromData(AchievementData data)
    {
        if (data == null) return;

        this.jumpCount = data.jumpCount;
        this.jumpPlatform = data.jumpPlatform;
        this.researcherStage = data.researcherStage;
        this.fallingCrash = data.fallingCrash;
        this.grabCount = data.grabCount;
        this.playTime1HourUnlocked = data.playTime1HourUnlocked;
        this.playTime3HourUnlocked = data.playTime3HourUnlocked;
        this.customizationChangedUnlocked = data.customizationChangedUnlocked;
        this.idle30MinutesUnlocked = data.idle30MinutesUnlocked;
        this.playTime = data.playTime;
    }
}
