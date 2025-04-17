using UnityEngine;

#if UNITY_EDITOR
#endif
public class CameraController : MonoBehaviour
{
    public Transform target;        // 카메라가 따라다닐 대상
    public float minDistance = 1f;  // 카메라와 타겟 간 최소 거리
    public float maxDistance = 5f;  // 카메라와 타겟 간 최대 거리
    public float smoothSpeed = 10f; // 카메라 이동 부드럽게 만드는 속도
    public LayerMask obstacleLayers;    // 카메라와 대상 사이에 감지할 장애물 레이어

    private Vector3 _currentVelocity;    // SmoothDamp에 쓰일 속도 저장 변수

    // 마우스 회전 관련 변수들
    private float _yaw = 0f;     // 좌우 회전
    private float _pitch = 0f;   // 상하 회전

    [Header("Mouse Settings")]
    public float mouseSensitivity = 3f; // 마우스 감도
    public float pitchMin = -30f;       // 아래로 회전 가능한 최소 각도
    public float pitchMax = 80f;        // 위로 회전 가능한 최대 각도

#if UNITY_EDITOR
    private void OnEnable()
    {
        // 에디터에서만 작동 (플레이 중이 아니면 CinemachineBrain 켜주기 - 플레이어 이동 편하게 하기 위해)
        Cinemachine.CinemachineBrain cameraBrain = Camera.main?.GetComponent<Cinemachine.CinemachineBrain>();
        if (cameraBrain != null)
        {
            cameraBrain.enabled = !Application.isPlaying;
        }
    }
#else
    void OnEnable()
    {
        // 실행 중일 땐 CinemachineBrain 꺼주기
        Cinemachine.CinemachineBrain cameraBrain = Camera.main?.GetComponent<Cinemachine.CinemachineBrain>();
        if (cameraBrain != null)
        {
            cameraBrain.enabled = false;
        }
    }
#endif

    private void LateUpdate()
    {
        if (!target) return;

        // 마우스 입력 처리
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _yaw += mouseX * mouseSensitivity;
        _pitch -= mouseY * mouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax); // 상하 회전 제한

        // 회전값을 기준으로 방향 계산 (방향 벡터로 변환)
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3 direction = rotation * Vector3.back;    // 뒤쪽 방향으로 이동

        // 장애물 체크
        float targetDistance = maxDistance;
        if (Physics.SphereCast(target.position, 0.2f, direction, out RaycastHit hit, maxDistance, obstacleLayers))
        {
            targetDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);   // 장애물에 부딪히면 거리 조정
        }

        // 카메라 최종 위치 계산
        Vector3 desiredPosition = target.position + direction * targetDistance;
        
        // 카메라 부드럽게 이동
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _currentVelocity, 1f / smoothSpeed);

        // 카메라가 항상 대상을 바라보게
        transform.LookAt(target.position);
    }
}