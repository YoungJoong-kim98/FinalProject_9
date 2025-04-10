using UnityEngine;

public class ReflectObstacle : MonoBehaviour
{
    [SerializeField] private float _reflectPower = -1f;

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

        if (target.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(direction * _reflectPower, ForceMode.Impulse);
        }
        else
        {
            return;
        }
    }
}
