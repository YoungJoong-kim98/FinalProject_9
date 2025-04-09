using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private const string UIResourceFolderPath = "UI/"; //UI리소스 위치 폴더 경로 - 이후 ui 프리팹으로 만들어 관리

    private Dictionary<string, BaseUI> permanentUIs = new Dictionary<string, BaseUI>(); // 상시 UI들
    private Dictionary<string, BaseUI> activePopupUIs = new Dictionary<string, BaseUI>(); // 팝업 UI들

    void Start()
    {
        InitializeUI();
    }
    private void Awake()
    {
        Instance = this;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPopupUI<GoToStartUI>();
        }
    }

    private void InitializeUI()
    {
        // 모든 UI 비활성화
        foreach (var ui in permanentUIs.Values)
        {
            ui.gameObject.SetActive(false);
        }
        foreach (var ui in activePopupUIs.Values)
        {
            ui.gameObject.SetActive(false);
        }

        // 게임 시작 시 StartUI만 활성화
        ShowPopupUI<StartUI>();
    }
    public void ShowPermanentUI<UIType>() where UIType : BaseUI//상시 UI표시
    {
        string uiName = typeof(UIType).Name;

        if (permanentUIs.ContainsKey(uiName))//활성화되어있다면 다시표시
        {
            permanentUIs[uiName].OnShow();
        }
        else
        {            
            UIType uiComponent = LoadUI<UIType>(uiName); // 상시 UI 새로 로드하고 화면에 표시
            if (uiComponent != null)
            {
                uiComponent.Initialize();
                uiComponent.OnShow();
                permanentUIs.Add(uiName, uiComponent); // 상시 UI로 딕셔너리에 추가
            }
        }
    }

   
    public void HidePermanentUI<UIType>() where UIType : BaseUI  // 상시 UI를 숨기기
    {
        string uiName = typeof(UIType).Name;

        if (permanentUIs.ContainsKey(uiName))
        {
            permanentUIs[uiName].OnHide(); // 상시 UI 숨기기
        }
        else
        {
            Debug.LogWarning($"{uiName} UI는 활성화되어 있지 않음");
        }
    }


    public void ShowPopupUI<UIType>() where UIType : BaseUI // 팝업 UI를 표시
    {
        string uiName = typeof(UIType).Name;
      
        if (activePopupUIs.ContainsKey(uiName)) //활성화 되어있다면 다시표시
        {
            activePopupUIs[uiName].OnShow();
        }
        else
        {
            UIType uiComponent = LoadUI<UIType>(uiName); // 팝업 UI 새로 로드하고 화면에 표시
            if (uiComponent != null)
            {
                uiComponent.Initialize();
                uiComponent.OnShow();
                activePopupUIs.Add(uiName, uiComponent); // 팝업 UI로 딕셔너리에 추가
            }
        }
    }

    public void HidePopupUI<UIType>() where UIType : BaseUI //팝업UI 숨기기
    {
        string uiName = typeof(UIType).Name;

        if (activePopupUIs.ContainsKey(uiName))
        {
            activePopupUIs[uiName].OnHide(); // 팝업 UI 숨기기
            Destroy(activePopupUIs[uiName].gameObject); //팝업ui객체 파괴
            activePopupUIs.Remove(uiName); // 팝업 UI 딕셔너리에서 제거
        }
        else
        {
            Debug.LogWarning($"{uiName} 팝업 UI는 활성화되어 있지 않음");
        }
    }

    private UIType LoadUI<UIType>(string uiName) where UIType : BaseUI // UI를 로드하고 인스턴스를 생성
    {
        GameObject uiPrefab = Resources.Load<GameObject>($"{UIResourceFolderPath}{uiName}");

        if (uiPrefab == null)
        {
            Debug.LogWarning($"{uiName} UI Prefab을 찾을 수 없음");
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