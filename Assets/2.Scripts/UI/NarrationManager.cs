using System.Collections;
using UnityEngine;
using TMPro;

public class NarrationManager : MonoBehaviour
{
    public GameObject narrationUI;         // UI Panel
    public TextMeshProUGUI narrationText;  // 텍스트 컴포넌트

    public float displayDuration = 5f;

    [Header("Cartoon Voice")]
    public CartoonTTS cartoonTTS;
    public void ShowNarration(string text, float duration = -1f)
    {
        StopAllCoroutines();  // 이전 UI 코루틴 정리
        StartCoroutine(DisplayNarrationWithVoice(text, duration));
    }

    private IEnumerator DisplayNarrationWithVoice(string text, float duration)
    {
        narrationUI.SetActive(true);
        narrationText.text = "";

        foreach (char c in text)
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
    }
}
