using UnityEngine;

public class RotateObstacle : MonoBehaviour, ISaveObstacle, IObstacle
{
    //회전하는 방향
    [SerializeField] private Vector3 _rotateDirection;
    //회전하는 속도
    [SerializeField] private float _rotateSpeed = -1f;

    [SerializeField] private string _id;
    [SerializeField] private ObstacleDataType _type = ObstacleDataType.RotateObstacle;

    public string Id
    {
        get => _id;
        set => _id = value;
    }

    public ObstacleDataType Type => _type;

    private void Start()
    {
        //ObstacleManager의 rotateObstacles에 추가
        ObstacleManager.Instance.rotateObstacles.Add(this);
        AddList();

        //데이터 초기화
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _rotateSpeed, data.rotateSpeed);
    }

    //회전하는 메서드
    public void Rotate()
    {
        //물리처리
        transform.Rotate(_rotateDirection * _rotateSpeed * Time.fixedDeltaTime);
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
        saveData.rotation = Utilitys.Vector3ToFloat(transform.eulerAngles);
        return saveData;
    }

    public void LoadtoData(ObstacleSaveData data)
    {
        transform.eulerAngles = Utilitys.FloatToVecter3(data.rotation);
    }
}
