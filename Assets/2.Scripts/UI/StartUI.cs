using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : PopUpUI
{
    public Button gameplayButton;
    public Button loadGameButton;
    public Button settingButton;
    public Button creditButton;
    public Button gameOverButton;
    public Button customButton;
    public GameObject startCharacter;
    public RuntimeAnimatorController defaultAnimatorController;

    void Start()
    {
        gameplayButton.onClick.AddListener(OnGameplayButtonClicked);
        loadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
        settingButton.onClick.AddListener(OnSettingButtonClicked);
        creditButton.onClick.AddListener(OnCreditButtonClicked);
        gameOverButton.onClick.AddListener(OnGameOverButtonClicked);
        customButton.onClick.AddListener(OnCustomButtonClicked);
        InitializeStartCharacterAnimator();
    }

    void Update()
    {

    }
    private void OnCustomButtonClicked()
    {
        this.gameObject.SetActive(false);
        UIManager.Instance.ShowPopupUI<CustomizingUI>();
    }
    private void OnGameplayButtonClicked()
    {
        Debug.Log("게임 시작");
        UIManager.Instance.HidePopupUI<StartUI>();
        UIManager.Instance.ShowPermanentUI<InGameUI>();
    }
    private void OnSettingButtonClicked()
    {
        UIManager.Instance.ShowPopupUI<SettingUI>();
    }

    private void OnLoadGameButtonClicked()
    {
        //로드 ui 제작? 게임매니저를 통해 로드게임제작필요
    }

    private void OnCreditButtonClicked()
    {
        UIManager.Instance.ShowPopupUI<CreditUI>();
    }

    private void OnGameOverButtonClicked()
    {
        // 게임이 실제 빌드에서 실행 중이라면 종료
#if UNITY_EDITOR
        // 에디터에서 실행 중일 때는 게임을 종료하지 않고, 에디터에서 멈추도록 처리
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 실제 빌드에서는 게임 종료
            Application.Quit();
#endif
    }
    public void SetCharacterPrefab(GameObject prefabToSet)
    {
        if (startCharacter == null || prefabToSet == null)
        {
            Debug.LogWarning("StartCharacter 또는 전달된 프리팹이 null입니다.");
            return;
        }

        Transform startCharacterTransform = startCharacter.transform;
        Transform oldChild = startCharacterTransform.childCount > 0 ? startCharacterTransform.GetChild(0) : null;

        Vector3 localPos = Vector3.zero;
        Quaternion localRot = Quaternion.identity;
        Vector3 localScale = Vector3.one;

        if (oldChild != null)
        {
            localPos = oldChild.localPosition;
            localRot = oldChild.localRotation;
            localScale = oldChild.localScale;
            Destroy(oldChild.gameObject);
        }

        // 새 캐릭터 생성
        GameObject newCharacter = Instantiate(prefabToSet, startCharacterTransform);
        newCharacter.transform.localPosition = localPos;
        newCharacter.transform.localRotation = localRot;
        newCharacter.transform.localScale = localScale;

        // 애니메이터 설정
        Animator animator = newCharacter.GetComponent<Animator>();
        if (animator != null && defaultAnimatorController != null)
        {
            animator.runtimeAnimatorController = defaultAnimatorController;
        }

        Debug.Log("[StartUI] 캐릭터 프리팹 적용 및 애니메이션 설정 완료");
    }
    private void InitializeStartCharacterAnimator()
    {
        if (startCharacter == null || defaultAnimatorController == null) return;

        if (startCharacter.transform.childCount > 0)
        {
            var child = startCharacter.transform.GetChild(0);
            var animator = child.GetComponent<Animator>();

            if (animator != null)
            {
                StartCoroutine(AssignAnimatorNextFrame(animator));
            }
        }
    }private IEnumerator AssignAnimatorNextFrame(Animator animator)
{
    yield return null; // 프레임 한 번 대기
    animator.runtimeAnimatorController = defaultAnimatorController;
    animator.Rebind(); // 추가로 상태를 재설정
    animator.Update(0f); // 즉시 반영

    Debug.Log("[StartUI] Animator 지연 할당 및 Rebind 완료");
}
}
