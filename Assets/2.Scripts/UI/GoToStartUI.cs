using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToStartUI : PopUpUI
{
    public Button yesButton;
    public Button noButton;

    void Start()
    {
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
        Time.timeScale = 0;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    UIManager.Instance.ShowPopupUI<GoToStartUI>();
        //    Time.timeScale = 0;
        //}
    }

    private void OnYesClicked()//Yes 버튼
    {
        //스타트UI로 이동
        UIManager.Instance.ShowPopupUI<StartUI>();

        // 게임 종료 및 자동 저장 처리
        //SaveManager.Instance.SaveGame();
        //GameManager.Instance.QuitGame();
        Time.timeScale = 1;
        ClosePopup();
    }
    private void OnNoClicked()//No 버튼
    {
        ClosePopup();
        Time.timeScale = 1;
    }
}
