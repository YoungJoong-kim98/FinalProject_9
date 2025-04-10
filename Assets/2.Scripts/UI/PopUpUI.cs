using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUI : BaseUI
{
    //팝업 ui 특화기능 추가가능
    public virtual void ClosePopup()
    {
        // 팝업 UI 닫기 로직
        gameObject.SetActive(false);
    }
}
