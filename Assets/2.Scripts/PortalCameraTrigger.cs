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
        
        // 입력 움직임 막기
        if (playerStateMachine != null)
        {
            playerStateMachine.IsMovementLocked = true;
            playerStateMachine.MovementInput = Vector2.zero;
        }
        
        // 물리 움직임 막기
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
        
        // 포탈 시네마틱 카메라 활성화
        moveCam.Priority = 5;
        blendCam.Priority = 20;
    
        // 시네마틱 유지 시간
        yield return new WaitForSeconds(blendHoldTime);
    
        // 다시 무브 카메라로 전환
        blendCam.Priority = 5;
        moveCam.Priority = 20;
        
        // 입력 및 물리 복원
        if (rb != null) rb.isKinematic = false;
        if (playerStateMachine != null) playerStateMachine.IsMovementLocked = false;
    }
    
    // private IEnumerator PlayPortalCinematic()
    // {
    //     var player = GameObject.FindGameObjectWithTag("Player");
    //     var playerStateMachine = player?.GetComponent<Player>()?.stateMachine;
    //     var rb = player?.GetComponent<Rigidbody>();
    //     
    //     // 입력 움직임 막기
    //     if (playerStateMachine != null)
    //     {
    //         playerStateMachine.IsMovementLocked = true;
    //         playerStateMachine.MovementInput = Vector2.zero;
    //     }
    //     
    //     // 물리 움직임 막기
    //     if (rb != null)
    //     {
    //         rb.velocity = Vector3.zero;
    //         rb.angularVelocity = Vector3.zero;
    //         rb.isKinematic = true;
    //     }
    //     
    //     // 메인 카메라 스크립트 끄고 시네머신 켜기
    //     if (_cameraController != null) _cameraController.enabled = false;
    //     if (_brain != null) _brain.enabled = true;
    //
    //     // moveCam 우선순위 설정, 1프레임 대기
    //     moveCam.Priority = 20;
    //     blendCam.Priority = 5;
    //     yield return null;
    //
    //     // blendCam 활성화
    //     blendCam.Priority = 30;
    //     moveCam.Priority = 10;
    //
    //     // 시네마틱 유지
    //     yield return new WaitForSeconds(blendHoldTime);
    //
    //     // 다시 move로 전환
    //     blendCam.Priority = 5;
    //     moveCam.Priority = 30;
    //
    //     // 메인 카메라로 전환
    //     yield return null;
    //     Transform virtualCamTransform = moveCam.transform;
    //     _mainCamObj.transform.position = virtualCamTransform.position;
    //     _mainCamObj.transform.rotation = virtualCamTransform.rotation;
    //
    //     // 시네머신 끄고 메인 카메라 스크립트 켜주기
    //     if (_brain != null) _brain.enabled = false;
    //     if (_cameraController != null) _cameraController.enabled = true;
    //     
    //     // 입력 및 물리 복구
    //     if (rb != null) rb.isKinematic = false;
    //     if (playerStateMachine != null) playerStateMachine.IsMovementLocked = false;
    // }
    
    // private IEnumerator PlayPortalCinematic()
    // {
    //     var player = GameObject.FindGameObjectWithTag("Player");
    //     var stateMachine = player?.GetComponent<Player>()?.stateMachine;
    //     var rb = player?.GetComponent<Rigidbody>();
    //
    //     // 입력 움직임 막기
    //     if (stateMachine != null)
    //     {
    //         stateMachine.IsMovementLocked = true;
    //         stateMachine.MovementInput = Vector2.zero;
    //     }
    //
    //     // 물리 움직임 막기
    //     if (rb != null)
    //     {
    //         rb.velocity = Vector3.zero;
    //         rb.angularVelocity = Vector3.zero;
    //         rb.isKinematic = true;
    //     }
    //
    //     // 메인 카메라 스크립트 끄고 시네머신 켜기
    //     if (_cameraController != null) _cameraController.enabled = false;
    //     if (_brain != null) _brain.enabled = true;
    //
    //     // 현재 시점 복제한 임시 카메라 생성
    //     var tempCamGO = new GameObject("TempCam");
    //     var tempCam = tempCamGO.AddComponent<CinemachineVirtualCamera>();
    //     tempCam.Priority = 30; // 우선순위 가장 높게
    //
    //     // 현재 카메라 위치, 회전 그대로
    //     tempCam.transform.position = _mainCamObj.transform.position;
    //     tempCam.transform.rotation = _mainCamObj.transform.rotation;
    //     
    //     // 카메라 기울기 제거
    //     tempCam.m_Lens.Dutch = 0f;
    //
    //     yield return null;
    //
    //     // blendCam으로 전환
    //     blendCam.Priority = 40;
    //     tempCam.Priority = 30;
    //
    //     yield return new WaitForSeconds(blendHoldTime);
    //
    //     // moveCam으로 전환
    //     moveCam.Priority = 50;
    //     blendCam.Priority = 40;
    //
    //     yield return new WaitForSeconds(2f); // 블렌드 대기
    //     
    //     // 메인카메라 위치 moveCam에 맞추기
    //     _mainCamObj.transform.position = moveCam.transform.position;
    //     _mainCamObj.transform.rotation = moveCam.transform.rotation;
    //
    //     // 임시 카메라 제거
    //     Destroy(tempCamGO);
    //
    //     _brain.enabled = false;
    //     _cameraController.enabled = true;
    //
    //     // 입력 및 물리 복구
    //     if (rb != null) rb.isKinematic = false;
    //     if (stateMachine != null) stateMachine.IsMovementLocked = false;
    // }
}
