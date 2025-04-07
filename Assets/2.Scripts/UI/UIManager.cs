using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private const string UIResourceFolderPath = "UI/"; //UI리소스 위치 폴더 경로 - 이후 ui 프리팹으로 만들어 관리
    private Dictionary<string, BaseUI> activeUIs = new Dictionary<string, BaseUI>();
    public UIType ShowUI<UIType>() where UIType : BaseUI
    {
        string uiName = typeof(UIType).Name;//UI 이름- uitype 통해 얻음

        if (activeUIs.ContainsKey(uiName))//활성화 ui 확인
        {
            activeUIs[uiName].OnShow();
            return activeUIs[uiName] as UIType;
        }
        GameObject uiPrefab = Resources.Load<GameObject>($"{UIResourceFolderPath}{uiName}");

        if (uiPrefab != null)
        {
            return null;
        }
        GameObject uiInstance = Instantiate(uiPrefab, transform); //ui인스턴스 생성,화면표시
        UIType uiComponent = uiInstance.GetComponent<UIType>();

        if (uiComponent == null)
        {
            Destroy(uiInstance); // UI 인스턴스를 잘못 생성한 경우 삭제
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
            Debug.LogWarning("UI가 활성화 상태가 아님");
        }
    }
}
