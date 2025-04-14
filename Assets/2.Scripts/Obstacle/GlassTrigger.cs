using UnityEngine;

public class GlassTrigger : MonoBehaviour
{
    //유리 발판
    [SerializeField] private GlassPlatform _glassPlatform;

    private void OnCollisionEnter(Collision collision)
    {
        //유리 발판 깨지는 메서드 실행
        _glassPlatform.Break();
    }
}
