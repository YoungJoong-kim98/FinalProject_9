using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class AssignRenderCamera : MonoBehaviour
{
    void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();

        if (canvas.worldCamera == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                canvas.worldCamera = mainCam;
                Debug.Log($"[AssignRenderCamera] '{gameObject.name}'�� Canvas�� Main Camera �Ҵ��.");
            }
            else
            {
                Debug.LogWarning("[AssignRenderCamera] Main Camera�� ã�� �� ����");
            }
        }
    }
}