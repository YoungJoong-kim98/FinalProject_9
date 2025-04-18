using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotator : MonoBehaviour
{
    public float rotationSpeed = 20f;
    private bool isDragging = false;
    private Vector3 previousMousePosition;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺�� ������ ��: �巡�� ����
            isDragging = true;
            previousMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // ���콺���� ���� ���� ��: �巡�� ����
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - previousMousePosition;
            float horizontalRotation = delta.x * rotationSpeed * Time.deltaTime;

            // Y���� �������� ȸ��
            transform.Rotate(Vector3.up, -horizontalRotation, Space.World);

            previousMousePosition = Input.mousePosition;
        }
    }
}