using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotator : MonoBehaviour
{
    public float rotationSpeed = 20f;
    public GameObject parentObject; // RectTransform�� �ִ� UI ����
    private RectTransform parentRectTransform;

    private bool isDragging = false;
    private Vector3 previousMousePosition;

    void Start()
    {
        if (parentObject != null)
        {
            parentRectTransform = parentObject.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("parentObject�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    void Update()
    {
        if (parentRectTransform == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOverRectTransform())
            {
                isDragging = true;
                previousMousePosition = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - previousMousePosition;
            float horizontalRotation = delta.x * rotationSpeed * Time.deltaTime;

            // ��� �ڽ� ������Ʈ ȸ��
            foreach (Transform child in transform)
            {
                child.Rotate(Vector3.up, -horizontalRotation, Space.Self);
            }

            previousMousePosition = Input.mousePosition;
        }
    }

    bool IsMouseOverRectTransform()
    {
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRectTransform,
            Input.mousePosition,
            Camera.main,
            out localMousePosition
        );

        return parentRectTransform.rect.Contains(localMousePosition);
    }
}
