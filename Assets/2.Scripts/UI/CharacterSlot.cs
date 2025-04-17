using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterSlot
{
    public Button slotButton;
    public GameObject unlockImage;
    public GameObject hiddenImage;
    public int characterBaseIndex;
    public string unlockConditionId; // 앞으로 업적 시스템과 연결할 ID
    [HideInInspector] public bool isUnlocked;

    public void EvaluateUnlock()
    {
        if (string.IsNullOrEmpty(unlockConditionId))
        {
            isUnlocked = true; // 기본 해금
        }
        else
        {
            // 나중에 연결될 실제 시스템으로 교체
         //   isUnlocked = AchievementManager.Instance.HasAchievement(unlockConditionId);
        }
    }
    public void UpdateLockVisual()
    {
        if (unlockImage != null)
            unlockImage.SetActive(!isUnlocked);
        if (hiddenImage != null)
            hiddenImage.SetActive(false);
    }
}