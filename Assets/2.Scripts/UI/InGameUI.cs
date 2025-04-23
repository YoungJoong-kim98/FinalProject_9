using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : BaseUI
{
    public TextMeshProUGUI timetableText;
    private float elapsedTime = 0f;//경과 시간
    private float previousTime = 0f;//이전시간
    private float timeAccumulator = 0f;
    public Button bGM;
    public Button eFS;
    private SoundManager soundManager;
    public GameObject operateImage;
    public TextMeshProUGUI operateTxt;
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        UpdateTimetableText();
        bGM.onClick.AddListener(OnclickedbGM);
        eFS.onClick.AddListener(OnclickedeFS);
    }

    // Update is called once per frame
    void Update()
    {
        timeAccumulator += Time.deltaTime;
        if (timeAccumulator >= 1f)
        {
            elapsedTime += timeAccumulator;
            timeAccumulator = 0f;
            UpdateTimetableText();
        }
    }
    private void UpdateTimetableText()
    {
        int hours = Mathf.FloorToInt(elapsedTime / 3600); // 시
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60); // 분
        int seconds = Mathf.FloorToInt(elapsedTime % 60); //초
        timetableText.text = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", hours, minutes, seconds);
    }
    public void OnclickedbGM()
    {
        soundManager?.PlayBGM(0);
    }
    public void OnclickedeFS()
    {
        soundManager?.PlaySFX("Glass");
    }
}
