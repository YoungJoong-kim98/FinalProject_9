using UnityEngine;

public class FanArea : MonoBehaviour
{
    //���� ����
    public Vector3 forceDirection = Vector3.forward;
    //���� ũ��
    [SerializeField] private float forceStrength = -1f;

    private void Start()
    {
        //������ �ʱ�ȭ
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref forceStrength, data.forceStrength);
    }

    private void OnTriggerStay(Collider other)
    {
        //��ǳ�� �޼��� ����
        Fan(other.gameObject);
    }

    //��ǳ�� �޼���
    private void Fan(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Rigidbody rb))
        {
            //���� ó��
            rb.AddForce(forceDirection.normalized * forceStrength, ForceMode.Force);
        }
        //rigidbody�� ������ �����ڵ�
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
            //ť���� ũ�⸸ŭ ��ο�
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(renderer.bounds.center, renderer.bounds.size);
        }
    }
}
