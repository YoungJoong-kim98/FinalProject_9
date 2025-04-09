using UnityEngine;

public class PunchTrigger : MonoBehaviour
{
    [SerializeField] private PunchObstacle _punchObstacle;

    private void OnTriggerEnter(Collider other)
    {
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
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(renderer.bounds.center, renderer.bounds.size);
        }
    }
}
