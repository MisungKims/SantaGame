/**
 * @brief ������ ����
 * @author ��̼�
 * @date 22-04-24
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GiftManager : MonoBehaviour
{
    #region ����
    public List<Gift> giftList = new List<Gift>();

    int totalWeight = 0;

    // ĳ��
    private GetRewardWindow getRewardWindow;

    // �̱���
    private static GiftManager instance;
    public static GiftManager Instance
    {
        get { return instance; }
    }
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        getRewardWindow = UIManager.Instance.getRewardWindow;

        if (!LoadData())
        {
            for (int i = 0; i < giftList.Count; i++)
            {
                giftList[i].inventoryIndex = -1;
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < giftList.Count; i++)
        {
            totalWeight += Gift.GetWeight(giftList[i].giftGrade);
        }
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ���� ���� ��ȯ
    /// </summary>
    public Gift RandomGift()
    {
        // ����ġ ���� ���
        int weight = 0;
        int select = Mathf.RoundToInt(totalWeight * Random.Range(0.0f, 1.0f));

        for (int i = 0; i < giftList.Count; i++)
        {
            weight += Gift.GetWeight(giftList[i].giftGrade);
            if (select <= weight)
            {
                return new Gift(giftList[i]);
            }
        }

        return null;
    }

    ///// <summary>
    ///// ���� ����Ʈ�� �߰��� ������ �������� ��ȯ
    ///// </summary>
    ///// <returns></returns>
    //public Gift RandomWishListGift()
    //{
    //    if (!IsHaveWishList())       // ���ø���Ʈ�� ������� ��
    //    {
    //        return null;
    //    }

    //    Gift gift = RandomGift();
    //    if (gift.wishCount > 0)     // �������� ������ ������ ���ø���Ʈ�� ���� �� ��ȯ
    //    {
    //        return gift;
    //    }
    //    else
    //    {
    //        return RandomWishListGift();
    //    }
    //}

    ///// <summary>
    ///// ���ø���Ʈ�� ��� ������ ������ false ��ȯ 
    ///// </summary>
    ///// <returns>���ø���Ʈ�� ������ �ϳ��� ������ true</returns>
    //public bool IsHaveWishList()
    //{
    //    bool returnVal = false;

    //    for (int i = 0; i < giftList.Count; i++)
    //    {
    //        if (giftList[i].wishCount > 0)
    //        {
    //            returnVal = true;
    //            break;
    //        }
    //    }

    //    return returnVal;
    //}

    /// <summary>
    /// �������� ������ ����
    /// </summary>
    public void ReceiveRandomGift()
    {
        ReceiveGift(RandomGift().giftType);
    }

    /// <summary>
    /// ���� ������ �κ��丮�� ����
    /// </summary>
    public void ReceiveGift(EGiftType giftType)
    {
        Gift gift = giftList[(int)giftType];

        getRewardWindow.OpenWindow(gift);       // ���� ȹ��â ������
        Inventory.Instance.AddItem(gift);       // �κ��丮�� ����
    }
    #endregion
    //���� Ȱ��ȭ ���¸� �����ϴ� ����
    bool isPaused = false;

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = true;

            SaveData();         // ���� ��Ȱ��ȭ�Ǿ��� �� ������ ����
        }

        else
        {
            if (isPaused)
            {
                isPaused = false;
                /* ���� Ȱ��ȭ �Ǿ��� �� ó�� */
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveData();         // �� ���� �� ������ ����
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    void SaveData()
    {
        string jdata = JsonUtility.ToJson(new Serialization<Gift>(giftList));
        File.WriteAllText(Application.dataPath + "/Resources/GiftData.json", jdata);
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// <returns>�ҷ����� ���� ����</returns>
    public bool LoadData()
    {
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Resources/GiftData.json");
        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.dataPath + "/Resources/GiftData.json");

            giftList = JsonUtility.FromJson<Serialization<Gift>>(jdata).target;
            
            return true;
        }

        return false;
    }
}
