using UnityEngine;

public class TestPlayerGravity : MonoBehaviour
{
    public float forceGravity = 50f;

    private void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * forceGravity);
    }
}
