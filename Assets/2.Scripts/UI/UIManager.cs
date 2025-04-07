using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private const string UIResourceFolderPath = "UI/"; //UI���ҽ� ��ġ ���� ��� - ���� ui ���������� ����� ����
    private Dictionary<string, BaseUI> activeUIs = new Dictionary<string, BaseUI>();
    public UIType ShowUI<UIType>() where UIType : BaseUI
    {
        string uiName = typeof(UIType).Name;//UI �̸�- uitype ���� ����

        if (activeUIs.ContainsKey(uiName))//Ȱ��ȭ ui Ȯ��
        {
            activeUIs[uiName].OnShow();
            return activeUIs[uiName] as UIType;
        }
        GameObject uiPrefab = Resources.Load<GameObject>($"{UIResourceFolderPath}{uiName}");

        if (uiPrefab != null)
        {
            return null;
        }
        GameObject uiInstance = Instantiate(uiPrefab, transform); //ui�ν��Ͻ� ����,ȭ��ǥ��
        UIType uiComponent = uiInstance.GetComponent<UIType>();

        if (uiComponent == null)
        {
            Destroy(uiInstance); // UI �ν��Ͻ��� �߸� ������ ��� ����
            return null;
        }

        activeUIs.Add(uiName, uiComponent);
        uiComponent.Initialize();
        uiComponent.OnShow();
        return uiComponent;
    }
    public void HideUI<UIType>() where UIType : BaseUI
    {
        string uiName = typeof(UIType).Name;

        if (activeUIs.TryGetValue(uiName, out BaseUI ui))
        {
            ui.OnHide();
        }
        else
        {
            Debug.LogWarning("UI�� Ȱ��ȭ ���°� �ƴ�");
        }
    }
}
