using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private float _disappearTime = 1f;
    [SerializeField] private float _appearTime = 5f;

    [SerializeField] private bool _isInteracting = false;

    public PlatformManager platformManager;//testcode

    private void Awake()
    {
        //testcode
        platformManager.HideAndRestore(gameObject, _disappearTime, _appearTime);
        Invoke("ResetInteraction", _disappearTime + _appearTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !_isInteracting)
        {
            _isInteracting = true;
            platformManager.HideAndRestore(gameObject,_disappearTime, _appearTime);

            Invoke("ResetInteraction", _disappearTime + _appearTime);
        }
    }

    private void ResetInteraction()
    {
        _isInteracting = false;
    }
}
