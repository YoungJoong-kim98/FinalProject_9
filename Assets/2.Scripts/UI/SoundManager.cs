using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // �ʵ�: �������(BGM)�� ȿ����(SFX)�� ����Ʈ
    [SerializeField] List<AudioClip> bgms = new List<AudioClip>();
    [SerializeField] List<AudioClip> sfxs = new List<AudioClip>();

    // �ʵ�: BGM�� SFX�� ����� �ҽ�(AudioSource)
    [SerializeField] AudioSource audioBGM;
    [SerializeField] AudioSource audioSfx;

    private float masterVolume = 1f; // ������ ���� (0.0f ~ 1.0f)
    private float backgroundMusicVolume = 1f; // ��� ���� ����
    private float effectSoundVolume = 1f; // ȿ���� ����

    private void Start()
    {
        // �ʱ� ���� ����
        SetMasterVolume(masterVolume);
        SetBackgroundMusicVolume(backgroundMusicVolume);
        SetEffectSoundVolume(effectSoundVolume);
    }


    public void SetMasterVolume(float volume)//������ ���� ����
    {

    }
    public void SetBackgroundMusicVolume(float volume)//������� ���� ����
    {

    }

    public void SetEffectSoundVolume(float volume)//ȿ���� ����
    {

    }

    public void PlayBGM(int index) //������� ���
    {

    }

    public void PauseBGM() //������� �Ͻ�����
    {

    }

    public void ResumeBGM() //������� �簳
    {

    }
    public void StopBGM() //������� ����
    {

    }
    public void PlayEfs(int index) //ȿ���� ���
    {

    }
}
