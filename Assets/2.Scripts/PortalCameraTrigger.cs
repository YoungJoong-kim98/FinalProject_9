using UnityEngine;
using Cinemachine;
using System.Collections;

public class PortalCameraTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera moveCam;
    public CinemachineVirtualCamera blendCam;
    public float blendHoldTime = 8f; 

    private bool triggered = false;
    
    private GameObject _mainCamObj;
    private CameraController _cameraController;
    private CinemachineBrain _brain;

    private void Start()
    {
        _mainCamObj = Camera.main.gameObject;
        _cameraController = _mainCamObj.GetComponent<CameraController>();
        _brain = _mainCamObj.GetComponent<CinemachineBrain>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(PlayPortalCinematic());
        }
    }
    
    // private IEnumerator PlayPortalCinematic()
    // {
    //     var player = GameObject.FindGameObjectWithTag("Player");
    //     var playerStateMachine = player?.GetComponent<Player>()?.stateMachine;
    //     var rb = player?.GetComponent<Rigidbody>();
    //     
    //     // �Է� ������ ����
    //     if (playerStateMachine != null)
    //     {
    //         playerStateMachine.IsMovementLocked = true;
    //         playerStateMachine.MovementInput = Vector2.zero;
    //     }
    //     
    //     // ���� ������ ����
    //     if (rb != null)
    //     {
    //         rb.velocity = Vector3.zero;
    //         rb.angularVelocity = Vector3.zero;
    //         rb.isKinematic = true;
    //     }
    //     
    //     // ��Ż �ó׸�ƽ ī�޶� Ȱ��ȭ
    //     moveCam.Priority = 5;
    //     blendCam.Priority = 20;
    //     // �ó׸�ƽ ���� �ð�
    //     yield return new WaitForSeconds(blendHoldTime);
    //
    //     // �ٽ� ���� ī�޶�� ��ȯ
    //     blendCam.Priority = 5;
    //     moveCam.Priority = 20;
    //     
    //     // �Է� �� ���� ����
    //     if (rb != null) rb.isKinematic = false;
    //     if (playerStateMachine != null) playerStateMachine.IsMovementLocked = false;
    // }
    
    // private IEnumerator PlayPortalCinematic()
    // {
    //     var player = GameObject.FindGameObjectWithTag("Player");
    //     var playerStateMachine = player?.GetComponent<Player>()?.stateMachine;
    //     var rb = player?.GetComponent<Rigidbody>();
    //     
    //     // �ƾ� ���� ���� ī�޶� ��ġ ����
    //     Vector3 originalCamPos = _mainCamObj.transform.position;
    //     Quaternion originalCamRot = _mainCamObj.transform.rotation;
    //     
    //     // �Է� ������ ����
    //     if (playerStateMachine != null)
    //     {
    //         playerStateMachine.IsMovementLocked = true;
    //         playerStateMachine.MovementInput = Vector2.zero;
    //     }
    //     
    //     // ���� ������ ����
    //     if (rb != null)
    //     {
    //         rb.velocity = Vector3.zero;
    //         rb.angularVelocity = Vector3.zero;
    //         rb.isKinematic = true;
    //     }
    //     
    //     // ī�޶� ��Ʈ�ѷ� ��Ȱ��ȭ
    //     _cameraController.enabled = false;
    //     
    //     // Cinemachine Ȱ��ȭ
    //     _brain.enabled = true;
    //     
    //     // �ʱ� Priority ����
    //     moveCam.Priority = 10;
    //     blendCam.Priority = 5;
    //
    //     yield return null; // 1������ ��� (���� ����ȭ)
    //     
    //     // blendCam���� ��ȯ (�ó׸�ƽ ī�޶� ����)
    //     moveCam.Priority = 15;
    //     blendCam.Priority = 20;
    //
    //     yield return new WaitForSeconds(blendHoldTime); // �ó׸�ƽ ���� �ð�
    //
    //     // �ٽ� ���� ī�޶�� ��ȯ
    //     moveCam.Priority = 25;
    //     blendCam.Priority = 20;
    //
    //     yield return new WaitForSeconds(1.0f);
    //     
    //     // Cinemachine ��Ȱ��ȭ �� ī�޶� ��ġ ����
    //     _brain.enabled = false;
    //     _mainCamObj.transform.position = originalCamPos;
    //     _mainCamObj.transform.rotation = originalCamRot;
    //     
    //     // �Է� �� ���� ����
    //     if (rb != null) rb.isKinematic = false;
    //     if (playerStateMachine != null) playerStateMachine.IsMovementLocked = false;
    //     
    //     _cameraController.enabled = true;  // CameraController �ٽ� Ȱ��ȭ
    // }
    
    private IEnumerator PlayPortalCinematic()
    {
        // 1. �÷��̾� �� ������Ʈ
        var player = GameObject.FindGameObjectWithTag("Player");
        var stateMachine = player?.GetComponent<Player>()?.stateMachine;
        var rb = player?.GetComponent<Rigidbody>();

        // 2. �Է�, ���� ��Ȱ��ȭ
        if (stateMachine != null)
        {
            stateMachine.IsMovementLocked = true;
            stateMachine.MovementInput = Vector2.zero;
        }

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // 3. ī�޶� ���� ��Ȱ��ȭ
        _cameraController.enabled = false;
        _brain.enabled = true;

        // 4. ���� ������ ������ VirtualCamera ����
        var tempCamGO = new GameObject("TempCam");
        var tempCam = tempCamGO.AddComponent<CinemachineVirtualCamera>();
        tempCam.Priority = 30; // ���� ���� �켱����

        // ���� ī�޶� ��ġ & ȸ�� �״��
        tempCam.transform.position = _mainCamObj.transform.position;
        tempCam.transform.rotation = _mainCamObj.transform.rotation;

        yield return null;

        // 5. blendCam���� ����
        blendCam.Priority = 40;
        tempCam.Priority = 30;

        yield return new WaitForSeconds(blendHoldTime);

        // 6. moveCam���� �ٽ� ����
        moveCam.Priority = 50;
        blendCam.Priority = 40;

        yield return new WaitForSeconds(1f); // ���� ���

        // 7. ����
        Destroy(tempCamGO);

        _brain.enabled = false;
        _cameraController.enabled = true;

        if (rb != null) rb.isKinematic = false;
        if (stateMachine != null) stateMachine.IsMovementLocked = false;
    }
}
