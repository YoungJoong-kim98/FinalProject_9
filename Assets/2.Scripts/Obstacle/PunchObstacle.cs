using System.Collections;
using UnityEngine;

public class PunchObstacle : MonoBehaviour
{
    [SerializeField] private float _pushPower = 15f;
    [SerializeField] private float _pushSpeed = 15f;
    [SerializeField] private float _backSpeed = 5f;
    [SerializeField] private float _moveDistance = 5f;
    [SerializeField] private Vector3 _direction = Vector3.forward;
    
    private Vector3 _startPos;
    private Vector3 _targetPos;
    private bool _isPunching = false;

    //testcode
    //private void Start()
    //{
    //    Punch();
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && _isPunching)
        {
            Push(collision.gameObject);
        }
    }

    private void Push(GameObject go)
    {
        Rigidbody rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(_direction.normalized * _pushPower, ForceMode.Impulse);
        }
    }

    public void Punch()
    {
        if (!_isPunching)
        {
            StartCoroutine(PunchMove());
        }
    }

    private IEnumerator PunchMove()
    {
        _isPunching = true;

        _startPos = transform.position;
        _targetPos = transform.position + _direction.normalized * _moveDistance;

        while (Vector3.Distance(transform.position, _targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, _pushSpeed * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(transform.position, _startPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPos, _backSpeed * Time.deltaTime);
            yield return null;
        }

        _isPunching = false;
        yield return null;
    }
}
