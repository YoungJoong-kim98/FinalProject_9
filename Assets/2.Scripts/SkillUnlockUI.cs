using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillUnlockUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Image skillImage;
    public TextMeshProUGUI skillText;
    public float fadeDuration = 1.5f;
    public float displayTime = 3.5f;

    private Sequence currentSequence; // 현재 시퀀스를 저장
    public void Show(string skillName, Sprite image)
    {
        // 기존 시퀀스가 있다면 제거
        currentSequence?.Kill();

        // 이미지 및 텍스트 설정
        
        skillText.text = skillName.Equals("all") ? $"{skillName} lock": $"{skillName} Unlock";
        skillImage.sprite = image;

        // 투명도 초기화
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(true);

        // DOTween 애니메이션 시퀀스 생성 및 저장
        currentSequence = DOTween.Sequence();
        currentSequence.Append(canvasGroup.DOFade(1, fadeDuration))
                       .AppendInterval(displayTime)
                       .Append(canvasGroup.DOFade(0, fadeDuration))
                       .OnComplete(() =>
                       {
                           canvasGroup.gameObject.SetActive(false);
                       });
    }
}
