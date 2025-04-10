using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 필드: 배경음악(BGM)과 효과음(SFX)의 리스트
    [SerializeField] List<AudioClip> bgms = new List<AudioClip>();
    [SerializeField] List<AudioClip> sfxs = new List<AudioClip>();

    // 필드: BGM과 SFX의 오디오 소스(AudioSource)
    [SerializeField] AudioSource audioBGM;
    [SerializeField] AudioSource audioSfx;

    private float masterVolume = 1f; // 마스터 볼륨 (0.0f ~ 1.0f)
    private float backgroundMusicVolume = 1f; // 배경 음악 볼륨
    private float effectSoundVolume = 1f; // 효과음 볼륨

    private void Start()
    {
        // 초기 볼륨 설정
        SetMasterVolume(masterVolume);
        SetBackgroundMusicVolume(backgroundMusicVolume);
        SetEffectSoundVolume(effectSoundVolume);
    }


    public void SetMasterVolume(float volume)//마스터 볼륨 설정
    {

    }
    public void SetBackgroundMusicVolume(float volume)//배경음악 볼륨 설정
    {

    }

    public void SetEffectSoundVolume(float volume)//효과음 성정
    {

    }

    public void PlayBGM(int index) //배경음악 재생
    {

    }

    public void PauseBGM() //배경음악 일시정지
    {

    }

    public void ResumeBGM() //배경음악 재개
    {

    }
    public void StopBGM() //배경음악 멈춤
    {

    }
    public void PlayEfs(int index) //효과음 재생
    {

    }
}
