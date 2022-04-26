using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectFlag // �迭�̳� ����Ʈ�� ����
{
    getCarrotButton
}
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance; // �ν��Ͻ� ���ؼ� get set�ϵ���

    public GameObject[] cpyObject; //������ ģ��
    public List<Queue<GameObject>> queList = new List<Queue<GameObject>>(); // ���� ť
    public int[] initCount; //�ʱ� ���� ����


    private void init(int count, GameObject gb, int flag)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject tempGb = GameObject.Instantiate(gb, this.transform);
            tempGb.gameObject.SetActive(false);
            queList[flag].Enqueue(tempGb);
        }
    }

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < cpyObject.Length; i++) // ��� �迭 Ž���ؼ�  initCount ��ŭ �̸� ����
        {
            queList.Add(new Queue<GameObject>());
            init(initCount[i], cpyObject[i], i);
        }
    }

    public GameObject Get(EObjectFlag flag) //ȣ���ϴ� ģ���� �� �� ����������� �˷���
    {
        int index = (int)flag;
        GameObject tempGb;

        if (queList[index].Count > 0) // �ش��ϴ� ť�� ���� ������Ʈ�� ���� ������ �װ��� ��ȯ
        {
            tempGb = queList[index].Dequeue(); //��ť�� ���ؼ� ���������� ��ȯ

            //tempGb.transform.SetParent(null);
            tempGb.SetActive(true);

        }
        else // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(cpyObject[index]);
        }

        Debug.Log("get");

        return tempGb;
    }

    public GameObject Get(EObjectFlag flag , Vector3 pos , Vector3 rot) //ȣ���ϴ� ģ���� �� �� ����������� �˷���
    {
        int index = (int)flag;
        GameObject tempGb;

        if (queList[index].Count > 0) // �ش��ϴ� ť�� ���� ������Ʈ�� ���� ������ �װ��� ��ȯ
        {
            tempGb = queList[index].Dequeue(); //��ť�� ���ؼ� ���������� ��ȯ

            //tempGb.transform.SetParent(null);
            tempGb.SetActive(true);

        }
        else // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(cpyObject[index]);
        }

        tempGb.transform.position = pos;
        tempGb.transform.eulerAngles = rot;

        return tempGb;
    }


    public void Set(GameObject gb, EObjectFlag flag) // �� �ٽ�� ť�� �����ٰ�
    {
        int index = (int)flag;
        gb.SetActive(false);
        gb.transform.SetParent(this.transform);
        queList[index].Enqueue(gb);
    }
}
