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
    public TextMeshProUGUI screenCondition;//��üȭ�� �ؽ�Ʈ
    public Button screenConditionBeforeButton;//��üȭ�� �ʱ�ȭ
    public Button screenConditionAfterButton;//��üȭ�� �ʱ�ȭ
    public TextMeshProUGUI mainResolution; //�ػ� �ؽ�Ʈ
    public Button mainResolutionBeforeButton;//�ػ󵵺���
    public Button mainResolutionAfterButton;//�ػ� ����

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
    
    private void UpdateScreenConditionText() //ȭ�� �ؽ�Ʈ ������Ʈ
    {
        screenCondition.text = isFullScreen ? "��ü ȭ��" : "â ȭ��";
    }
    private void DecreaseResolution() //�ػ� ������ư
    {
        currentResolutionIndex--;
        if (currentResolutionIndex < 0)
        {
            currentResolutionIndex = resolutionsWidth.Length - 1; // ������ �ػ󵵷� ��ȯ
        }

        ApplyResolution();
    }

    private void IncreaseResolution()//�ػ� ������ư
    {
        currentResolutionIndex++;
        if (currentResolutionIndex >= resolutionsWidth.Length)
        {
            currentResolutionIndex = 0; // ù ��° �ػ󵵷� ��ȯ
        }

        ApplyResolution();
    }

    private void ApplyResolution()// �ػ� ����
    {
        int width = resolutionsWidth[currentResolutionIndex];
        int height = resolutionsHeight[currentResolutionIndex];

        Screen.SetResolution(width, height, isFullScreen);
        UpdateResolutionText();
    }
    private void UpdateResolutionText()//�ػ� �ؽ�Ʈ ������Ʈ
    {
        int width = resolutionsWidth[currentResolutionIndex];
        int height = resolutionsHeight[currentResolutionIndex];
        mainResolution.text = $"{width} x {height}";
    }
}
