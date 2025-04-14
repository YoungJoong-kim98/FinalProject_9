using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    //������� ������ �ð�
    [SerializeField] private float _disappearTime = -1f;
    //�����Ǳ� ������ �ð�
    [SerializeField] private float _appearTime = -1f;

    //�÷���
    private bool _isInteracting = false;

    private void Start()
    {
        //������ �ʱ�ȭ
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _disappearTime, data.disapearTime);
        Utilitys.SetIfNegative(ref _appearTime, data.apearTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�÷��̾�� �浹 ���̰� ���� ���� �ƴҶ�
        if (collision.collider.CompareTag("Player") && !_isInteracting)
        {
            //������� ����� ���� ����
            _isInteracting = true;
            StartCoroutine(HideAndRestoreCoroutine());
        }
    }

    //������� ����� ����
    private IEnumerator HideAndRestoreCoroutine()
    {
        //�÷���
        _isInteracting = true;

        //MeshRenderer�� Collider ��Ʈ��
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        Collider collider = gameObject.GetComponent<Collider>();

        yield return new WaitForSeconds(_disappearTime);
        meshRenderer.enabled = false;
        collider.enabled = false;

        yield return new WaitForSeconds(_appearTime);
        meshRenderer.enabled = true;
        collider.enabled = true;

        //�÷���
        _isInteracting = false;
    }
}
