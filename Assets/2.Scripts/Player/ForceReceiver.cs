using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForceReceiver : MonoBehaviour
{
    private Rigidbody rb;

    private float gravityScale = 1f;
    private float defaultGravity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        defaultGravity = Physics.gravity.y;
    }

    public void Jump(float jumpForce)
    {
        // 점프하기 전에 Y속도 초기화 (낙하 중 점프시 반응 방지)
        Vector3 velocity = rb.velocity;
        velocity.y = 0;
        rb.velocity = velocity;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void SetGravityScale(float scale)
    {
        gravityScale = scale;
        Physics.gravity = new Vector3(0, defaultGravity * gravityScale, 0);
    }
}
