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

    private Sequence currentSequence; // ���� �������� ����
    public void Show(string skillName, Sprite image)
    {
        // ���� �������� �ִٸ� ����
        currentSequence?.Kill();

        // �̹��� �� �ؽ�Ʈ ����
        
        skillText.text = skillName.Equals("all") ? $"{skillName} lock": $"{skillName} Unlock";
        skillImage.sprite = image;

        // ���� �ʱ�ȭ
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(true);

        // DOTween �ִϸ��̼� ������ ���� �� ����
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
