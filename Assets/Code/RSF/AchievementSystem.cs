using UnityEngine;
using System.Collections.Generic;

public class AchievementSystem : MonoBehaviour
{
    [System.Serializable]
    public class Achievement
    {
        public string name;
        public string description;
        public bool isUnlocked;
        public System.Func<bool> checkCondition;
    }

    [Header("Achievement UI")]
    public GameObject achievementNotificationPrefab;
    public Transform notificationParent;

    private List<Achievement> achievements = new List<Achievement>();

    public static AchievementSystem Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeAchievements();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAchievements()
    {
        achievements.Add(new Achievement
        {
            name = "First Win",
            description = "Win your first hand",
            checkCondition = () => GameStatistics.Instance.currentWinStreak >= 1
        });

        achievements.Add(new Achievement
        {
            name = "Lucky Seven",
            description = "Win 7 hands in a row",
            checkCondition = () => GameStatistics.Instance.bestWinStreak >= 7
        });

        achievements.Add(new Achievement
        {
            name = "Royal Flush Master",
            description = "Get a Royal Straight Flush",
            checkCondition = () => GameStatistics.Instance.royalStraightFlushCount >= 1
        });

        achievements.Add(new Achievement
        {
            name = "Combo King",
            description = "Achieve 10 combo bonuses",
            checkCondition = () => GameStatistics.Instance.comboAchievements >= 10
        });

        achievements.Add(new Achievement
        {
            name = "High Roller",
            description = "Win $50,000 or more in a single hand",
            checkCondition = () => GameStatistics.Instance.bigWins >= 1
        });
    }

    public void CheckAchievements()
    {
        foreach (Achievement achievement in achievements)
        {
            if (!achievement.isUnlocked && achievement.checkCondition())
            {
                UnlockAchievement(achievement);
            }
        }
    }

    private void UnlockAchievement(Achievement achievement)
    {
        achievement.isUnlocked = true;

        Debug.Log("Achievement Unlocked: " + achievement.name);

        SaveAchievements();
    }

    private void SaveAchievements()
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            PlayerPrefs.SetInt("Achievement_" + i, achievements[i].isUnlocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void LoadAchievements()
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            achievements[i].isUnlocked = PlayerPrefs.GetInt("Achievement_" + i, 0) == 1;
        }
    }
}
