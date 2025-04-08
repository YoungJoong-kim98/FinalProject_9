using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private const string UIResourceFolderPath = "UI/"; //UI���ҽ� ��ġ ���� ��� - ���� ui ���������� ����� ����

    private Dictionary<string, BaseUI> permanentUIs = new Dictionary<string, BaseUI>(); // ��� UI��
    private Dictionary<string, BaseUI> activePopupUIs = new Dictionary<string, BaseUI>(); // �˾� UI��

    public void ShowPermanentUI<UIType>() where UIType : BaseUI//��� UI
    {
        string uiName = typeof(UIType).Name;

        if (permanentUIs.ContainsKey(uiName))//Ȱ��ȭ�Ǿ��ִٸ� �ٽ�ǥ��
        {
            permanentUIs[uiName].OnShow();
        }
        else
        {            
            UIType uiComponent = LoadUI<UIType>(uiName); // ��� UI ���� �ε��ϰ� ȭ�鿡 ǥ��
            if (uiComponent != null)
            {
                uiComponent.Initialize();
                uiComponent.OnShow();
                permanentUIs.Add(uiName, uiComponent); // ��� UI�� ��ųʸ��� �߰�
            }
        }
    }

   
    public void HidePermanentUI<UIType>() where UIType : BaseUI  // ��� UI�� �����
    {
        string uiName = typeof(UIType).Name;

        if (permanentUIs.ContainsKey(uiName))
        {
            permanentUIs[uiName].OnHide(); // ��� UI �����
        }
        else
        {
            Debug.LogWarning($"{uiName} UI�� Ȱ��ȭ�Ǿ� ���� ����");
        }
    }


    public void ShowPopupUI<UIType>() where UIType : BaseUI // �˾� UI�� ǥ��
    {
        string uiName = typeof(UIType).Name;
      
        if (activePopupUIs.ContainsKey(uiName)) //Ȱ��ȭ �Ǿ��ִٸ� �ٽ�ǥ��
        {
            activePopupUIs[uiName].OnShow();
        }
        else
        {
            UIType uiComponent = LoadUI<UIType>(uiName); // �˾� UI ���� �ε��ϰ� ȭ�鿡 ǥ��
            if (uiComponent != null)
            {
                uiComponent.Initialize();
                uiComponent.OnShow();
                activePopupUIs.Add(uiName, uiComponent); // �˾� UI�� ��ųʸ��� �߰�
            }
        }
    }

    public void HidePopupUI<UIType>() where UIType : BaseUI //�˾�UI �����
    {
        string uiName = typeof(UIType).Name;

        if (activePopupUIs.ContainsKey(uiName))
        {
            activePopupUIs[uiName].OnHide(); // �˾� UI �����
            activePopupUIs.Remove(uiName); // �˾� UI ��ųʸ����� ����
        }
        else
        {
            Debug.LogWarning($"{uiName} �˾� UI�� Ȱ��ȭ�Ǿ� ���� ����");
        }
    }

    private UIType LoadUI<UIType>(string uiName) where UIType : BaseUI // UI�� �ε��ϰ� �ν��Ͻ��� ����
    {
        GameObject uiPrefab = Resources.Load<GameObject>($"{UIResourceFolderPath}{uiName}");

        if (uiPrefab == null)
        {
            Debug.LogWarning($"{uiName} UI Prefab�� ã�� �� ����");
            return null;
        }

        GameObject uiInstance = Instantiate(uiPrefab, transform);
        UIType uiComponent = uiInstance.GetComponent<UIType>();

        if (uiComponent == null)
        {
            Destroy(uiInstance);
            return null;
        }

        return uiComponent;
    }
}