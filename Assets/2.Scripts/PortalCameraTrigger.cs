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
    //     // 포탈 시네마틱 카메라 활성화
    //     moveCam.Priority = 5;
    //     blendCam.Priority = 20;
    //     // 시네마틱 유지 시간
    //     yield return new WaitForSeconds(blendHoldTime);
    //
    //     // 다시 무브 카메라로 전환
    //     blendCam.Priority = 5;
    //     moveCam.Priority = 20;
    //     
    //     // 입력 및 물리 복원
    //     if (rb != null) rb.isKinematic = false;
    //     if (playerStateMachine != null) playerStateMachine.IsMovementLocked = false;
    // }
    
    // private IEnumerator PlayPortalCinematic()
    // {
    //     var player = GameObject.FindGameObjectWithTag("Player");
    //     var playerStateMachine = player?.GetComponent<Player>()?.stateMachine;
    //     var rb = player?.GetComponent<Rigidbody>();
    //     
    //     // 컷씬 직전 메인 카메라 위치 저장
    //     Vector3 originalCamPos = _mainCamObj.transform.position;
    //     Quaternion originalCamRot = _mainCamObj.transform.rotation;
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
    //     // 카메라 컨트롤러 비활성화
    //     _cameraController.enabled = false;
    //     
    //     // Cinemachine 활성화
    //     _brain.enabled = true;
    //     
    //     // 초기 Priority 설정
    //     moveCam.Priority = 10;
    //     blendCam.Priority = 5;
    //
    //     yield return null; // 1프레임 대기 (적용 안정화)
    //     
    //     // blendCam으로 전환 (시네마틱 카메라 진입)
    //     moveCam.Priority = 15;
    //     blendCam.Priority = 20;
    //
    //     yield return new WaitForSeconds(blendHoldTime); // 시네마틱 유지 시간
    //
    //     // 다시 무브 카메라로 전환
    //     moveCam.Priority = 25;
    //     blendCam.Priority = 20;
    //
    //     yield return new WaitForSeconds(1.0f);
    //     
    //     // Cinemachine 비활성화 및 카메라 위치 복구
    //     _brain.enabled = false;
    //     _mainCamObj.transform.position = originalCamPos;
    //     _mainCamObj.transform.rotation = originalCamRot;
    //     
    //     // 입력 및 물리 복구
    //     if (rb != null) rb.isKinematic = false;
    //     if (playerStateMachine != null) playerStateMachine.IsMovementLocked = false;
    //     
    //     _cameraController.enabled = true;  // CameraController 다시 활성화
    // }
    
    private IEnumerator PlayPortalCinematic()
    {
        // 1. 플레이어 및 컴포넌트
        var player = GameObject.FindGameObjectWithTag("Player");
        var stateMachine = player?.GetComponent<Player>()?.stateMachine;
        var rb = player?.GetComponent<Rigidbody>();

        // 2. 입력, 물리 비활성화
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

        // 3. 카메라 제어 비활성화
        _cameraController.enabled = false;
        _brain.enabled = true;

        // 4. 현재 시점을 복제한 VirtualCamera 생성
        var tempCamGO = new GameObject("TempCam");
        var tempCam = tempCamGO.AddComponent<CinemachineVirtualCamera>();
        tempCam.Priority = 30; // 가장 높은 우선순위

        // 현재 카메라 위치 & 회전 그대로
        tempCam.transform.position = _mainCamObj.transform.position;
        tempCam.transform.rotation = _mainCamObj.transform.rotation;

        yield return null;

        // 5. blendCam으로 블렌드
        blendCam.Priority = 40;
        tempCam.Priority = 30;

        yield return new WaitForSeconds(blendHoldTime);

        // 6. moveCam으로 다시 블렌드
        moveCam.Priority = 50;
        blendCam.Priority = 40;

        yield return new WaitForSeconds(1f); // 블렌드 대기

        // 7. 정리
        Destroy(tempCamGO);

        _brain.enabled = false;
        _cameraController.enabled = true;

        if (rb != null) rb.isKinematic = false;
        if (stateMachine != null) stateMachine.IsMovementLocked = false;
    }
}
