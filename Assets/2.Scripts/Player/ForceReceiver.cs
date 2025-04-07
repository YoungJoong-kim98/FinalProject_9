using System;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller; //���� �پ��ִ��� �ƴ��� Ȯ���ϴ� ������ ��������.

    private float verticalVelocity;

    public Vector3 Movement => Vector3.up * verticalVelocity;
    private float gravityScale = 1f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * gravityScale * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;
        }
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }

    public void SetGravityScale(float scale)
    {
        gravityScale = scale;
    }
}