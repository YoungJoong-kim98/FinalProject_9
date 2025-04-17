using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        SetupCharacterSlots();

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

            capturedSlot.EvaluateUnlock();
            capturedSlot.UpdateLockVisual();

            if (capturedSlot.isUnlocked)
            {
                capturedSlot.slotButton.onClick.AddListener(() =>
                    SelectCharacter(capturedSlot.characterBaseIndex));
            }
            else
            {
                capturedSlot.slotButton.onClick.AddListener(() =>
                    ShowUnlockInfo(capturedSlot));
            }
        }
    }
    void OnClickBack()
    {
        this.gameObject.SetActive(false);
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
        Vector3 pos = mainCharacter.transform.position;
        Quaternion rot = mainCharacter.transform.rotation;
        Transform parent = mainCharacter.transform.parent;

        Destroy(mainCharacter);
        mainCharacter = Instantiate(newPrefab, pos, rot, parent);
    }
    void OnClickApply()
    {
        selectedCharacterPrefab = GetCharacterFromIndex(currentCharacterBaseIndex + currentColorOffset);

        Debug.Log($"[Apply] ���õ� ĳ���� ������: {selectedCharacterPrefab?.name}");

        //TODO: �÷��̾� ĳ���� �ý��۰� ������ �κ�
        //PlayerManager.Instance.SetCharacter(selectedCharacterPrefab);
        this.gameObject.SetActive(false);
    }
    void ShowUnlockInfo(CharacterSlot slot)
    {
        Debug.Log($"[���] ���� {slot.characterBaseIndex}�� ���� �رݵ��� �ʾ���");
       
    }
    public void RefreshCharacterSlots()
    {
        foreach (var slot in characterSlots)
        {
            slot.EvaluateUnlock();
            slot.UpdateLockVisual();
        }
    }
}
