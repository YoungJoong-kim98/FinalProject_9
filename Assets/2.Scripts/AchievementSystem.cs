using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
    public int jumpCount;
    public int jumpPlatform;
    public bool researcherStage;
    public bool fallingCrash;
    public int grabCount;
    public bool completionTime;

    private void Start()
    {
        jumpCount = 0;
        jumpPlatform = 0;
        researcherStage= false;
        fallingCrash = false;
        grabCount = 0;
        completionTime = false;

    }
    //private void Update() //- inspectorâ������ ���� �����Ͽ� Ȯ���ϴ� �뵵
    //{
    //    var customizingUI = UIManager.Instance.GetCurrentCustomizingUI();
    //    if (customizingUI != null)
    //    {
    //        customizingUI.RefreshCharacterSlots(); // ���� ���� ���� ����
    //    }
    //}
    public void JumpCount()
    {
        jumpCount += 1;
        Debug.Log(jumpCount);
        UpdateAchievements();
    }

    public  void JumpPlatformCount()
    {
        jumpPlatform += 1;
        UpdateAchievements();
    }

    public void GrabCount()
    {
        grabCount += 1;
        Debug.Log(grabCount);
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
    public AchievementData ToData()
    {
        return new AchievementData
        {
            jumpCount = this.jumpCount,
            jumpPlatform = this.jumpPlatform,
            researcherStage = this.researcherStage,
            fallingCrash = this.fallingCrash,
            grabCount = this.grabCount,
            completionTime = this.completionTime
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
        this.completionTime = data.completionTime;
    }
}
