/**
 * @brief ����ڰ� ���� ���� UI ����
 * @author ��̼�
 * @date 22-06-04
 */

using UnityEngine;
using UnityEngine.UI;

public class ClothesSlot : MonoBehaviour
{
    #region ����
    // UI ����
    [SerializeField]
    private Image clothesImage;             // ������ �� �̹���

    [SerializeField]
    private GameObject checkImage;          // ���� ���� ���θ� �����ִ� �̹���

    [SerializeField]
    private Button button;                  // ������ ��ư

    private bool isWearing;                 // ���� ���� ����
    public bool IsWearing
    {
        set
        {
            isWearing = value;
            checkImage.SetActive(isWearing);
        }
    }

    public RabbitCitizen rabbitCitizen;     // ���� �䳢 �ֹ�â�� �䳢

    public Clothes clothes;                 // �ش� ������ ��

    // ĳ��
    private ClothesManager clothesManager;
    #endregion

    #region ����Ƽ �Լ�
    void Awake()
    {
        clothesManager = ClothesManager.Instance;
    }

    private void OnEnable()
    {
        CheckWearing();

        SetInteractable();
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ���� UI ����
    /// </summary>
    /// <param name="myClothes">�ش� ������ ��</param>
    public void SetClothes(Clothes myClothes)
    {
        clothes = myClothes;
        clothesImage.gameObject.SetActive(true);
        clothesImage.sprite = myClothes.image;
    }

    /// <summary>
    /// ���� UI�� �ʱ� ���·�
    /// </summary>
    public void Reset()
    {
        clothes = null;
        clothesImage.gameObject.SetActive(false);
        checkImage.SetActive(false);
    }

    /// <summary>
    /// �� ������ ���� �԰��ִ���?
    /// </summary>
    /// <returns></returns>
    bool CheckWearing()
    {
        if (!rabbitCitizen)
        {
           rabbitCitizen = UIManager.Instance.citizenPanel.rabbitCitizen;
        }

        if (rabbitCitizen.clothes == clothes && clothes != null)       // �ش� ���� �԰� ���� ��
        {
            IsWearing = true;
            return true;
        }
        else    //���� �԰����� ���� ��
        {
            IsWearing = false;
            return false;
        }
    }

    /// <summary>
    /// ��ư�� Interactable ����
    /// </summary>
    void SetInteractable()
    {
        if (clothes == null)
        {
            button.interactable = false;
        }
        else
        {
            // ���� ���� �� ����, �䳢�� ������ ���� �԰� ���� ���� �� ��ư�� ���� �� ����
            if (clothes.clothesInfo.totalAmount <= clothes.clothesInfo.wearingCount && rabbitCitizen.clothes != clothes)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
    }

    /// <summary>
    /// �䳢 �ֹο��� ���� �����ų� ����
    /// </summary>
    public void PutOnOrOffRabbit()
    {
        if (!rabbitCitizen) return;

        if (CheckWearing())     // �Ȱ��� ���� �԰����� ���� ����
        {
            PutOff();
        }
        else   // �Ȱ��� ���� �԰����� ���� ���� ����
        {
            if (clothes.clothesInfo.totalAmount > clothes.clothesInfo.wearingCount)
            {
                // �̹� �ٸ� ���� �԰� ���� ������ �� ���� ã�� ����
                if (rabbitCitizen.clothes != null)      
                {
                    for (int i = 0; i < clothesManager.clothesSlotCount; i++)
                    {
                        if (clothesManager.clothesSlotList[i].clothes == rabbitCitizen.clothes)
                        {
                            clothesManager.clothesSlotList[i].PutOff();
                            break;
                        }
                    }
                }

                PutOn();
            }
        }

        SetInteractable();
    }

    /// <summary>
    /// �䳢���� ���� ����
    /// </summary>
    void PutOn()
    {
        if (rabbitCitizen.PutOn(clothes))
        {
            clothes.clothesInfo.wearingCount++;
            IsWearing = true;
        }
    }

    /// <summary>
    /// �䳢���� ���� ����
    /// </summary>
    void PutOff()
    {
        rabbitCitizen.PutOff();
        clothes.clothesInfo.wearingCount--;

        IsWearing = false;
    }
    #endregion
}
