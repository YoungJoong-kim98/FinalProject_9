using UnityEngine;

public class GlassTrigger : MonoBehaviour
{
    [SerializeField] private GlassPlatform _glassPlatform;

    private void OnCollisionEnter(Collision collision)
    {
        _glassPlatform.Break();
    }
}
