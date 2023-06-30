using UnityEngine;

namespace Statics
{
    public static class PlayerPrefManager
    {
        private const string HighScore = "HighScore";

        public static int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
    
        public static int GetHighScore(int levelNum)
        {
            return GetInt(HighScore + levelNum);
        }

        public static void SetHighScore(int levelNum, int highScore)
        {
            SetInt(HighScore + levelNum, highScore);
        }
    }
}
