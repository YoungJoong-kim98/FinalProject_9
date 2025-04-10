using UnityEngine;

public class GlassPlatform : MonoBehaviour
{
    [SerializeField] private GameObject _glassObject;
    [SerializeField] private GameObject _shatteredObject;

    public void Break()
    {
        _shatteredObject.SetActive(true);

        foreach (Rigidbody rb in _shatteredObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(300f, transform.position, 2f);
        }

        Destroy(_glassObject.gameObject); // 원래 유리 제거

        Invoke("OnDestroy", 5f);
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
