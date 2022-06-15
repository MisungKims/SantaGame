/**
 * @brief �䳢 �ֹ�â UI
 * @author ��̼�
 * @date 22-06-04
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public enum EMoveType { none, opening, closing };          

public class CitizenPanel : MonoBehaviour
{
    #region ����
    public RabbitCitizen rabbitCitizen;     // ���� �䳢 �ֹ�â�� �䳢

    [SerializeField]
    private GameObject clothesWindowButton;       // �䳢�� ����â ��ư

    [SerializeField]
    private GameObject clothesWindow;       // �䳢�� ����â

    [SerializeField]
    private Text buttonText;


    private Vector3 upCloset = new Vector3(0, -377f, 0);

    private Vector3 downCloset = new Vector3(0, -546f, 0);

    private EMoveType moveType;         // ������ ���� �ݴ� ����

    // ĳ��
    private ClothesManager clothesManager;
    private ObjectManager objectManager;
    #endregion

    #region ����Ƽ �Լ�

    void Awake()
    {
        clothesManager = ClothesManager.Instance;
        objectManager = ObjectManager.Instance;

    }

    private void OnEnable()
    {
        Open();
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// ���� ������
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCloset()
    {
        while (true)
        {
            // ������ ��
            if (moveType.Equals(EMoveType.opening))
            {
                OpenCloset();

                while (clothesWindowButton.transform.localPosition.y - upCloset.y < 0.05f && moveType.Equals(EMoveType.opening))
                {
                    clothesWindowButton.transform.localPosition = Vector3.Lerp(clothesWindowButton.transform.localPosition, upCloset, Time.deltaTime * 2f);

                    yield return null;
                }

                clothesWindowButton.transform.localPosition = upCloset;
                if (moveType.Equals(EMoveType.opening)) moveType = EMoveType.none;
            }
            // ������ ����
            else if (moveType.Equals(EMoveType.closing))
            {
                while (clothesWindowButton.transform.localPosition.y - downCloset.y > 0.05f && moveType.Equals(EMoveType.closing))
                {
                    clothesWindowButton.transform.localPosition = Vector3.Lerp(clothesWindowButton.transform.localPosition, downCloset, Time.deltaTime * 2f);

                    yield return null;
                }

                if (moveType.Equals(EMoveType.closing)) moveType = EMoveType.none;
                CloseCloset();
            }

            yield return null;
        }
    }
    #endregion

    #region �Լ�

    public void Open()
    {
        // �ʰ��԰� ������� �Ǿ����� ���� â�� ������
        if (objectManager.objectList[7].buildingLevel > 0)
        {
            if (!clothesWindowButton.activeSelf)
            {
                clothesWindowButton.SetActive(true);
            }

            CloseCloset();
            StartCoroutine(MoveCloset());
        }
        else if (clothesWindowButton.activeSelf && objectManager.objectList[7].buildingLevel <= 0)
        {
            clothesWindowButton.SetActive(false);
        }
    }

    /// <summary>
    /// ���� ���ų� �ݱ� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void OpenOrCloseCloset()
    {
        switch (moveType)
        {
            case EMoveType.none:
                // ������ �����ִٸ� �ݰ�, �����ִٸ� ����
                if (clothesWindow.activeSelf)
                {
                    moveType = EMoveType.closing;
                }
                else
                {
                    moveType = EMoveType.opening;
                }
                break;

            case EMoveType.opening:
                moveType = EMoveType.closing;
                break;

            case EMoveType.closing:
                moveType = EMoveType.opening;
                break;
        }
        
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void OpenCloset()
    {
        buttonText.text = "<";
        InitClothesSlot();
        clothesWindow.SetActive(true);
    }

    /// <summary>
    /// ���� �ݱ�
    /// </summary>
    public void CloseCloset()
    {
        buttonText.text = ">";
        clothesWindow.SetActive(false);
        clothesWindowButton.transform.localPosition = downCloset;
    }

    /// <summary>
    /// ���� ���� UI �ʱ� ����
    /// </summary>
    public void InitClothesSlot()
    {
        // �� UI ���Ե��� rabbitCitizen�� ���� �䳢�� ����
        for (int i = 0; i < clothesManager.clothesSlotList.Count; i++)
        {
            clothesManager.clothesSlotList[i].rabbitCitizen = rabbitCitizen;
            clothesManager.clothesSlotList[i].gameObject.SetActive(true);
        }
    }
    #endregion
}
