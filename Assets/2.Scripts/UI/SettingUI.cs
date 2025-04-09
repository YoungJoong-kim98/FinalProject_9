using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : PopUpUI
{
    [Header("Buttons")]
    public Button displayButton;//���÷��� ��ư
    public Button audioButton;//�������ư
    public Button languageButton;//����ư

    [Header("Display")]
    public GameObject displayobject;//���÷���â
    public TextMeshProUGUI screenCondition;//��üȭ�� �ؽ�Ʈ
    public Button screenConditionBeforeButton;//��üȭ�� �ʱ�ȭ
    public Button screenConditionAfterButton;//��üȭ�� �ʱ�ȭ
    public TextMeshProUGUI mainResolution; //�ػ� �ؽ�Ʈ
    public Button mainResolutionBeforeButton;//�ػ󵵺���
    public Button mainResolutionAfterButton;//�ػ� ����

    [Header("Audio")]
    public GameObject audioobject;//�����â
    public Slider masterSoundController;//�����ͺ�������
    public Slider backGroungMusicController;//���������
    public Slider effectSoundController;//ȿ��������
    public TextMeshProUGUI masterSoundNumber;//�����ͺ�������
    public TextMeshProUGUI backGroundSoundNumber;//���������
    public TextMeshProUGUI effectSoundNumber;//ȿ��������

    [Header("Language")]
    public GameObject languageobject;//���â
    public TextMeshProUGUI changeLanguageName;
    public TextMeshProUGUI languageName;
    public Button beforeLanguageButton;
    public Button afterLanguageButton;

    [Header("Other")]
    public Button settingUIBackButton; //�ڷΰ���
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

    private void MasterSoundControl(float value)//������ ����
    {
        // GameManager.Instance.soundManager.MasterVolumeControl(value);
        masterSoundNumber.text = Mathf.RoundToInt(value * 100).ToString();
    }

    private void BGMSoundControl(float value)//������� 
    {
        // GameManager.Instance.soundManager.BGMVolumeControl(value);
        backGroundSoundNumber.text = Mathf.RoundToInt(value * 100).ToString();
    }

    private void EfSoundControl(float value)//ȿ����
    {
        // GameManager.Instance.soundManager.EfSoundControl(value);
        effectSoundNumber.text = Mathf.RoundToInt(value * 100).ToString();
    }

    private void ActivateDisplayObject()//���÷���â Ȱ��ȭ
    {
        displayobject.SetActive(true);
        audioobject.SetActive(false);
        languageobject.SetActive(false);
        SetButtonColor(displayButton, Color.yellow);
        SetButtonColor(audioButton, originalButtonColor);
        SetButtonColor(languageButton, originalButtonColor);

    }

    private void ActivateAudioObject()//�Ҹ�â Ȱ��ȭ
    {
        audioobject.SetActive(true);
        displayobject.SetActive(false);
        languageobject.SetActive(false);
        SetButtonColor(audioButton, Color.yellow);
        SetButtonColor(displayButton, originalButtonColor);
        SetButtonColor(languageButton, originalButtonColor);
    }

    private void ActivateLanguageObject() //���âȰ��ȭ
    {
        languageobject.SetActive(true);
        displayobject.SetActive(false);
        audioobject.SetActive(false);
        SetButtonColor(languageButton, Color.yellow);
        SetButtonColor(displayButton, originalButtonColor);
        SetButtonColor(audioButton, originalButtonColor);
    }
    private void SetButtonColor(Button button, Color color)//��ư���󺯰��Լ�
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
