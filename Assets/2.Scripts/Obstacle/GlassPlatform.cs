using UnityEngine;

public class GlassPlatform : MonoBehaviour
{
    //정상적인 유리 오브젝트
    [SerializeField] private GameObject _glassObject;
    //깨진 유리 오브젝트
    [SerializeField] private GameObject _shatteredObject;

    //깨지는 메서드
    public void Break()
    {
        //깨진 유리 활성화
        _shatteredObject.SetActive(true);

        //깨진 유리 조각들에게 실행
        foreach (Rigidbody rb in _shatteredObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(300f, transform.position, 2f);
        }
        //원래 유리 제거
        Destroy(_glassObject.gameObject);
        //5초뒤에 제거
        Invoke("OnDestroy", 5f);
    }

    private void OnDestroy()
    {
        //게임 오브젝트 제거
        Destroy(gameObject);
    }
}
