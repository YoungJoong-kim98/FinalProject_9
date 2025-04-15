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
    public Button slot1;
    public Button slot2;
    public Button slot3;
    public Button slot4;
    public Button slot5;
    public Button slot6;
    public Button slot7;
    public Button slot8;

    [Header("colorButtons")]
    public Button aButton;
    public Button bButton;
    public Button cButton;
    public Button dButton;

    [Header("HiddenImages")]
    public GameObject hiddenImage3;
    public GameObject hiddenImage4;
    public GameObject hiddenImage5;
    public GameObject hiddenImage6;
    public GameObject hiddenImage7;
    public GameObject hiddenImage8;

    [Header("Achievement")]
    public GameObject Unlock3;
    public GameObject Unlock4;
    public GameObject Unlock5;
    public GameObject Unlock6;
    public GameObject Unlock7;
    public GameObject Unlock8;

    [Header("All character prefabs(�����ٲٸ�ȵ�)")]
    public List<GameObject> allCharacterPrefabs;

    public GameObject mainCharacter;

    private int currentCharacterBaseIndex = 0;  // ���� ���� �� ������ �Ǵ� ĳ���� �ε���
    private int currentColorOffset = 0;         // ���� ���õ� ���� A(0), B(1), C(2), D(3)
    private GameObject selectedCharacterPrefab; // Apply���� ���� ������ ������

    void Start()
    {
        slot1.onClick.AddListener(() => SelectCharacter(0));  // Boss B A
        slot2.onClick.AddListener(() => SelectCharacter(4));  // Boss A A
        slot3.onClick.AddListener(() => SelectCharacter(8));  // Intern A A
        slot4.onClick.AddListener(() => SelectCharacter(12)); // Intern B A
        slot5.onClick.AddListener(() => SelectCharacter(16)); // Supervisor A A
        slot6.onClick.AddListener(() => SelectCharacter(20)); // Supervisor B A
        slot7.onClick.AddListener(() => SelectCharacter(24)); // Worker A A
        slot8.onClick.AddListener(() => SelectCharacter(28)); // Worker B A

        aButton.onClick.AddListener(() => ChangeColor(0));
        bButton.onClick.AddListener(() => ChangeColor(1));
        cButton.onClick.AddListener(() => ChangeColor(2));
        dButton.onClick.AddListener(() => ChangeColor(3));

        ApplyButton.onClick.AddListener(OnClickApply);
        BackButton.onClick.AddListener(OnClickBack);
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
}
