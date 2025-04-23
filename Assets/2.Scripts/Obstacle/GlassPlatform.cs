using System.Runtime.CompilerServices;
using UnityEngine;

public enum GlassPlatformState
{
    None,
    Break
}

public class GlassPlatform : MonoBehaviour, ISaveObstacle
{
    //정상적인 유리 오브젝트
    [SerializeField] private GameObject _glassObject;
    //깨진 유리 오브젝트
    [SerializeField] private GameObject _shatteredObject;

    public GlassPlatformState state = GlassPlatformState.None;

    [SerializeField] private string _id;
    [SerializeField] private ObstacleDataType _type = ObstacleDataType.GlassPlatform;

    public string Id
    {
        get => _id;
        set => _id = value;
    }

    public ObstacleDataType Type => _type;

    private void Start()
    {
        AddList();
    }

    public void Init()
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

    public void AddList()
    {
        ObstacleManager.Instance.saveObstacles.Add(this);
    }

    public ObstacleSaveData ToData()
    {
        ObstacleSaveData saveData = new ObstacleSaveData();
        saveData.glassPlatformState = state;
        return saveData;
    }

    public void LoadtoData(ObstacleSaveData data)
    {
        state = data.glassPlatformState;
        Init();
    }

#if UNITY_EDITOR
    public void SetId(string newId)
    {
        Id = newId;
    }
#endif
}
