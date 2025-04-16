using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillUnlockUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Image skillImage;
    public TextMeshProUGUI skillText;
    public float fadeDuration = 0.5f;
    public float displayTime = 2f;

    public void Show(string skillName, Sprite image)
    {
        // 이미지 및 텍스트 설정
        skillText.text = $"{skillName} Unlock";
        skillImage.sprite = image;

        // 투명도 초기화
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(true);

        // DOTween 애니메이션 시퀀스
        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(1, fadeDuration))
           .AppendInterval(displayTime)
           .Append(canvasGroup.DOFade(0, fadeDuration))
           .OnComplete(() =>
           {
               canvasGroup.gameObject.SetActive(false);
           });
    }
}
