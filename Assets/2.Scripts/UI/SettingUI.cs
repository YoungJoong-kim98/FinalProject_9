using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : PopUpUI
{
    [Header("Buttons")]
    public Button displayButton;//디스플레이 버튼
    public Button audioButton;//오디오버튼
    public Button languageButton;//언어버튼

    [Header("Display")]
    public GameObject displayobject;//디스플레이창
    public TextMeshProUGUI screenCondition;//전체화면 텍스트
    public Button screenConditionBeforeButton;//전체화면 초기화
    public Button screenConditionAfterButton;//전체화면 초기화
    public TextMeshProUGUI mainResolution; //해상도 텍스트
    public Button mainResolutionBeforeButton;//해상도변경
    public Button mainResolutionAfterButton;//해상도 변경

    [Header("Audio")]
    public GameObject audioobject;//오디오창
    public Slider masterSoundController;//마스터볼륨조절
    public Slider backGroungMusicController;//배경음조절
    public Slider effectSoundController;//효과음조절
    public TextMeshProUGUI masterSoundNumber;//마스터볼륨숫자
    public TextMeshProUGUI backGroundSoundNumber;//배경음숫자
    public TextMeshProUGUI effectSoundNumber;//효과음숫자

    [Header("Language")]
    public GameObject languageobject;//언어창
    public TextMeshProUGUI changeLanguageName;
    public TextMeshProUGUI languageName;
    public Button beforeLanguageButton;
    public Button afterLanguageButton;

    [Header("Other")]
    public Button settingUIBackButton; //뒤로가기
    public Button settingUIDefalutValueButton;
    public Button settingUIApplyButton;

    private bool isFullScreen = false;
    private int currentResolutionIndex = 3; // 0: 1024x768, 1: 1280x720, 2: 1600x900, 3: 1920x1080
    private int[] resolutionsWidth = { 1024, 1280, 1600, 1920 };
    private int[] resolutionsHeight = { 768, 720, 900, 1080 };

    private Color originalButtonColor;

    void Start()
    {
        displayButton.onClick.AddListener(ActivateDisplayObject);
        audioButton.onClick.AddListener(ActivateAudioObject);
        languageButton.onClick.AddListener(ActivateLanguageObject);

        screenConditionBeforeButton.onClick.AddListener(ToggleFullScreen);
        screenConditionAfterButton.onClick.AddListener(ToggleFullScreen);

        mainResolutionBeforeButton.onClick.AddListener(DecreaseResolution);
        mainResolutionAfterButton.onClick.AddListener(IncreaseResolution);

        masterSoundController.onValueChanged.AddListener(MasterSoundControl);
        backGroungMusicController.onValueChanged.AddListener(BGMSoundControl);
        effectSoundController.onValueChanged.AddListener(EfSoundControl);

        settingUIBackButton.onClick.AddListener(OnClickedBackButton);

        UpdateScreenConditionText();
        UpdateResolutionText();
        originalButtonColor = Color.green;
        displayobject.SetActive(true);
        audioobject.SetActive(false);
        languageobject.SetActive(false);
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

    private void MasterSoundControl(float value)//마스터 볼륨
    {
        // GameManager.Instance.soundManager.MasterVolumeControl(value);
        masterSoundNumber.text = Mathf.RoundToInt(value * 100).ToString();
    }

    private void BGMSoundControl(float value)//배경음악 
    {
        // GameManager.Instance.soundManager.BGMVolumeControl(value);
        backGroundSoundNumber.text = Mathf.RoundToInt(value * 100).ToString();
    }

    private void EfSoundControl(float value)//효과음
    {
        // GameManager.Instance.soundManager.EfSoundControl(value);
        effectSoundNumber.text = Mathf.RoundToInt(value * 100).ToString();
    }

    private void ActivateDisplayObject()//디스플레이창 활성화
    {
        displayobject.SetActive(true);
        audioobject.SetActive(false);
        languageobject.SetActive(false);
        SetButtonColor(displayButton, Color.yellow);
        SetButtonColor(audioButton, originalButtonColor);
        SetButtonColor(languageButton, originalButtonColor);

    }

    private void ActivateAudioObject()//소리창 활성화
    {
        audioobject.SetActive(true);
        displayobject.SetActive(false);
        languageobject.SetActive(false);
        SetButtonColor(audioButton, Color.yellow);
        SetButtonColor(displayButton, originalButtonColor);
        SetButtonColor(languageButton, originalButtonColor);
    }

    private void ActivateLanguageObject() //언어창활성화
    {
        languageobject.SetActive(true);
        displayobject.SetActive(false);
        audioobject.SetActive(false);
        SetButtonColor(languageButton, Color.yellow);
        SetButtonColor(displayButton, originalButtonColor);
        SetButtonColor(audioButton, originalButtonColor);
    }
    private void SetButtonColor(Button button, Color color)//버튼색상변경함수
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        //colors.selectedColor = color;
        //colors.highlightedColor = color;
        //colors.pressedColor = color;
        button.colors = colors;
    }

    private void OnClickedBackButton()
    {
        UIManager.Instance.HidePopupUI<SettingUI>();
    }
}
