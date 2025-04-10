using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : BaseUI
{
    public TextMeshProUGUI timetableText;
    private float elapsedTime = 0f;//경과 시간
    private float previousTime = 0f;//이전시간
    void Start()
    {
        UpdateTimetableText();
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
        int hours = Mathf.FloorToInt(elapsedTime / 3600); // 시
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60); // 분
        int seconds = Mathf.FloorToInt(elapsedTime % 60); //초
        timetableText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }
}
