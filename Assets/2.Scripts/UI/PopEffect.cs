using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopEffect : MonoBehaviour
{
    public float duration = 0.5f;
    public Vector3 startOffset = new Vector3(0, -1f, 0);
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Coroutine currentRoutine;
    private Transform playerTransform;

    private void Awake()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.position = originalPosition + startOffset;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    public void Show()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(Animate(true));
    }

    public void Hide()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(Animate(false));
    }

    private System.Collections.IEnumerator Animate(bool showing)
    {
        float time = 0f;
        Vector3 startPos = showing ? originalPosition + startOffset : originalPosition;
        Vector3 endPos = showing ? originalPosition : originalPosition + startOffset;

        Vector3 startScale = showing ? Vector3.zero : originalScale;
        Vector3 endScale = showing ? originalScale : Vector3.zero;

        while (time < duration)
        {
            float t = time / duration;
            float easedT = EaseOutBack(t);
            transform.position = Vector3.Lerp(startPos, endPos, easedT);
            transform.localScale = Vector3.Lerp(startScale, endScale, easedT);
            time += Time.deltaTime;

            if (showing && playerTransform != null)
            {
                Vector3 directionToPlayer = playerTransform.position - transform.position;
                directionToPlayer.y = 0;  // 수직으로 회전하지 않도록 y값을 0으로 설정
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
            yield return null;
        }

        transform.position = endPos;
        transform.localScale = endScale;
    }

    // Ease Out Back 함수 (t는 0~1 범위)
    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }
}
