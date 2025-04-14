using System.Collections;
using UnityEngine;

public class PunchObstacle : MonoBehaviour
{
    [SerializeField] private float _pushPower = -1f;
    [SerializeField] private float _pushSpeed = -1f;
    [SerializeField] private float _backSpeed = -1f;
    [SerializeField] private float _moveDistance = -1f;
    [SerializeField] private Vector3 _direction = Vector3.forward;
     
    [SerializeField] private bool _isReglar = false;
    [SerializeField] private float _regularTime = 1f;

    private Vector3 _startPos;
    private Vector3 _targetPos;
    private bool _isPunching = false;

    private Coroutine _punchCoroutine;    

    private void Start()
    {
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _pushPower, data.pushPower);
        Utilitys.SetIfNegative(ref _pushSpeed, data.pushSpeed);
        Utilitys.SetIfNegative(ref _backSpeed, data.backSpeed);
        Utilitys.SetIfNegative(ref _moveDistance, data.moveDistance);

        _startPos = transform.position;
        _targetPos = transform.position + _direction.normalized * _moveDistance;

        if (_isReglar)
        {
            Punch();
        }
    }

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
        Player player = go.GetComponent<Player>();
        if (player != null)
        {
            ObstacleManager.Instance.StartLockMovement(player);
        }
    }

    public void Punch()
    {
        if (_punchCoroutine == null)
        {
            _punchCoroutine = StartCoroutine(PunchMove());
        }
    }

    private IEnumerator PunchMove()
    {
        do
        {
            _isPunching = true;

            while (Vector3.Distance(transform.position, _targetPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPos, _pushSpeed * Time.deltaTime);
                yield return null;
            }

            _isPunching = false;

            while (Vector3.Distance(transform.position, _startPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _startPos, _backSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(_regularTime);
        } while (_isReglar);

        _punchCoroutine = null;
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position, transform.position + _direction * 2f);
    }
}
