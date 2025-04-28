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
    public AchievementSystem achievementSystem;//업적

    [SerializeField] private RuntimeAnimatorController playerAnimatorController; // 플레이어 애니메이터 컨트롤러

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

        if (mainCharacter.name == newPrefab.name) return;

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
        if (!achievementSystem.customizationChangedUnlocked)
        {
            achievementSystem.NotifyCustomizationChanged();
        }
    }
    void OnClickApply()
    {
        //selectedCharacterPrefab = GetCharacterFromIndex(currentCharacterBaseIndex + currentColorOffset);
      
        Debug.Log($"[Apply] 선택된 캐릭터 프리팹: {selectedCharacterPrefab?.name}");

        ApplyCharacter(mainCharacter);

        var startUI = UIManager.Instance.GetPopupUI<StartUI>();
        if (startUI != null && mainCharacter != null)
        {
            startUI.SetCharacterPrefab(mainCharacter);
            startUI.gameObject.SetActive(true);
            
            
        }
        else
        {
            Debug.LogWarning("StartUI 또는 선택된 프리팹이 null");
        }

        this.gameObject.SetActive(false);
    }

    private GameObject currentModel; // 현재 적용된 모델 기억
    public void ApplyCharacter(GameObject selectedPrefab)
    {
        Player player = FindObjectOfType<Player>();
        if (player == null)
        {
            Debug.LogError("Player를 찾을 수 없습니다.");
            return;
        }

        Transform modelParent = player.transform;

        // 현재 적용된 모델이 있으면 삭제
        if (currentModel != null)
        {
            Destroy(currentModel);
        }
        else
        {
            // 처음 한 번은 기존 "Office_worker_Boss_B_D" 삭제
            Transform oldModel = modelParent.Find("Office_worker_Boss_B_D");
            if (oldModel != null)
            {
                Destroy(oldModel.gameObject);
            }
        }

        // 새 모델 생성
        currentModel = Instantiate(selectedPrefab, modelParent);
        currentModel.name = selectedPrefab.name;

        // 위치, 스케일 조정
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localScale = Vector3.one * 3f;

        // 새 모델의 Animator를 Player에 다시 연결
        Animator newAnimator = currentModel.GetComponent<Animator>();
        if (newAnimator != null)
        {
            player.Animator = newAnimator; // 여기가 핵심!
            player.Animator.runtimeAnimatorController = playerAnimatorController; // 애니메이터 컨트롤러도 다시 세팅
        }
        else
        {
            Debug.LogWarning("새 모델에 Animator가 없습니다!");
        }
    }




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
                    isUnlocked = achievementSystem.playTime1HourUnlocked;
                    break;
            }

            slot.isUnlocked = isUnlocked;

            if (isUnlocked && slot.hiddenImage != null)
            {
                Destroy(slot.hiddenImage); // 잠금 이미지 제거
                slot.hiddenImage = null;   // 참조 제거
            }

            slot.UpdateLockVisual(); // 비주얼 상태 업데이트
        }
    }
}
