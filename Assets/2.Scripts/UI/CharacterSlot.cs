using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterSlot
{
    public Button slotButton;
    public GameObject hiddenImage;
    public GameObject unlockImage;
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
        if (isUnlocked)
        {
            if (hiddenImage != null)
            {
                GameObject.Destroy(hiddenImage);
                hiddenImage = null;
            }

            if (unlockImage != null)
            {
                GameObject.Destroy(unlockImage);
                unlockImage = null;
            }
        }
        else
        {
            if (hiddenImage != null)
                hiddenImage.SetActive(true);

            if (unlockImage != null)
                unlockImage.SetActive(false);
        }
    }
}