using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUI : BaseUI
{
    //�˾� ui Ưȭ��� �߰�����
    public virtual void ClosePopup()
    {
        // �˾� UI �ݱ� ����
        gameObject.SetActive(false);
    }
}
