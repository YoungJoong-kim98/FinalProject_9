using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class NarrationManager : MonoBehaviour
{
    public GameObject narrationUI;
    public TextMeshProUGUI narrationText;
    public float displayDuration = 5f;
    public CartoonTTS cartoonTTS;

    private Coroutine currentRoutine;

    private void Start()
    {
        narrationUI.SetActive(false);
    }
    public void ShowNarration(string key, float duration = -1f)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowLocalizedNarration(key, duration));
    }

    private IEnumerator ShowLocalizedNarration(string key, float duration)
    {
        // 번역된 문자열 요청
        var table = LocalizationSettings.StringDatabase;
        var localizedString = table.GetLocalizedStringAsync("NarrationTable", key);  // "NarrationTable"은 Table 이름
        yield return localizedString;

        string result = localizedString.Result;

        narrationUI.SetActive(true);
        narrationText.text = "";

        foreach (char c in result)
        {
            if (char.IsWhiteSpace(c))
            {
                narrationText.text += c;
                continue;
            }

            cartoonTTS.PlayCartoonSpeech(c.ToString());
            narrationText.text += c;
            yield return new WaitForSeconds(cartoonTTS.interval);
        }

        yield return new WaitForSeconds(duration > 0 ? duration : displayDuration);
        narrationUI.SetActive(false);
        currentRoutine = null;
    }
}
