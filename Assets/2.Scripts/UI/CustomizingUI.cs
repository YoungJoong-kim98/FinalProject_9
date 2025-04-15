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

    [Header("All character prefabs(순서바꾸면안됨)")]
    public List<GameObject> allCharacterPrefabs;

    public GameObject mainCharacter;

    private int currentCharacterBaseIndex = 0;  // 색상 변경 시 기준이 되는 캐릭터 인덱스
    private int currentColorOffset = 0;         // 현재 선택된 색상 A(0), B(1), C(2), D(3)
    private GameObject selectedCharacterPrefab; // Apply에서 최종 저장할 프리팹

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
        currentColorOffset = 0; // 기본 A 색상
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

        Debug.LogError($"프리팹 인덱스 {index} 가 리스트 범위를 벗어났음");
        return null;
    }

    void ReplaceMainCharacter(GameObject newPrefab)
    {
        if (newPrefab == null || mainCharacter == null) return;

        // 기존 위치, 회전 저장
        Vector3 pos = mainCharacter.transform.position;
        Quaternion rot = mainCharacter.transform.rotation;
        Transform parent = mainCharacter.transform.parent;

        Destroy(mainCharacter);
        mainCharacter = Instantiate(newPrefab, pos, rot, parent);
    }
    void OnClickApply()
    {
        selectedCharacterPrefab = GetCharacterFromIndex(currentCharacterBaseIndex + currentColorOffset);

        Debug.Log($"[Apply] 선택된 캐릭터 프리팹: {selectedCharacterPrefab?.name}");

        //TODO: 플레이어 캐릭터 시스템과 연결할 부분
        //PlayerManager.Instance.SetCharacter(selectedCharacterPrefab);
        this.gameObject.SetActive(false);
    }
}
