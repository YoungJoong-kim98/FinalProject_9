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
        Debug.Log("���� ����");
        UIManager.Instance.HidePopupUI<StartUI>();
        UIManager.Instance.ShowPermanentUI<InGameUI>();
    }
    private void OnSettingButtonClicked()
    {
        UIManager.Instance.ShowPopupUI<SettingUI>();
    }

    private void OnLoadGameButtonClicked()
    {
        //�ε� ui ����? ���ӸŴ����� ���� �ε���������ʿ�
    }

    private void OnCreditButtonClicked()
    {
        UIManager.Instance.ShowPopupUI<CreditUI>();
    }

    private void OnGameOverButtonClicked()
    {
        // ������ ���� ���忡�� ���� ���̶�� ����
#if UNITY_EDITOR
        // �����Ϳ��� ���� ���� ���� ������ �������� �ʰ�, �����Ϳ��� ���ߵ��� ó��
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // ���� ���忡���� ���� ����
            Application.Quit();
#endif
    }
    public void SetCharacterPrefab(GameObject prefabToSet)
    {
        if (startCharacter == null || prefabToSet == null)
        {
            Debug.LogWarning("StartCharacter �Ǵ� ���޵� �������� null�Դϴ�.");
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

        // �� ĳ���� ����
        GameObject newCharacter = Instantiate(prefabToSet, startCharacterTransform);
        newCharacter.transform.localPosition = localPos;
        newCharacter.transform.localRotation = localRot;
        newCharacter.transform.localScale = localScale;

        // �ִϸ����� ����
        Animator animator = newCharacter.GetComponent<Animator>();
        if (animator != null && defaultAnimatorController != null)
        {
            animator.runtimeAnimatorController = defaultAnimatorController;
        }

        Debug.Log("[StartUI] ĳ���� ������ ���� �� �ִϸ��̼� ���� �Ϸ�");
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
    yield return null; // ������ �� �� ���
    animator.runtimeAnimatorController = defaultAnimatorController;
    animator.Rebind(); // �߰��� ���¸� �缳��
    animator.Update(0f); // ��� �ݿ�

    Debug.Log("[StartUI] Animator ���� �Ҵ� �� Rebind �Ϸ�");
}
}
