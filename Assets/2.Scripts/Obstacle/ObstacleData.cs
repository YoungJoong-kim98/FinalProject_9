using UnityEngine;

public class ObstacleData : MonoBehaviour
{
    //�÷��̾��� ����
    [Header("Player")]
    //�÷����� �������� ���ѵǴ� �ð�
    public float moveLockTime = 0.5f;

    //�����̴� ����
    [Header("Platform")]
    //������������ �ð�
    public float disapearTime = 1;
    //�����Ǳ������ �ð�
    public float apearTime = 5;

    //���� ����
    [Header("Jump Platform")]
    //�����ϴ� ��
    public float jumpPower = 15;

    //��ġ ��ֹ�
    [Header("Punch Obstacle")]
    //�̴� ��
    public float pushPower = 15f;
    //��ġ�ϴ� �ӵ�
    public float pushSpeed = 15f;
    //���ư��� �ӵ�
    public float backSpeed = 5f;
    //�����̴� �Ÿ�
    public float moveDistance = 5f;

    //�ݻ� ��ֹ�
    [Header("Reflect Obstacle")]
    //�ݻ��ϴ� ��
    public float reflectPower = 30f;
    //�ּ� ��
    public float reflectMinPower = 15f;
    //�ִ� ��
    public float reflectMaxPower = 60f;

    //ȸ���ϴ� ��ֹ�
    [Header("Rotate Obstacle")]
    //ȸ���ϴ� �ӵ�
    public float rotateSpeed = 10f;

    //��ǳ�� ����
    [Header("Fan Area")]
    //��ǳ�� ��
    public float forceStrength = 1f;

    //�����̴� ����
    [Header("Move Platform")]
    //�����̴� �ӵ�
    public float moveSpeed = 5f;
}
