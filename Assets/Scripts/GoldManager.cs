using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Text;

public class GoldManager : MonoBehaviour
{

    private static readonly BigInteger unitSize = 1000;
    private static bool isInit = false;
    private static int unitCapacity = 5;
    private static readonly int aAscii = 65;
    private static readonly int zAscii = 90;

    private static readonly List<string> unitOfGold = new List<String>();
    private static Dictionary<string, BigInteger> unitsMap = new Dictionary<string, BigInteger>();

    private StringBuilder sb = new StringBuilder();

    private static void InitUnit(int capacity)
    {
        unitCapacity += capacity;

        unitOfGold.Clear();
        unitOfGold.Add("");

        unitsMap.Clear();
        unitsMap.Add("", 0);

        string unit = "";
        float iCnt;
        int nextAscii;
        char first;
        char second;

        for (int i = 0; i <= unitCapacity; i++)
        {
            for (int j = aAscii; j <= zAscii; j++)
            {
                if (i == 0)
                    unit = ((char)j).ToString();
                else
                {
                    iCnt = (float)i / 26;
                    nextAscii = aAscii + i - 1;
                    first = (char)nextAscii;
                    second = (char)j;
                    unit = string.Format("{0}{1}", first, second);
                }
                unitOfGold.Add(unit);
                unitsMap.Add(unit, BigInteger.Pow(unitSize, unitOfGold.Count - 1));
            }
        }

        isInit = true;
    }

    public static int GetDecimal(int value)
    {
        return (value % 1000) / 100;
    }

    private static (int value, int index, int decimalPoint) GetSize(BigInteger value)
    {
        BigInteger currentVal = value;
        int index = 0;
        int lastVal = 0;

        while (currentVal > unitSize - 1)
        {
            BigInteger predCurrentVal = currentVal / unitSize;
            if (predCurrentVal <= unitSize - 1)
            {
                lastVal = (int)currentVal;
            }
            currentVal = predCurrentVal;
            index += 1;
        }

        int point = GetDecimal(lastVal);

        while (unitOfGold.Count <= index)
        {
            InitUnit(5);
        }

        return ((int)currentVal, index, point);
    }

    /// <summary>
    /// ���ڸ� ������ �ٿ� ����
    /// </summary>
    /// <param name="myGold"></param>
    /// <returns></returns>
    public static string ExpressUnitOfGold(BigInteger myGold)
    {
        if (!isInit)
        {
            InitUnit(5);
        }

        var sizeStruct = GetSize(myGold);

        return string.Format("{0}.{1} {2}", sizeStruct.value, sizeStruct.decimalPoint, unitOfGold[sizeStruct.index]);
    }

    /// <summary>
    /// ������ ���ڷ� ����
    /// </summary>
    /// <param name="unit">����</param>
    /// <returns></returns>
    public static BigInteger UnitToBigInteger(string unit)
    {
        if (!isInit)
        {
            InitUnit(5);
        }

        string[] strArr = unit.Split('.');

        // �Ҽ����� ���� ����
        if (strArr.Length >= 2)
        {
            BigInteger value = BigInteger.Parse(strArr[0]);                                         // �Ҽ��� ���� ������
            BigInteger decimalPoint = BigInteger.Parse((Regex.Replace(strArr[1], "[^0-9]", "")));   // �Ҽ��� ���ڸ�
            string unitStr = Regex.Replace(strArr[1], "[^A-Z]", "");                                // ����

            if (unitStr == "")
            {
                return value;
            }

            if (decimalPoint == 0)
            {
                return (unitsMap[unitStr] * value);
            }
            else
            {
                var unitValue = unitsMap[unitStr];
                return (unitValue * value) + (unitValue / 10) * decimalPoint;
            }
        }
        // ��Ҽ��� ���� ����
        else
        {
            BigInteger value = BigInteger.Parse((Regex.Replace(unit, "[^0-9]", "")));
            string unitStr = Regex.Replace(unit, "[^A-Z]", "");

            while (!unitsMap.ContainsKey(unitStr))
            {
                InitUnit(5);
            }

            BigInteger result = unitsMap[unitStr] * value;

            if (result == 0)
                return int.Parse((unit));
            else
                return result;
        }
    }

    /// <summary>
    /// mine���� ������ True��ȯ
    /// </summary>
    /// <param name="unitStr">���� ����</param>
    /// <returns></returns>
    public static bool CompareBigintAndUnit(BigInteger myInteger, string unitStr)
    {
        if (myInteger >= UnitToBigInteger(unitStr))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

   /// <summary>
   /// ������ float�� ���Ͽ� ��ȯ
   /// </summary>
   /// <returns></returns>
    public static string MultiplyUnit(string unitStr, float multiply)
    {
        if (Char.IsLetter(unitStr[unitStr.Length - 1]))     //������ �ִ� ������ ��
        {
            BigInteger bigInteger = UnitToBigInteger(unitStr);

            BigInteger resultInt = new BigInteger((double)bigInteger * multiply);

            return ExpressUnitOfGold(resultInt);
        }
        else         // ������ ���� ������ ��
        {
            float fUnit = float.Parse(unitStr);

            if (fUnit * multiply >= 1000.0f)
            {
                return ExpressUnitOfGold((BigInteger)(fUnit * multiply));
            }
            else
            {
                return string.Format("{0:F1}", (fUnit * multiply));
            }
        }
    }
}

