using System.Collections;
using UnityEngine;
using TMPro;

public class NarrationManager : MonoBehaviour
{
    public GameObject narrationUI;         // UI Panel
    public TextMeshProUGUI narrationText;  // �ؽ�Ʈ ������Ʈ

    public float displayDuration = 5f;

    public void ShowNarration(string text, float duration = -1f)
    {
        StopAllCoroutines();  // ���� UI �ڷ�ƾ ����
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
