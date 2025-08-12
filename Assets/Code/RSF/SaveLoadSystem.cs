using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    private const string SAVE_KEY = "RSF_PlayerData";

    [System.Serializable]
    public class SaveData
    {
        public int money;
        public int[] handRankLevels = new int[11];
    }

    public void SaveGame()
    {
        PlayerData playerData = PlayerData.Instance;
        SaveData saveData = new SaveData();

        saveData.money = playerData.currentMoney;
        saveData.handRankLevels[0] = playerData.onePairLevel;
        saveData.handRankLevels[1] = playerData.twoPairLevel;
        saveData.handRankLevels[2] = playerData.redFlushLevel;
        saveData.handRankLevels[3] = playerData.blackFlushLevel;
        saveData.handRankLevels[4] = playerData.tripleLevel;
        saveData.handRankLevels[5] = playerData.straightLevel;
        saveData.handRankLevels[6] = playerData.flushLevel;
        saveData.handRankLevels[7] = playerData.fullHouseLevel;
        saveData.handRankLevels[8] = playerData.fourKindLevel;
        saveData.handRankLevels[9] = playerData.straightFlushLevel;
        saveData.handRankLevels[10] = playerData.royalStraightFlushLevel;

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("Game saved!");
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            PlayerData playerData = PlayerData.Instance;
            playerData.currentMoney = saveData.money;
            playerData.onePairLevel = saveData.handRankLevels[0];
            playerData.twoPairLevel = saveData.handRankLevels[1];
            playerData.redFlushLevel = saveData.handRankLevels[2];
            playerData.blackFlushLevel = saveData.handRankLevels[3];
            playerData.tripleLevel = saveData.handRankLevels[4];
            playerData.straightLevel = saveData.handRankLevels[5];
            playerData.flushLevel = saveData.handRankLevels[6];
            playerData.fullHouseLevel = saveData.handRankLevels[7];
            playerData.fourKindLevel = saveData.handRankLevels[8];
            playerData.straightFlushLevel = saveData.handRankLevels[9];
            playerData.royalStraightFlushLevel = saveData.handRankLevels[10];

            Debug.Log("Game loaded!");
        }
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        Debug.Log("Save data deleted!");
    }
}