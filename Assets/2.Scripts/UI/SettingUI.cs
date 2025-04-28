using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class SettingUI : PopUpUI
{
    [Header("Buttons")]
    public Button displayButton;//디스플레이 버튼
    public Button audioButton;//오디오버튼
    public Button languageButton;//언어버튼

    [Header("Display")]
    public GameObject displayobject;//디스플레이창
    public TextMeshProUGUI fullScreen;//전체화면 텍스트
    public TextMeshProUGUI fullWindowMode;//전체창 텍스트
    public TextMeshProUGUI findowMode;//창화면 텍스트
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

    private FullScreenMode[] screenModes = {
        FullScreenMode.ExclusiveFullScreen,
        FullScreenMode.FullScreenWindow,
        FullScreenMode.Windowed
    };
    private int currentScreenModeIndex = 0;
    private int previewScreenModeIndex = 0;

    private int previewResolutionIndex = 3;
    private int currentResolutionIndex = 3; // 0: 1024x768, 1: 1280x720, 2: 1600x900, 3: 1920x1080
    private int[] resolutionsWidth = { 1024, 1280, 1600, 1920 };
    private int[] resolutionsHeight = { 768, 720, 900, 1080 };

    private Color originalButtonColor;

    private float savedMasterVolume; //저장된 사운드 설정값
    private float savedBGMVolume;
    private float savedEffectVolume;

    private const float DEFAULT_VOLUME = 1.0f;
    private const int DEFAULT_RESOLUTION_INDEX = 3;
    private const int DEFAULT_SCREEN_MODE_INDEX = 0;

    private int currentLanguageIndex = 0;
    private int previewLanguageIndex = 0;

    private const int DEFAULT_LANGUAGE_INDEX = 0; // 0: Korean

    void Start()
    {
        displayButton.onClick.AddListener(ActivateDisplayObject);
        audioButton.onClick.AddListener(ActivateAudioObject);
        languageButton.onClick.AddListener(ActivateLanguageObject);

        screenConditionBeforeButton.onClick.AddListener(() => ChangeScreenMode(false));
        screenConditionAfterButton.onClick.AddListener(() => ChangeScreenMode(true));

        mainResolutionBeforeButton.onClick.AddListener(DecreaseResolution);
        mainResolutionAfterButton.onClick.AddListener(IncreaseResolution);

        masterSoundController.onValueChanged.AddListener(MasterSoundControl);
        backGroungMusicController.onValueChanged.AddListener(BGMSoundControl);
        effectSoundController.onValueChanged.AddListener(EfSoundControl);

        settingUIBackButton.onClick.AddListener(OnClickedBackButton);
        settingUIApplyButton.onClick.AddListener(OnClickedApplyButton);
        settingUIDefalutValueButton.onClick.AddListener(OnClickedDefaultButton);

        beforeLanguageButton.onClick.AddListener(() => ChangeLanguage(false));
        afterLanguageButton.onClick.AddListener(() => ChangeLanguage(true));

        // 초기 볼륨 동기화
        var sound = GameManager.Instance.SoundManager;
        savedMasterVolume = sound.MasterVolume;
        savedBGMVolume = sound.BackgroundMusicVolume;
        savedEffectVolume = sound.EffectSoundVolume;

        masterSoundController.value = savedMasterVolume;
        backGroungMusicController.value = savedBGMVolume;
        effectSoundController.value = savedEffectVolume;

        sound.SetMasterVolume(savedMasterVolume);
        sound.SetBackgroundMusicVolume(savedBGMVolume);
        sound.SetEffectSoundVolume(savedEffectVolume);

        MasterSoundControl(savedMasterVolume);
        BGMSoundControl(savedBGMVolume);
        EfSoundControl(savedEffectVolume);

        previewResolutionIndex = currentResolutionIndex;
        previewScreenModeIndex = currentScreenModeIndex;

        PreviewResolution();
        PreviewScreenMode();

        originalButtonColor = Color.black;

        // 디스플레이가 처음 열림
        displayobject.SetActive(true);
        audioobject.SetActive(false);
        languageobject.SetActive(false);

        SetButtonColor(displayButton, Color.yellow);
        SetButtonColor(audioButton, originalButtonColor);
        SetButtonColor(languageButton, originalButtonColor);

        // 현재 언어 인덱스를 Localization에서 가져옴
        Locale currentLocale = LocalizationSettings.SelectedLocale;
        currentLanguageIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(currentLocale);
        previewLanguageIndex = currentLanguageIndex;

        UpdateLanguageText(previewLanguageIndex);
    }

    private void ChangeScreenMode(bool isNext)
    {
        if (isNext)
            previewScreenModeIndex = (previewScreenModeIndex + 1) % screenModes.Length;
        else
            previewScreenModeIndex = (previewScreenModeIndex - 1 + screenModes.Length) % screenModes.Length;

        PreviewScreenMode();
    }

    private void PreviewScreenMode()
    {
        var mode = screenModes[previewScreenModeIndex];
        Screen.fullScreenMode = mode;
        UpdateScreenConditionText(previewScreenModeIndex);
    }

    private void UpdateScreenConditionText(int index)
    {
        fullScreen.gameObject.SetActive(false);
        fullWindowMode.gameObject.SetActive(false);
        findowMode.gameObject.SetActive(false);

        switch (screenModes[index])
        {
            case FullScreenMode.ExclusiveFullScreen:
                fullScreen.gameObject.SetActive(true);
                break;
            case FullScreenMode.FullScreenWindow:
                fullWindowMode.gameObject.SetActive(true);
                break;
            case FullScreenMode.Windowed:
                findowMode.gameObject.SetActive(true);
                break;
        }
    }

    private void DecreaseResolution() //해상도 이전버튼
    {
        currentResolutionIndex--;
        if (currentResolutionIndex < 0)
        {
            currentResolutionIndex = resolutionsWidth.Length - 1; // 마지막 해상도로 순환
        }

        PreviewResolution();
    }

    private void IncreaseResolution()//해상도 다음버튼
    {
        currentResolutionIndex++;
        if (currentResolutionIndex >= resolutionsWidth.Length)
        {
            currentResolutionIndex = 0; // 첫 번째 해상도로 순환
        }

        PreviewResolution();
    }

    private void PreviewResolution()
    {
        int width = resolutionsWidth[previewResolutionIndex];
        int height = resolutionsHeight[previewResolutionIndex];

        Screen.SetResolution(width, height, screenModes[previewScreenModeIndex] != FullScreenMode.Windowed);
        UpdateResolutionText(previewResolutionIndex);
    }
    private void UpdateResolutionText(int index)//해상도 텍스트 업데이트
    {
        int width = resolutionsWidth[currentResolutionIndex];
        int height = resolutionsHeight[currentResolutionIndex];
        mainResolution.text = $"{width} x {height}";
    }

    private void MasterSoundControl(float value)//마스터 볼륨
    {
        GameManager.Instance.SoundManager.SetMasterVolume(value);
        masterSoundNumber.text = Mathf.RoundToInt(value * 100).ToString();
    }

    private void BGMSoundControl(float value)//배경음악 
    {
        GameManager.Instance.SoundManager.SetBackgroundMusicVolume(value);
        backGroundSoundNumber.text = Mathf.RoundToInt(value * 100).ToString();
    }

    private void EfSoundControl(float value)//효과음
    {
        GameManager.Instance.SoundManager.SetEffectSoundVolume(value);
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
        colors.selectedColor = color;
        colors.highlightedColor = color;
        colors.pressedColor = color;
        button.colors = colors;
    }

    private void OnClickedBackButton()
    {
        //사운드 설정 복원
        masterSoundController.value = savedMasterVolume;
        backGroungMusicController.value = savedBGMVolume;
        effectSoundController.value = savedEffectVolume;

        MasterSoundControl(savedMasterVolume);
        BGMSoundControl(savedBGMVolume);
        EfSoundControl(savedEffectVolume);

        // 화면 설정 복원
        previewResolutionIndex = currentResolutionIndex;
        previewScreenModeIndex = currentScreenModeIndex;
        PreviewResolution();
        PreviewScreenMode();

        //언어 설정 복원
        previewLanguageIndex = currentLanguageIndex;
        UpdateLanguageText(previewLanguageIndex);

        UIManager.Instance.HidePopupUI<SettingUI>();
    }

    private void OnClickedApplyButton()
    {
        // 현재 슬라이더 값 저장
        savedMasterVolume = masterSoundController.value;
        savedBGMVolume = backGroungMusicController.value;
        savedEffectVolume = effectSoundController.value;

        currentResolutionIndex = previewResolutionIndex;
        currentScreenModeIndex = previewScreenModeIndex;
        currentLanguageIndex = previewLanguageIndex;

        ApplyResolution();
        ApplyScreenMode();
        ApplyLanguage();

        UIManager.Instance.HidePopupUI<SettingUI>();
    }
    private void ApplyResolution()
    {
        int width = resolutionsWidth[currentResolutionIndex];
        int height = resolutionsHeight[currentResolutionIndex];
        Screen.SetResolution(width, height, screenModes[currentScreenModeIndex] != FullScreenMode.Windowed);
    }

    private void ApplyScreenMode()
    {
        Screen.fullScreenMode = screenModes[currentScreenModeIndex];
    }
    private void ApplyLanguage()
    {
        var selectedLocale = LocalizationSettings.AvailableLocales.Locales[currentLanguageIndex];
        LocalizationSettings.SelectedLocale = selectedLocale;
    }

    private void OnClickedDefaultButton()
    {
        masterSoundController.value = DEFAULT_VOLUME;
        backGroungMusicController.value = DEFAULT_VOLUME;
        effectSoundController.value = DEFAULT_VOLUME;

        MasterSoundControl(DEFAULT_VOLUME);
        BGMSoundControl(DEFAULT_VOLUME);
        EfSoundControl(DEFAULT_VOLUME);

        previewResolutionIndex = DEFAULT_RESOLUTION_INDEX;
        PreviewResolution();

        previewScreenModeIndex = DEFAULT_SCREEN_MODE_INDEX;
        PreviewScreenMode();

        previewLanguageIndex = DEFAULT_LANGUAGE_INDEX;
        UpdateLanguageText(previewLanguageIndex);
    }
    private void ChangeLanguage(bool isNext)
    {
        int count = LocalizationSettings.AvailableLocales.Locales.Count;

        if (isNext)
            previewLanguageIndex = (previewLanguageIndex + 1) % count;
        else
            previewLanguageIndex = (previewLanguageIndex - 1 + count) % count;

        UpdateLanguageText(previewLanguageIndex);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[previewLanguageIndex];
    }

    private void UpdateLanguageText(int index)
    {
        var locale = LocalizationSettings.AvailableLocales.Locales[index];
        languageName.text = locale.LocaleName;
    }
}
