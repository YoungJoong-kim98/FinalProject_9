using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageSelector : MonoBehaviour
{
    public void SetKorean()
    {
        SetLocale("ko-KR"); // 한국어
    }

    public void SetEnglish()
    {
        SetLocale("en-US"); // 영어
    }

    private void SetLocale(string code)
    {
        // 설치된 로케일 리스트에서 코드에 맞는 로케일 찾아 설정
        var locales = LocalizationSettings.AvailableLocales.Locales;
        foreach (var locale in locales)
        {
            if (locale.Identifier.Code == code)
            {
                LocalizationSettings.SelectedLocale = locale;
                Debug.Log("언어 변경: " + code);
                return;
            }
        }

        Debug.LogWarning("해당 코드에 맞는 언어를 찾을 수 없음: " + code);
    }
}
