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

    [Header("All character prefabs(순서바꾸면안됨)")]
    public List<GameObject> allCharacterPrefabs;
    public GameObject mainCharacter;

    private int currentCharacterBaseIndex = 0;  // 색상 변경 시 기준이 되는 캐릭터 인덱스
    private int currentColorOffset = 0;         // 현재 선택된 색상 A(0), B(1), C(2), D(3)
    private GameObject selectedCharacterPrefab; // Apply에서 최종 저장할 프리팹
    public RuntimeAnimatorController fallingPlayerAnimatorController;// FallingPlayer 애니메이션 컨트롤러

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

            Debug.Log($"[Setup] 슬롯 {i} → BaseIndex: {capturedSlot.characterBaseIndex}");

            capturedSlot.slotButton.onClick.RemoveAllListeners();
            // 버튼 클릭 시 캐릭터 선택
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
        Vector3 currentPosition = mainCharacter.transform.position;
        Quaternion currentRotation = mainCharacter.transform.rotation;
        Vector3 currentScale = mainCharacter.transform.localScale; // 스케일 정보 저장
        Transform currentParent = mainCharacter.transform.parent; // 부모 정보 저장

        // 기존 캐릭터 파괴
        Destroy(mainCharacter);

        // 새 캐릭터 인스턴스화
        mainCharacter = Instantiate(newPrefab, currentPosition, currentRotation, currentParent);

        // 새 캐릭터의 스케일을 기본값으로 설정
        mainCharacter.transform.localScale = currentScale;

        // 새 캐릭터에 Animator가 있을 경우 애니메이터 컨트롤러 변경
        Animator animator = mainCharacter.GetComponent<Animator>();
        if (animator != null && fallingPlayerAnimatorController != null)
        { 
            animator.runtimeAnimatorController = fallingPlayerAnimatorController; // 애니메이터 컨트롤러 변경
        }
    }
    void OnClickApply()
    {
        selectedCharacterPrefab = GetCharacterFromIndex(currentCharacterBaseIndex + currentColorOffset);

        Debug.Log($"[Apply] 선택된 캐릭터 프리팹: {selectedCharacterPrefab?.name}");

        var startUI = UIManager.Instance.GetPopupUI<StartUI>();
        if (startUI != null && selectedCharacterPrefab != null && startUI.startCharacter != null)
        {
            Transform startCharacterTransform = startUI.startCharacter.transform;

            // 기존 자식(캐릭터 프리팹) 가져오기
            Transform oldChild = startCharacterTransform.childCount > 0 ? startCharacterTransform.GetChild(0) : null;

            // 기본값: startCharacter 위치 기준
            Vector3 localPos = Vector3.zero;
            Quaternion localRot = Quaternion.identity;
            Vector3 localScale = Vector3.one;

            if (oldChild != null)
            {
                localPos = oldChild.localPosition;
                localRot = oldChild.localRotation;
                localScale = oldChild.localScale;

                Destroy(oldChild.gameObject); // 기존 캐릭터 제거
            }

            // 새 캐릭터 생성
            GameObject newCharacter = Instantiate(selectedCharacterPrefab, startCharacterTransform);

            // 저장된 localTransform 값 적용
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
    void ShowUnlockInfo(CharacterSlot slot)
    {
        Debug.Log($"[잠금] 슬롯 {slot.characterBaseIndex}는 아직 해금되지 않았음");
       
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
