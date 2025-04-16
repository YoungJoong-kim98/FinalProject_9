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
        // �̹��� �� �ؽ�Ʈ ����
        skillText.text = $"{skillName} Unlock";
        skillImage.sprite = image;

        // ���� �ʱ�ȭ
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(true);

        // DOTween �ִϸ��̼� ������
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
