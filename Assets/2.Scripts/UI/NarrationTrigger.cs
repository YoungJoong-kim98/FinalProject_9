using UnityEngine;

public class NarrationTrigger : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string narrationText;

    [SerializeField] private float duration = 5f;
    [SerializeField] private bool playOnce = true;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed && playOnce) return;

        if (other.CompareTag("Player"))
        {
            UIManager.Instance.NarrationManager.ShowNarration(narrationText, duration); // ← 여기서 바로 호출!
            hasPlayed = true;
        }
    }

}
