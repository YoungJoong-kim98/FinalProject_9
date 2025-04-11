using System.Collections;
using UnityEngine;

public class ReflectObstacle : MonoBehaviour
{
    [SerializeField] private float _reflectPower = -1f;
    private GameObject target;

    private void Start()
    {
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _reflectPower, data.reflectPower);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Reflect(collision.gameObject);
        }
    }

    private void Reflect(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0f;
        direction = direction.normalized;

        target.transform.forward = -direction;

        if (target.TryGetComponent(out Player player))
        {
            ObstacleManager.Instance.StartLockMovement(player);

            if (target.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(direction * _reflectPower, ForceMode.Impulse);
            }
        }
    }
}
