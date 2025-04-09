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
        // �����ϱ� ���� Y�ӵ� �ʱ�ȭ (���� �� ������ ���� ����)
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
