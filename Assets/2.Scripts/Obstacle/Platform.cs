using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    //사라지기 까지의 시간
    [SerializeField] private float _disappearTime = -1f;
    //생성되기 까지의 시간
    [SerializeField] private float _appearTime = -1f;

    //플래그
    private bool _isInteracting = false;

    private void Start()
    {
        //데이터 초기화
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _disappearTime, data.disapearTime);
        Utilitys.SetIfNegative(ref _appearTime, data.apearTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //플레이어와 충돌 중이고 실행 중이 아닐때
        if (collision.collider.CompareTag("Player") && !_isInteracting)
        {
            //사라지고 생기는 로직 실행
            _isInteracting = true;
            StartCoroutine(HideAndRestoreCoroutine());
        }
    }

    //사라지고 생기는 로직
    private IEnumerator HideAndRestoreCoroutine()
    {
        //플래그
        _isInteracting = true;

        //MeshRenderer와 Collider 컨트롤
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        Collider collider = gameObject.GetComponent<Collider>();

        yield return new WaitForSeconds(_disappearTime);
        meshRenderer.enabled = false;
        collider.enabled = false;

        yield return new WaitForSeconds(_appearTime);
        meshRenderer.enabled = true;
        collider.enabled = true;

        //플래그
        _isInteracting = false;
    }
}
