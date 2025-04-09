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

    private void OnYesClicked()//Yes ��ư
    {
        //��ŸƮUI�� �̵�
        UIManager.Instance.ShowPopupUI<StartUI>();

        // ���� ���� �� �ڵ� ���� ó��
        //SaveManager.Instance.SaveGame();
        //GameManager.Instance.QuitGame();
        Time.timeScale = 1;
        ClosePopup();
    }
    private void OnNoClicked()//No ��ư
    {
        ClosePopup();
        Time.timeScale = 1;
    }
}
