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
        // ��Ż �ó׸�ƽ ī�޶� Ȱ��ȭ
        moveCam.Priority = 5;
        blendCam.Priority = 20;
        // �ó׸�ƽ ���� �ð�
        yield return new WaitForSeconds(blendHoldTime);

        // �ٽ� ���� ī�޶�� ��ȯ
        blendCam.Priority = 5;
        moveCam.Priority = 20;
    }
}
