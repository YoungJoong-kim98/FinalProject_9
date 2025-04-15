using UnityEngine;

public class PunchTrigger : MonoBehaviour
{
    //��ġ ��ֹ�
    [SerializeField] private PunchObstacle _punchObstacle;

    private void OnTriggerStay(Collider other)
    {
        //�÷��̾ �浹��
        if (other.gameObject.CompareTag("Player"))
        {
            _punchObstacle.Punch();
        }
    }
    private void OnDrawGizmos()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            //������ŭ ť�� ��ο� 
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(renderer.bounds.center, renderer.bounds.size);
        }
    }
}
