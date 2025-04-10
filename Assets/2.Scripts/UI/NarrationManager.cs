using System.Collections;
using UnityEngine;
using TMPro;

public class NarrationManager : MonoBehaviour
{
    public GameObject narrationUI;         // UI Panel
    public TextMeshProUGUI narrationText;  // 텍스트 컴포넌트

    public float displayDuration = 5f;

    public void ShowNarration(string text, float duration = -1f)
    {
        StopAllCoroutines();  // 이전 UI 코루틴 정리
        StartCoroutine(DisplayNarration(text, duration));
    }

    private IEnumerator DisplayNarration(string text, float duration)
    {
        narrationUI.SetActive(true);
        narrationText.text = text;

        yield return new WaitForSeconds(duration > 0 ? duration : displayDuration);

        narrationUI.SetActive(false);
    }
}
