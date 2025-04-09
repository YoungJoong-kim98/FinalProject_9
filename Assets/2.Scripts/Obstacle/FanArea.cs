using UnityEditor.Playables;
using UnityEngine;

public class FanArea : MonoBehaviour
{
    public Vector3 forceDirection = Vector3.forward;
    [SerializeField] private float forceStrength = -1f;

    private void Start()
    {
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref forceStrength, data.forceStrength);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(forceDirection.normalized * forceStrength, ForceMode.Force);
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
