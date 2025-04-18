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
            // 마우스를 눌렀을 때: 드래그 시작
            isDragging = true;
            previousMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // 마우스에서 손을 뗐을 때: 드래그 종료
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - previousMousePosition;
            float horizontalRotation = delta.x * rotationSpeed * Time.deltaTime;

            // Y축을 기준으로 회전
            transform.Rotate(Vector3.up, -horizontalRotation, Space.World);

            previousMousePosition = Input.mousePosition;
        }
    }
}