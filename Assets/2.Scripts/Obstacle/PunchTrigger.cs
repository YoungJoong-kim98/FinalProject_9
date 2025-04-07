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
}
