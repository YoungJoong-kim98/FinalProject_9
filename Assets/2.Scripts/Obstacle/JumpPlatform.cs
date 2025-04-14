using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    //점프 힘
    [SerializeField] private float _jumpPower = -1f;

    private void Start()
    {
        //데이터 초기화
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _jumpPower, data.jumpPower);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //점프 메서드 실행
            AddJumpPower(collision.gameObject);
        }
    }

    //점프 메서드
    private void AddJumpPower(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Rigidbody rb))
        {
            //물리 처리
            rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }
        //rigidbody가 없을때 에러코드
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
