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

    void Start()
    {
        gameplayButton.onClick.AddListener(OnGameplayButtonClicked);
        loadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
        settingButton.onClick.AddListener(OnSettingButtonClicked);
        creditButton.onClick.AddListener(OnCreditButtonClicked);
        gameOverButton.onClick.AddListener(OnGameOverButtonClicked);
        customButton.onClick.AddListener(OnCustomButtonClicked);
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
}
