using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField] private float _jumpPower = 10;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            AddJumpPower(collision.gameObject);
        }
    }

    private void AddJumpPower(GameObject gameObject)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
    }
}
