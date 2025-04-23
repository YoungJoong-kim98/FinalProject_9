using UnityEngine;

public class FanArea : MonoBehaviour
{
    //힘의 방향
    public Vector3 forceDirection = Vector3.forward;
    //힘의 크기
    [SerializeField] private float forceStrength = -1f;

    private void Start()
    {
        //데이터 초기화
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref forceStrength, data.forceStrength);
    }

    private void OnTriggerStay(Collider other)
    {
        //선풍기 메서드 실행
        Fan(other.gameObject);
    }

    //선풍기 메서드
    private void Fan(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Rigidbody rb))
        {
            //물리 처리
            rb.AddForce(forceDirection.normalized * forceStrength, ForceMode.Force);
        }
        //rigidbody가 없을때 에러코드
        else
        {
            Debug.LogWarning($"{gameObject.name} does not have rigidbody");
        }
    }

    private void OnDrawGizmos()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            //큐브의 크기만큼 드로우
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(renderer.bounds.center, renderer.bounds.size);
        }
    }
}
