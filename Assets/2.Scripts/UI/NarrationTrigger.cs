using UnityEngine;

public class NarrationTrigger : MonoBehaviour
{
    [Header("Localization Key")]
    [SerializeField] private string localizationKey; // <- 키값만 받도록 변경!

    [SerializeField] private float duration = 5f;
    [SerializeField] private bool playOnce = true;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed && playOnce) return;

        if (other.CompareTag("Player"))
        {
            // 키값만 넘기면 알아서 언어에 맞게 번역해서 출력됨!
            UIManager.Instance.NarrationManager.ShowNarration(localizationKey, duration);
            hasPlayed = true;
        }
    }
}
