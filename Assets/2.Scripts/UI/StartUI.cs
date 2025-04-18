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
}
