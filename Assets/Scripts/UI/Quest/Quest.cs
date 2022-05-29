/**
 * @brief Äù½ºÆ® ±¸Á¶Ã¼
 * @author ±è¹Ì¼º
 * @date 22-04-19
 */

[System.Serializable]
public class Quest
{
    public int id;
    public string name;
    public int count;
    public int maxCount;
    public string rewardType;
    public string amount;
    public bool isSuccess;
    public bool isGetReward;

    public Quest(int id, string name, int count, int maxCount, string rewardType, string amount, bool isSuccess, bool isGetReward)
    {
        this.id = id;
        this.name = name;
        this.count = count;
        this.maxCount = maxCount;
        this.rewardType = rewardType;
        this.amount = amount;
        this.isSuccess = isSuccess;
        this.isGetReward = isGetReward;
    }
}
