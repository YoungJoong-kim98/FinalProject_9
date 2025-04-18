using UnityEngine;

public class CartoonTTS : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] blipSounds;
    public float interval = 0.07f;

    public void PlayCartoonSpeech(string text)
    {
        // 한 글자만 소리낸다고 가정
        if (string.IsNullOrWhiteSpace(text)) return;

        audioSource.pitch = Random.Range(0.9f, 1.2f);
        audioSource.PlayOneShot(blipSounds[Random.Range(0, blipSounds.Length)]);
    }
}
