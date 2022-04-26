using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectFlag // 배열이나 리스트의 순서
{
    getCarrotButton
}
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance; // 인스턴스 통해서 get set하도록

    public GameObject[] cpyObject; //복제할 친구
    public List<Queue<GameObject>> queList = new List<Queue<GameObject>>(); // 담을 큐
    public int[] initCount; //초기 생성 숫자


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

        for (int i = 0; i < cpyObject.Length; i++) // 모든 배열 탐색해서  initCount 만큼 미리 생성
        {
            queList.Add(new Queue<GameObject>());
            init(initCount[i], cpyObject[i], i);
        }
    }

    public GameObject Get(EObjectFlag flag) //호출하는 친구야 너 뭐 가지고싶은지 알려줘
    {
        int index = (int)flag;
        GameObject tempGb;

        if (queList[index].Count > 0) // 해당하는 큐에 게임 오브젝트가 남아 있으면 그것을 반환
        {
            tempGb = queList[index].Dequeue(); //디큐를 통해서 실질적으로 반환

            //tempGb.transform.SetParent(null);
            tempGb.SetActive(true);

        }
        else // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(cpyObject[index]);
        }

        Debug.Log("get");

        return tempGb;
    }

    public GameObject Get(EObjectFlag flag , Vector3 pos , Vector3 rot) //호출하는 친구야 너 뭐 가지고싶은지 알려줘
    {
        int index = (int)flag;
        GameObject tempGb;

        if (queList[index].Count > 0) // 해당하는 큐에 게임 오브젝트가 남아 있으면 그것을 반환
        {
            tempGb = queList[index].Dequeue(); //디큐를 통해서 실질적으로 반환

            //tempGb.transform.SetParent(null);
            tempGb.SetActive(true);

        }
        else // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(cpyObject[index]);
        }

        tempGb.transform.position = pos;
        tempGb.transform.eulerAngles = rot;

        return tempGb;
    }


    public void Set(GameObject gb, EObjectFlag flag) // 나 다썼어 큐에 돌려줄게
    {
        int index = (int)flag;
        gb.SetActive(false);
        gb.transform.SetParent(this.transform);
        queList[index].Enqueue(gb);
    }
}
