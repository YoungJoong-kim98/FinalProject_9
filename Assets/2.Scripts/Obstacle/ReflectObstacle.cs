using System.Collections;
using UnityEngine;

public class ReflectObstacle : MonoBehaviour
{
    //반사하는 힘
    [SerializeField] private float _reflectPower = -1f;
    //최소 힘
    [SerializeField] private float _reflectMinPower = -1f;
    //최대 힘
    [SerializeField] private float _reflectMaxPower = -1f;

    private void Start()
    {
        //데이터 초기화
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _reflectPower, data.reflectPower);
        Utilitys.SetIfNegative(ref _reflectMinPower, data.reflectMinPower);
        Utilitys.SetIfNegative(ref _reflectMaxPower, data.reflectMaxPower);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //플레이어와 충돌시
        if (collision.gameObject.CompareTag("Player"))
        {
            //반사하는 메서드 실행
            Reflect(collision.gameObject, collision.relativeVelocity.magnitude);
        }
    }

    //반사하는 메서드
    private void Reflect(GameObject target, float impactSpeed)
    {
        //반사하는 방향(수평으로만 반사)
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0f;
        direction = direction.normalized;

        //타겟의 정면 변경
        target.transform.forward = -direction;

        //플레이어의 움직임 제한
        if (target.TryGetComponent(out Player player))
        {
            ObstacleManager.Instance.StartLockMovement(player);
        }

        //물리 처리
        if (target.TryGetComponent(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;

            float scaledPower = Mathf.Clamp(impactSpeed * _reflectPower, _reflectMinPower, _reflectMaxPower);
            rb.AddForce(direction * scaledPower, ForceMode.Impulse);
        }
    }
}
