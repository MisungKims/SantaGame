/**
 * @brief �� ����/�������� ����
 * @author ��̼�
 * @date 22-06-01
 */

using UnityEngine;
using UnityEngine.UI;

public class Closet : MonoBehaviour
{
    #region ����
    public Clothes clothes;     // � ������

    // UI ����
    [SerializeField]
    protected Text clothesNameText;
    [SerializeField]
    protected Image clothesImage;

    private string clothesName;         // ���� �̸�
    public string ClothesName
    {
        set
        {
            clothesName = value;
            clothesNameText.text = clothesName;
        }
    }

    [SerializeField]
    protected RabbitModel model;      // ���� ��
    #endregion

    #region �Լ�
    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="clothes">������ ��</param>
    public virtual void SetClothes(Clothes clothes)
    {
        this.clothes = clothes;
        ClothesName = clothes.clothesName;
        clothesImage.sprite = clothes.image;
    }

    /// <summary>
    /// �𵨿��� ���� ������ (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void Wear()
    {
        if (model.clothes != clothes)       // ���� ���� �ʰų� �ٸ� ���� �԰� ���� ������ ���� ����
        {
            model.PutOn(clothes);
        }
        else         // �ش� ������ ���� �԰� �־��ٸ� ���� ����
        {
            model.PutOff();
        }
    }
    #endregion
}
