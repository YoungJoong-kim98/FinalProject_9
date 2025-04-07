using System.Collections;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public void HideAndRestore(GameObject platform, float disappearTime, float appearTime)
    {
        StartCoroutine(HideAndRestoreCoroutine(platform, disappearTime, appearTime));
    }

    private IEnumerator HideAndRestoreCoroutine(GameObject platform, float disappearTime, float appearTime)
    {
        yield return new WaitForSeconds(disappearTime);
        platform.SetActive(false);

        yield return new WaitForSeconds(appearTime);
        platform.SetActive(true);
    }
}
