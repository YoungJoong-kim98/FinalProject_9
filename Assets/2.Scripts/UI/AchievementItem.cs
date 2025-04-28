using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementItem : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Image icon; 

    public void SetData(string title, string description, bool achieved)
    {
        titleText.text = title;
        descriptionText.text = description;
        icon.color = achieved ? Color.green : Color.gray;
    }
}
