using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : BaseUI
{
    [Header("Buttons")]
    public Button displayButton;
    public Button audioButton;
    public Button languageButton;

    [Header("Display")]
    public GameObject displayobject;
    public TextMeshProUGUI screenCondition;//전체화면 텍스트
    public Button screenConditionBeforeButton;//전체화면 초기화
    public Button screenConditionAfterButton;//전체화면 초기화
    public TextMeshProUGUI mainResolution; //해상도 텍스트
    public Button mainResolutionBeforeButton;//해상도변경
    public Button mainResolutionAfterButton;//해상도 변경

    [Header("Audio")]
    public GameObject audioobject;
    public Slider masterSoundController;
    public Slider backGroungMusicController;
    public Slider effectSoundController;
    public TextMeshProUGUI masterSoundNumber;
    public TextMeshProUGUI backGroundSoundNumber;
    public TextMeshProUGUI effectSoundNumber;

    [Header("Language")]
    public GameObject languageobject;
    public TextMeshProUGUI changeLanguageName;
    public TextMeshProUGUI languageName;
    public Button beforeLanguageButton;
    public Button afterLanguageButton;

    [Header("Other")]
    public Button settingUIBackButton;
    public Button settingUIDefalutValueButton;
    public Button settingUIApplyButton;

    private bool isFullScreen = false;
    private int currentResolutionIndex = 3; // 0: 1024x768, 1: 1280x720, 2: 1600x900, 3: 1920x1080
    private int[] resolutionsWidth = { 1024, 1280, 1600, 1920 };
    private int[] resolutionsHeight = { 768, 720, 900, 1080 };

    void Start()
    {
        screenConditionBeforeButton.onClick.AddListener(ToggleFullScreen);
        screenConditionAfterButton.onClick.AddListener(ToggleFullScreen);

        mainResolutionBeforeButton.onClick.AddListener(DecreaseResolution);
        mainResolutionAfterButton.onClick.AddListener(IncreaseResolution);

        UpdateScreenConditionText();
        UpdateResolutionText();
    }

    private void ToggleFullScreen()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        UpdateScreenConditionText();
    }
    
    private void UpdateScreenConditionText() //화면 텍스트 업데이트
    {
        screenCondition.text = isFullScreen ? "전체 화면" : "창 화면";
    }
    private void DecreaseResolution() //해상도 이전버튼
    {
        currentResolutionIndex--;
        if (currentResolutionIndex < 0)
        {
            currentResolutionIndex = resolutionsWidth.Length - 1; // 마지막 해상도로 순환
        }

        ApplyResolution();
    }

    private void IncreaseResolution()//해상도 다음버튼
    {
        currentResolutionIndex++;
        if (currentResolutionIndex >= resolutionsWidth.Length)
        {
            currentResolutionIndex = 0; // 첫 번째 해상도로 순환
        }

        ApplyResolution();
    }

    private void ApplyResolution()// 해상도 적용
    {
        int width = resolutionsWidth[currentResolutionIndex];
        int height = resolutionsHeight[currentResolutionIndex];

        Screen.SetResolution(width, height, isFullScreen);
        UpdateResolutionText();
    }
    private void UpdateResolutionText()//해상도 텍스트 업데이트
    {
        int width = resolutionsWidth[currentResolutionIndex];
        int height = resolutionsHeight[currentResolutionIndex];
        mainResolution.text = $"{width} x {height}";
    }
}
