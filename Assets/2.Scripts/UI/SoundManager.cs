using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // �ʵ�: �������(BGM)�� ȿ����(SFX)�� ����Ʈ
    [SerializeField] List<AudioClip> bgms = new List<AudioClip>();
    [SerializeField] List<AudioClip> sfxs = new List<AudioClip>();

    // ȿ������ �̸����� ã�� �� �ֵ��� ��ųʸ� ���
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    // ������� ����� ����� �ҽ�
    [SerializeField] AudioSource audioBGM;

    // ȿ���� ����� ����� �ҽ��� ���� �� ���� ���ÿ� ��� �����ϰ� ��
    [Header("ȿ���� ����� �ҽ� ����")]
    [SerializeField] int sfxSourceCount = 5; // �ӽ÷� 5��
    private List<AudioSource> sfxSources = new List<AudioSource>();

    private float masterVolume = 1f; // ������ ���� (0.0f ~ 1.0f)
    private float backgroundMusicVolume = 1f; // ��� ���� ����
    private float effectSoundVolume = 1f; // ȿ���� ����

    private void Awake()//�ʱ�ȭ
    {
        InitializeSFXDictionary(); // ȿ������ �̸����� �����ϱ� ���� �ʱ�ȭ
        InitializeSFXSources();    // ȿ���� ����� ����� �ҽ� ���� �� ����
    }
    private void Start()//���� �ݿ�
    {
        SetMasterVolume(masterVolume);
        SetBackgroundMusicVolume(backgroundMusicVolume);
        SetEffectSoundVolume(effectSoundVolume);
    }
    private void InitializeSFXDictionary()
    {
        // ȿ������ �̸����� ������ �� �ְ� ����
        foreach (var sfx in sfxs)
        {
            if (sfx != null && !sfxDictionary.ContainsKey(sfx.name))
            {
                sfxDictionary.Add(sfx.name, sfx);
            }
        }
    }
    private void InitializeSFXSources() // ȿ���� ����� ����� �ҽ�
    {
        for (int i = 0; i < sfxSourceCount; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(source);
        }
    }

    public void SetMasterVolume(float volume)//������ ���� ����
    {
        masterVolume = Mathf.Clamp01(volume); // 0~1�� ����
        audioBGM.volume = backgroundMusicVolume * masterVolume;

        foreach (var src in sfxSources)
        {
            src.volume = effectSoundVolume * masterVolume;
        }
    }
    public void SetBackgroundMusicVolume(float volume)//������� ���� ����
    {
        backgroundMusicVolume = Mathf.Clamp01(volume);
        audioBGM.volume = backgroundMusicVolume * masterVolume;
    }

    public void SetEffectSoundVolume(float volume)//ȿ���� ����
    {
        effectSoundVolume = Mathf.Clamp01(volume);

        foreach (var src in sfxSources)
        {
            src.volume = effectSoundVolume * masterVolume;
        }
    }

    public void PlayBGM(int index) //������� ���
    {
        if (index >= 0 && index < bgms.Count)
        {
            audioBGM.clip = bgms[index];
            audioBGM.loop = true;
            audioBGM.Play();
        }
    }

    public void PauseBGM() //������� �Ͻ�����
    {
        if (audioBGM.isPlaying)
        {
            audioBGM.Pause();
        }
    }

    public void ResumeBGM() //������� �簳
    {
        if (!audioBGM.isPlaying && audioBGM.clip != null)
        {
            audioBGM.UnPause();
        }
    }
    public void StopBGM() //������� ����
    {
        audioBGM.Stop();
    }
    public void PlaySFX(string sfxName) //ȿ���� ���
    {
        if (sfxDictionary.TryGetValue(sfxName, out AudioClip clip))
        {
            // ��� ������ ����� �ҽ��� ã�� ȿ���� ���
            AudioSource available = sfxSources.Find(src => !src.isPlaying);
            if (available != null)
            {
                available.PlayOneShot(clip, effectSoundVolume * masterVolume);
            }
            else
            {
                Debug.LogWarning("��� ������ ȿ���� ����� �ҽ��� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning($"ȿ���� '{sfxName}' �� ã�� �� �����ϴ�.");
        }
    }
}
