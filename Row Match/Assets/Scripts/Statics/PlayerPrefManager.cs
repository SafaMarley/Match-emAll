using UnityEngine;

namespace Statics
{
    public static class PlayerPrefManager
    {
        private const string HighScore = "HighScore";
        private const string PlayerLevel = "PlayerLevel";
        private const string LevelsDownloaded = "LevelsDownloaded";

        /*
        public static int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }*/
    
        public static int GetHighScore(int levelNum)
        {
            return PlayerPrefs.GetInt(HighScore + levelNum);
        }

        public static void SetHighScore(int levelNum, int highScore)
        {
            PlayerPrefs.SetInt(HighScore + levelNum, highScore);
        }

        public static int GetPlayerLevel()
        {
            return PlayerPrefs.GetInt(PlayerLevel);
        }
        
        public static void SetPlayerLevel()
        {
            PlayerPrefs.SetInt(PlayerLevel, 1);
        }
        
        public static int GetLevelStatus()
        {
            return PlayerPrefs.GetInt(LevelsDownloaded);
        }
        
        public static void SetLevelStatus()
        {
            PlayerPrefs.SetInt(LevelsDownloaded, 1);
        }

        public static void PlayerLevelUp()
        {
            PlayerPrefs.SetInt(PlayerLevel, PlayerPrefs.GetInt(PlayerLevel) + 1);
        }
    }
}
