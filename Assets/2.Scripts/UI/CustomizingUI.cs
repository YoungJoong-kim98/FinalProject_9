using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingUI : PopUpUI
{
    [Header("save")]
    public Button ApplyButton;
    public Button BackButton;

    [Header("character slots")]
    public List<CharacterSlot> characterSlots;

    [Header("colorButtons")]
    public Button aButton;
    public Button bButton;
    public Button cButton;
    public Button dButton;

    [Header("All character prefabs(�����ٲٸ�ȵ�)")]
    public List<GameObject> allCharacterPrefabs;
    public GameObject mainCharacter;

    private int currentCharacterBaseIndex = 0;  // ���� ���� �� ������ �Ǵ� ĳ���� �ε���
    private int currentColorOffset = 0;         // ���� ���õ� ���� A(0), B(1), C(2), D(3)
    private GameObject selectedCharacterPrefab; // Apply���� ���� ������ ������
    public RuntimeAnimatorController fallingPlayerAnimatorController;// FallingPlayer �ִϸ��̼� ��Ʈ�ѷ�
    public AchievementSystem achievementSystem;//����

    void Start()
    {
        achievementSystem = FindObjectOfType<AchievementSystem>();
        SetupCharacterSlots();
        RefreshCharacterSlots();

        aButton.onClick.AddListener(() => ChangeColor(0));
        bButton.onClick.AddListener(() => ChangeColor(1));
        cButton.onClick.AddListener(() => ChangeColor(2));
        dButton.onClick.AddListener(() => ChangeColor(3));

        ApplyButton.onClick.AddListener(OnClickApply);
        BackButton.onClick.AddListener(OnClickBack);
    }

    void SetupCharacterSlots()
    {
        for (int i = 0; i < characterSlots.Count; i++)
        {
            var capturedSlot = characterSlots[i];

            Debug.Log($"[Setup] ���� {i} �� BaseIndex: {capturedSlot.characterBaseIndex}");

            capturedSlot.slotButton.onClick.RemoveAllListeners();
            // ��ư Ŭ�� �� ĳ���� ����
            capturedSlot.slotButton.onClick.AddListener(() =>
            SelectCharacter(capturedSlot.characterBaseIndex));
        }
    }
    void OnClickBack()
    {
        this.gameObject.SetActive(false);
        var startUI = UIManager.Instance.GetPopupUI<StartUI>();
        if (startUI != null)
        {
            startUI.gameObject.SetActive(true);
        }
    }

    void SelectCharacter(int baseIndex)
    {
        currentCharacterBaseIndex = baseIndex;
        currentColorOffset = 0; // �⺻ A ����
        ReplaceMainCharacter(GetCharacterFromIndex(currentCharacterBaseIndex));
    }

    void ChangeColor(int colorOffset)
    {
        currentColorOffset = colorOffset;
        int finalIndex = currentCharacterBaseIndex + currentColorOffset;
        ReplaceMainCharacter(GetCharacterFromIndex(finalIndex));
    }

    GameObject GetCharacterFromIndex(int index)
    {
        if (index >= 0 && index < allCharacterPrefabs.Count)
        {
            return allCharacterPrefabs[index];
        }

        Debug.LogError($"������ �ε��� {index} �� ����Ʈ ������ �����");
        return null;
    }

    void ReplaceMainCharacter(GameObject newPrefab)
    {
        if (newPrefab == null || mainCharacter == null) return;

        // ���� ��ġ, ȸ�� ����
        Vector3 currentPosition = mainCharacter.transform.position;
        Quaternion currentRotation = mainCharacter.transform.rotation;
        Vector3 currentScale = mainCharacter.transform.localScale; // ������ ���� ����
        Transform currentParent = mainCharacter.transform.parent; // �θ� ���� ����

        // ���� ĳ���� �ı�
        Destroy(mainCharacter);

        // �� ĳ���� �ν��Ͻ�ȭ
        mainCharacter = Instantiate(newPrefab, currentPosition, currentRotation, currentParent);

        // �� ĳ������ �������� �⺻������ ����
        mainCharacter.transform.localScale = currentScale;

        // �� ĳ���Ϳ� Animator�� ���� ��� �ִϸ����� ��Ʈ�ѷ� ����
        Animator animator = mainCharacter.GetComponent<Animator>();
        if (animator != null && fallingPlayerAnimatorController != null)
        { 
            animator.runtimeAnimatorController = fallingPlayerAnimatorController; // �ִϸ����� ��Ʈ�ѷ� ����
        }
    }
    void OnClickApply()
    {
        selectedCharacterPrefab = GetCharacterFromIndex(currentCharacterBaseIndex + currentColorOffset);

        Debug.Log($"[Apply] ���õ� ĳ���� ������: {selectedCharacterPrefab?.name}");

        var startUI = UIManager.Instance.GetPopupUI<StartUI>();
        if (startUI != null && selectedCharacterPrefab != null && startUI.startCharacter != null)
        {
            Transform startCharacterTransform = startUI.startCharacter.transform;

            // ���� �ڽ�(ĳ���� ������) ��������
            Transform oldChild = startCharacterTransform.childCount > 0 ? startCharacterTransform.GetChild(0) : null;

            // �⺻��: startCharacter ��ġ ����
            Vector3 localPos = Vector3.zero;
            Quaternion localRot = Quaternion.identity;
            Vector3 localScale = Vector3.one;

            if (oldChild != null)
            {
                localPos = oldChild.localPosition;
                localRot = oldChild.localRotation;
                localScale = oldChild.localScale;

                Destroy(oldChild.gameObject); // ���� ĳ���� ����
            }

            // �� ĳ���� ����
            GameObject newCharacter = Instantiate(selectedCharacterPrefab, startCharacterTransform);

            // ����� localTransform �� ����
            newCharacter.transform.localPosition = localPos;
            newCharacter.transform.localRotation = localRot;
            newCharacter.transform.localScale = localScale;

            startUI.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("StartUI or selectedCharacterPrefab or startCharacter is null");
        }

        this.gameObject.SetActive(false);
    }
    //void ShowUnlockInfo(CharacterSlot slot)
    //{
    //    Debug.Log($"[���] ���� {slot.characterBaseIndex}�� ���� �رݵ��� �ʾ���");

    //}
    public void RefreshCharacterSlots()
    {
        for (int i = 0; i < characterSlots.Count; i++)
        {
            var slot = characterSlots[i];
            bool isUnlocked = false;

            switch (i)
            {
                case 0:
                case 1:
                    isUnlocked = true;
                    break;
                case 2:
                    isUnlocked = achievementSystem.jumpCount >= 100;
                    break;
                case 3:
                    isUnlocked = achievementSystem.jumpPlatform >= 30;
                    break;
                case 4:
                    isUnlocked = achievementSystem.researcherStage;
                    break;
                case 5:
                    isUnlocked = achievementSystem.fallingCrash;
                    break;
                case 6:
                    isUnlocked = achievementSystem.grabCount >= 100;
                    break;
                case 7:
                    isUnlocked = achievementSystem.completionTime;
                    break;
            }

            slot.isUnlocked = isUnlocked;

            if (isUnlocked && slot.hiddenImage != null)
            {
                Destroy(slot.hiddenImage); // ��� �̹��� ����
                slot.hiddenImage = null;   // ���� ����
            }

            slot.UpdateLockVisual(); // ���־� ���� ������Ʈ
        }
    }
}
