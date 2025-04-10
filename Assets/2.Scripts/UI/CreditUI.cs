using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditUI : PopUpUI
{
    public Button backButton;

    void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBackButtonClicked()
    {
        UIManager.Instance.HidePopupUI<CreditUI>();
    }
}
