/**
 * @details ������Ʈ(�ǹ�, ��Ÿ)�� ����
 * @author ��̼�
 * @date 22-04-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;

    public List<Building> BuildingList = new List<Building>();

    public void Awake()
    {
        instance = this;
    }
}
