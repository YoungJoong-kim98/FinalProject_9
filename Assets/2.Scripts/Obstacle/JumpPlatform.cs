using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField] private float _jumpPower = -1f;

    private void Start()
    {
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _jumpPower, data.jumpPower);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            AddJumpPower(collision.gameObject);
        }
    }

    private void AddJumpPower(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} does not have rigidbody");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position, transform.position + Vector3.up);
    }
}
