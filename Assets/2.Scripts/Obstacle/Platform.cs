using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private float _disappearTime = -1f;
    [SerializeField] private float _appearTime = -1f;

    [SerializeField] private bool _isInteracting = false;

    private void Start()
    {
        var data = ObstacleManager.Instance.obstacleData;
        if (_disappearTime < 0f)
        {
            _disappearTime = data.disapearTime;
        }
        if(_appearTime < 0f)
        {
            _appearTime = data.apearTime;
        }

        //testcode
        ObstacleManager.Instance.HideAndRestore(gameObject, _disappearTime, _appearTime);
        Invoke("ResetInteraction", _disappearTime + _appearTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !_isInteracting)
        {
            _isInteracting = true;
            ObstacleManager.Instance.HideAndRestore(gameObject,_disappearTime, _appearTime);

            Invoke("ResetInteraction", _disappearTime + _appearTime);
        }
    }

    private void ResetInteraction()
    {
        _isInteracting = false;
    }
}
