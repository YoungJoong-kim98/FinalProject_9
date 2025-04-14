using UnityEngine;

public class PunchTrigger : MonoBehaviour
{
    //펀치 장애물
    [SerializeField] private PunchObstacle _punchObstacle;

    private void OnTriggerStay(Collider other)
    {
        //플레이어가 충돌시
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
            //범위만큼 큐브 드로우 
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(renderer.bounds.center, renderer.bounds.size);
        }
    }
}
