using UnityEngine;
using Cinemachine;
using System.Collections;

public class PortalCameraTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera moveCam;
    public CinemachineVirtualCamera blendCam;
    public float blendHoldTime = 8f; 

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(PlayPortalCinematic());
        }
    }

    private IEnumerator PlayPortalCinematic()
    {
        // 포탈 시네마틱 카메라 활성화
        moveCam.Priority = 5;
        blendCam.Priority = 20;
        // 시네마틱 유지 시간
        yield return new WaitForSeconds(blendHoldTime);

        // 다시 무브 카메라로 전환
        blendCam.Priority = 5;
        moveCam.Priority = 20;
    }
}
