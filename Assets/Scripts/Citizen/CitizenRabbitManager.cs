/**
 * @brief ��Ÿ �ֹ��� ����
 * @author ��̼�
 * @date 22-04-22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class goal
{
    public goalObject[] goalObjects;
}

[System.Serializable]
public class goalObject
{
    public Transform pos;     // �ǹ����� �ֹ��� �� �� �ִ� ��ġ
    public bool isUse;          // �ٸ� �ֹ��� �̹� ��� ���ΰ�?
}

public class CitizenRabbitManager : MonoBehaviour
{
    public List<goal> goalPositions = new List<goal>();       // �ֹ��� �� �� �ִ� �ǹ��� ��ġ ����Ʈ

    // �̱���
    private static CitizenRabbitManager instance;
    public static CitizenRabbitManager Instance
    {
        get { return instance; }
    }

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }
    }

}
