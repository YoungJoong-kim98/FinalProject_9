using UnityEngine;

public class GlassPlatform : MonoBehaviour
{
    //�������� ���� ������Ʈ
    [SerializeField] private GameObject _glassObject;
    //���� ���� ������Ʈ
    [SerializeField] private GameObject _shatteredObject;

    //������ �޼���
    public void Break()
    {
        //���� ���� Ȱ��ȭ
        _shatteredObject.SetActive(true);

        //���� ���� �����鿡�� ����
        foreach (Rigidbody rb in _shatteredObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(300f, transform.position, 2f);
        }
        //���� ���� ����
        Destroy(_glassObject.gameObject);
        //5�ʵڿ� ����
        Invoke("OnDestroy", 5f);
    }

    private void OnDestroy()
    {
        //���� ������Ʈ ����
        Destroy(gameObject);
    }
}
