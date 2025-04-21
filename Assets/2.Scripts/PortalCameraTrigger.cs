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
    // private CameraController _cameraController;
    private CinemachineBrain _brain;

    private void Start()
    {
        _mainCamObj = Camera.main.gameObject;
        // _cameraController = _mainCamObj.GetComponent<CameraController>();
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
    
    private IEnumerator PlayPortalCinematic()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerStateMachine = player?.GetComponent<Player>()?.stateMachine;
        var rb = player?.GetComponent<Rigidbody>();
        
        // �Է� ������ ����
        if (playerStateMachine != null)
        {
            playerStateMachine.IsMovementLocked = true;
            playerStateMachine.MovementInput = Vector2.zero;
        }
        
        // ���� ������ ����
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
        
        // ��Ż �ó׸�ƽ ī�޶� Ȱ��ȭ
        moveCam.Priority = 5;
        blendCam.Priority = 20;
    
        // �ó׸�ƽ ���� �ð�
        yield return new WaitForSeconds(blendHoldTime);
    
        // �ٽ� ���� ī�޶�� ��ȯ
        blendCam.Priority = 5;
        moveCam.Priority = 20;
        
        // �Է� �� ���� ����
        if (rb != null) rb.isKinematic = false;
        if (playerStateMachine != null) playerStateMachine.IsMovementLocked = false;
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
    //     // ���� ī�޶� ��ũ��Ʈ ���� �ó׸ӽ� �ѱ�
    //     if (_cameraController != null) _cameraController.enabled = false;
    //     if (_brain != null) _brain.enabled = true;
    //
    //     // moveCam �켱���� ����, 1������ ���
    //     moveCam.Priority = 20;
    //     blendCam.Priority = 5;
    //     yield return null;
    //
    //     // blendCam Ȱ��ȭ
    //     blendCam.Priority = 30;
    //     moveCam.Priority = 10;
    //
    //     // �ó׸�ƽ ����
    //     yield return new WaitForSeconds(blendHoldTime);
    //
    //     // �ٽ� move�� ��ȯ
    //     blendCam.Priority = 5;
    //     moveCam.Priority = 30;
    //
    //     // ���� ī�޶�� ��ȯ
    //     yield return null;
    //     Transform virtualCamTransform = moveCam.transform;
    //     _mainCamObj.transform.position = virtualCamTransform.position;
    //     _mainCamObj.transform.rotation = virtualCamTransform.rotation;
    //
    //     // �ó׸ӽ� ���� ���� ī�޶� ��ũ��Ʈ ���ֱ�
    //     if (_brain != null) _brain.enabled = false;
    //     if (_cameraController != null) _cameraController.enabled = true;
    //     
    //     // �Է� �� ���� ����
    //     if (rb != null) rb.isKinematic = false;
    //     if (playerStateMachine != null) playerStateMachine.IsMovementLocked = false;
    // }
    
    // private IEnumerator PlayPortalCinematic()
    // {
    //     var player = GameObject.FindGameObjectWithTag("Player");
    //     var stateMachine = player?.GetComponent<Player>()?.stateMachine;
    //     var rb = player?.GetComponent<Rigidbody>();
    //
    //     // �Է� ������ ����
    //     if (stateMachine != null)
    //     {
    //         stateMachine.IsMovementLocked = true;
    //         stateMachine.MovementInput = Vector2.zero;
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
    //     // ���� ī�޶� ��ũ��Ʈ ���� �ó׸ӽ� �ѱ�
    //     if (_cameraController != null) _cameraController.enabled = false;
    //     if (_brain != null) _brain.enabled = true;
    //
    //     // ���� ���� ������ �ӽ� ī�޶� ����
    //     var tempCamGO = new GameObject("TempCam");
    //     var tempCam = tempCamGO.AddComponent<CinemachineVirtualCamera>();
    //     tempCam.Priority = 30; // �켱���� ���� ����
    //
    //     // ���� ī�޶� ��ġ, ȸ�� �״��
    //     tempCam.transform.position = _mainCamObj.transform.position;
    //     tempCam.transform.rotation = _mainCamObj.transform.rotation;
    //     
    //     // ī�޶� ���� ����
    //     tempCam.m_Lens.Dutch = 0f;
    //
    //     yield return null;
    //
    //     // blendCam���� ��ȯ
    //     blendCam.Priority = 40;
    //     tempCam.Priority = 30;
    //
    //     yield return new WaitForSeconds(blendHoldTime);
    //
    //     // moveCam���� ��ȯ
    //     moveCam.Priority = 50;
    //     blendCam.Priority = 40;
    //
    //     yield return new WaitForSeconds(2f); // ���� ���
    //     
    //     // ����ī�޶� ��ġ moveCam�� ���߱�
    //     _mainCamObj.transform.position = moveCam.transform.position;
    //     _mainCamObj.transform.rotation = moveCam.transform.rotation;
    //
    //     // �ӽ� ī�޶� ����
    //     Destroy(tempCamGO);
    //
    //     _brain.enabled = false;
    //     _cameraController.enabled = true;
    //
    //     // �Է� �� ���� ����
    //     if (rb != null) rb.isKinematic = false;
    //     if (stateMachine != null) stateMachine.IsMovementLocked = false;
    // }
}
