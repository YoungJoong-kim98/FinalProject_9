using UnityEngine;

public enum GlassPlatformState
{
    None,
    Break
}

public class GlassPlatform : MonoBehaviour
{
    //정상적인 유리 오브젝트
    [SerializeField] private GameObject _glassObject;
    //깨진 유리 오브젝트
    [SerializeField] private GameObject _shatteredObject;

    GlassPlatformState state = GlassPlatformState.None;

    private void Start()
    {
        switch (state)
        {
            case GlassPlatformState.Break:
                OnDestroy();
                break;
            default:
                break;
        }
    }

    //깨지는 메서드
    public void Break()
    {
        state = GlassPlatformState.Break;
        //깨진 유리 활성화
        _shatteredObject.SetActive(true);

        //깨진 유리 조각들에게 실행
        foreach (Rigidbody rb in _shatteredObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(300f, transform.position, 2f);
        }
        //원래 유리 비활성화
        _glassObject.SetActive(false);

        //5초뒤에 비활성화
        Invoke("OnDestroy", 5f);
    }

    private void OnDestroy()
    {
        //게임 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}
