using System.Collections;
using UnityEngine;

public enum PlatformState
{
    None,
    Hide,
    Restore
}

public class Platform : MonoBehaviour, ISaveObstacle, IObstacle
{
    [SerializeField] private float _disappearTime = -1f;
    [SerializeField] private float _appearTime = -1f;
    public float remainTime;

    private MeshRenderer _meshRenderer;
    private Collider _collider;

    private bool _isInteracting = false;

    public PlatformState state = PlatformState.None;

    [SerializeField] private string _id;
    [SerializeField] private ObstacleDataType _type = ObstacleDataType.Platform;

    public string Id
    {
        get => _id;
        set => _id = value;
    }

    public ObstacleDataType Type => _type;

    private void Start()
    {
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _disappearTime, data.disapearTime);
        Utilitys.SetIfNegative(ref _appearTime, data.apearTime);

        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();

        AddList();
    }

    public void Init()
    {
        switch (state)
        {
            case PlatformState.None:
                break;

            case PlatformState.Hide:
                StartCoroutine(HideCoroutine(remainTime));
                break;

            case PlatformState.Restore:
                StartCoroutine(RestoreCoroutine(remainTime));
                break;

            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !_isInteracting)
        {
            _isInteracting = true;
            StartCoroutine(HideCoroutine(_disappearTime));
        }
    }

    private IEnumerator HideCoroutine(float time)
    {
        yield return RunTimer(time, PlatformState.Hide);

        _meshRenderer.enabled = false;
        _collider.enabled = false;

        StartCoroutine(RestoreCoroutine(_appearTime));
    }

    private IEnumerator RestoreCoroutine(float time)
    {
        yield return RunTimer(time, PlatformState.Restore);

        _meshRenderer.enabled = true;
        _collider.enabled = true;

        state = PlatformState.None;
    }

    private IEnumerator RunTimer(float duration, PlatformState targetState)
    {
        state = targetState;
        remainTime = duration;

        while (remainTime > 0f)
        {
            remainTime -= Time.deltaTime;
            yield return null;

            if (state != targetState)
                yield break;
        }
    }
    public void AddList()
    {
        ObstacleManager.Instance.saveObstacles.Add(this);
    }

    public void SetId(string newId)
    {
        Id = newId;
    }

    public ObstacleSaveData ToData()
    {
        ObstacleSaveData saveData = new ObstacleSaveData();
        saveData.remainTime = remainTime;
        saveData.platformState = state;
        return saveData;
    }

    public void LoadtoData(ObstacleSaveData data)
    {
        remainTime = data.remainTime;
        state = data.platformState;
    }
}
