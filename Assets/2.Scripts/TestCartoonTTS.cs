using UnityEngine;

public class TestCartoonTTS : MonoBehaviour
{
    public CartoonTTS cartoonTTS;

    void Start()
    {
        cartoonTTS.PlayCartoonSpeech("안녕하세요! 오늘도 좋은 하루예요!");
    }
}
