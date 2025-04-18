using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : BaseUI
{
    public TextMeshProUGUI timetableText;
    private float elapsedTime = 0f;//��� �ð�
    private float previousTime = 0f;//�����ð�
    public Button bGM;
    public Button eFS;
    private SoundManager soundManager;
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
        elapsedTime += Time.deltaTime;
        if (elapsedTime - previousTime >= 1f)
        {
            previousTime = Mathf.Floor(elapsedTime);
            UpdateTimetableText();
        }
    }
    private void UpdateTimetableText()
    {
        int hours = Mathf.FloorToInt(elapsedTime / 3600); // ��
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60); // ��
        int seconds = Mathf.FloorToInt(elapsedTime % 60); //��
        timetableText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
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
