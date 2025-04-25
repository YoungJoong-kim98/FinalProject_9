using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupCaller : MonoBehaviour
{
    [Header("이 영역에 대한 팝업 대상")]
    public GameObject popupTargetObject;  // 해당 영역에 들어왔을 때 나타날 팝업 대상

    private PopEffect popupEffect;  // 팝업 효과 컴포넌트

    private void Start()
    {
        // popupTargetObject가 null이 아니면 PopEffect를 가져옴
        if (popupTargetObject != null)
        {
            popupEffect = popupTargetObject.GetComponent<PopEffect>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"플레이어가 {gameObject.name} 영역에 들어왔다.");

        // 팝업이 있을 경우 Show() 호출
        if (popupEffect != null)
        {
            popupEffect.Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 팝업이 있을 경우 Hide() 호출
        if (popupEffect != null)
        {
            popupEffect.Hide();
        }
    }
}

