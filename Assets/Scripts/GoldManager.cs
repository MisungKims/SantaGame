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
    private static readonly int aAscii = 97;
    private static readonly int zAscii = 122;

    private static readonly List<string> unitOfGold = new List<String>();

    private StringBuilder sb = new StringBuilder();
    
    private static void InitUnit(int capacity)
    {
        unitCapacity += capacity;

        unitOfGold.Clear();
        unitOfGold.Add("");

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
            }
        }

        isInit = true;
    }

    public static int GetPoint(int value)
    {
        return (value % 1000) / 100;
    }

    private static (int value, int index, int point) GetSize(BigInteger value)
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

        int point = GetPoint(lastVal);

        while (unitOfGold.Count <= index)
        {
            InitUnit(5);
        }

        return ((int)currentVal, index, point);
    }

    public static string ExpressUnitOfGold(BigInteger myGold)
    {
        if (!isInit)
        {
            InitUnit(5);
        }

        var sizeStruct = GetSize(myGold);

        return string.Format("{0}.{1}{2}", sizeStruct.value, sizeStruct.point, unitOfGold[sizeStruct.index]);
    }

}

