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
                Debug.Log($"[AssignRenderCamera] '{gameObject.name}'의 Canvas에 Main Camera 할당됨.");
            }
            else
            {
                Debug.LogWarning("[AssignRenderCamera] Main Camera를 찾을 수 없음");
            }
        }
    }
}