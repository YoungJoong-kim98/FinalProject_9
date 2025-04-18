using System.Collections;
using UnityEngine;

public class ReflectObstacle : MonoBehaviour
{
    //�ݻ��ϴ� ��
    [SerializeField] private float _reflectPower = -1f;
    //�ּ� ��
    [SerializeField] private float _reflectMinPower = -1f;
    //�ִ� ��
    [SerializeField] private float _reflectMaxPower = -1f;

    private void Start()
    {
        //������ �ʱ�ȭ
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _reflectPower, data.reflectPower);
        Utilitys.SetIfNegative(ref _reflectMinPower, data.reflectMinPower);
        Utilitys.SetIfNegative(ref _reflectMaxPower, data.reflectMaxPower);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�÷��̾�� �浹��
        if (collision.gameObject.CompareTag("Player"))
        {
            //�ݻ��ϴ� �޼��� ����
            Reflect(collision.gameObject, collision.relativeVelocity.magnitude);
        }
    }

    //�ݻ��ϴ� �޼���
    private void Reflect(GameObject target, float impactSpeed)
    {
        //�ݻ��ϴ� ����(�������θ� �ݻ�)
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0f;
        direction = direction.normalized;

        //Ÿ���� ���� ����
        target.transform.forward = -direction;

        //�÷��̾��� ������ ����
        if (target.TryGetComponent(out Player player))
        {
            ObstacleManager.Instance.StartLockMovement(player);
        }

        //���� ó��
        if (target.TryGetComponent(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;

            float scaledPower = Mathf.Clamp(impactSpeed * _reflectPower, _reflectMinPower, _reflectMaxPower);
            rb.AddForce(direction * scaledPower, ForceMode.Impulse);
        }
    }
}
