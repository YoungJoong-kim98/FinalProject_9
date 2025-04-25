using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 필드: 배경음악(BGM)과 효과음(SFX)의 리스트
    [SerializeField] List<AudioClip> bgms = new List<AudioClip>();
    [SerializeField] List<AudioClip> sfxs = new List<AudioClip>();

    // 효과음을 이름으로 찾을 수 있도록 딕셔너리 사용
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    // 배경음악 재생용 오디오 소스
    [SerializeField] AudioSource audioBGM;

    // 효과음 재생용 오디오 소스를 여러 개 만들어서 동시에 재생 가능하게 함
    [Header("효과음 오디오 소스 개수")]
    [SerializeField] int sfxSourceCount = 5; // 임시로 5개
    private List<AudioSource> sfxSources = new List<AudioSource>();

    private float masterVolume = 1f; // 마스터 볼륨 (0.0f ~ 1.0f)
    private float backgroundMusicVolume = 1f; // 배경 음악 볼륨
    private float effectSoundVolume = 1f; // 효과음 볼륨

    //읽기 전용 프로퍼티
    public float MasterVolume => masterVolume;
    public float BackgroundMusicVolume => backgroundMusicVolume;
    public float EffectSoundVolume => effectSoundVolume;

    private void Awake()//초기화
    {
        InitializeSFXDictionary(); // 효과음을 이름으로 관리하기 위해 초기화
        InitializeSFXSources();    // 효과음 재생용 오디오 소스 여러 개 생성
    }
    private void Start()//볼륨 반영
    {
        SetMasterVolume(masterVolume);
        SetBackgroundMusicVolume(backgroundMusicVolume);
        SetEffectSoundVolume(effectSoundVolume);
    }
    private void InitializeSFXDictionary()
    {
        // 효과음을 이름으로 관리할 수 있게 설정
        foreach (var sfx in sfxs)
        {
            if (sfx != null && !sfxDictionary.ContainsKey(sfx.name))
            {
                sfxDictionary.Add(sfx.name, sfx);
            }
        }
    }
    private void InitializeSFXSources() // 효과음 재생용 오디오 소스
    {
        for (int i = 0; i < sfxSourceCount; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(source);
        }
    }

    public void SetMasterVolume(float volume)//마스터 볼륨 설정
    {
        masterVolume = Mathf.Clamp01(volume); // 0~1로 제한
        if (audioBGM != null)
            audioBGM.volume = backgroundMusicVolume * masterVolume;
        else
            Debug.LogWarning("audioBGM이 할당되지 않았습니다.");

        foreach (var src in sfxSources)
        {
            src.volume = effectSoundVolume * masterVolume;
        }
    }
    public void SetBackgroundMusicVolume(float volume)//배경음악 볼륨 설정
    {
        backgroundMusicVolume = Mathf.Clamp01(volume);
        if (audioBGM != null)
            audioBGM.volume = backgroundMusicVolume * masterVolume;
        else
            Debug.LogWarning("audioBGM이 할당되지 않았습니다.");
    }

    public void SetEffectSoundVolume(float volume)//효과음 설정
    {
        effectSoundVolume = Mathf.Clamp01(volume);

        foreach (var src in sfxSources)
        {
            src.volume = effectSoundVolume * masterVolume;
        }
    }

    public void PlayBGM(int index) //배경음악 재생
    {
        if (bgms == null || bgms.Count == 0)
        {
            Debug.LogWarning("배경음악 리스트가 비어 있습니다. BGM을 등록해주세요.");
            return;
        }
        if (index >= 0 && index < bgms.Count)
        {
            audioBGM.clip = bgms[index];
            audioBGM.loop = true;
            audioBGM.Play();
        }
    }

    public void PauseBGM() //배경음악 일시정지
    {
        if (audioBGM.isPlaying)
        {
            audioBGM.Pause();
        }
    }

    public void ResumeBGM() //배경음악 재개
    {
        if (!audioBGM.isPlaying && audioBGM.clip != null)
        {
            audioBGM.UnPause();
        }
    }
    public void StopBGM() //배경음악 멈춤
    {
        audioBGM.Stop();
    }
    public void PlaySFX(string sfxName) //효과음 재생
    {
        if (sfxDictionary.TryGetValue(sfxName, out AudioClip clip))
        {
            // 사용 가능한 오디오 소스를 찾아 효과음 재생
            AudioSource available = sfxSources.Find(src => !src.isPlaying);
            if (available != null)
            {
                available.PlayOneShot(clip, effectSoundVolume * masterVolume);
            }
            else
            {
                Debug.LogWarning("사용 가능한 효과음 오디오 소스가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"효과음 '{sfxName}' 를 찾을 수 없습니다.");
        }
    }
}
