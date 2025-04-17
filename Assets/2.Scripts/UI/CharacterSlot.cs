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
    public string unlockConditionId; // ������ ���� �ý��۰� ������ ID
    [HideInInspector] public bool isUnlocked;

    public void EvaluateUnlock()
    {
        if (string.IsNullOrEmpty(unlockConditionId))
        {
            isUnlocked = true; // �⺻ �ر�
        }
        else
        {
            // ���߿� ����� ���� �ý������� ��ü
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