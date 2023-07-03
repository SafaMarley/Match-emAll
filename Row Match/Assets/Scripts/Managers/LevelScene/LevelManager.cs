using System;
using System.Collections.Generic;
using System.IO;
using Managers.Base;
using Managers.MainScene;
using Statics;
using UnityEngine;

namespace Managers.LevelScene
{
    [Serializable]
    public struct LevelInfo
    {
        private bool _isAccessible;
        public bool IsAccessible { get => _isAccessible; set => _isAccessible = value; }

        private int _levelNumber;
        public int LevelNumber { get => _levelNumber; }

        private int _gridWidth;
        public int GridWidth { get => _gridWidth; }

        private int _gridHeight;
        public int GridHeight { get => _gridHeight; }

        private int _moveCount;
        public int MoveCount { get => _moveCount; }

        private string _gridContent;
        public string GridContent { get => _gridContent; }

        public LevelInfo(int levelNumber, int gridWidth, int gridHeight, int moveCount, string gridContent)
        {
            _isAccessible = PlayerPrefManager.GetPlayerLevel() >= levelNumber;
            _levelNumber = levelNumber;
            _gridWidth = gridWidth;
            _gridHeight = gridHeight;
            _moveCount = moveCount;
            _gridContent = gridContent;
        }

        public void ActivateLevel()
        {
            IsAccessible = true;
        }
    }

    public class LevelManager : MonoSingleton<LevelManager>
    {
        private List<LevelInfo> _levelInfos = new List<LevelInfo>();

        public List<LevelInfo> LevelInfos { get => _levelInfos; }
    
        public void LoadLevelsFromProject()
        {
            string localLevelsPath = Path.Combine(Application.dataPath, "Levels");
            if (Directory.Exists(localLevelsPath))
            {
                foreach (string levelFiles in Directory.GetFiles(localLevelsPath, "*."))
                {
                    AddLevelsFromFile(levelFiles);
                }
            }
            //==== For Offline mode only
            _levelInfos.Sort((level1, level2) => level1.LevelNumber.CompareTo(level2.LevelNumber));
            MainMenuUIManager.Instance.LoadLevelsToUI();
            //==== For Offline mode only
        }
        
        public void LoadLevelsFromPersistent()
        {
            string persistentLevelsPath = Application.persistentDataPath;
            if (Directory.Exists(persistentLevelsPath))
            {
                foreach (string levelFiles in Directory.GetFiles(persistentLevelsPath))
                {
                    AddLevelsFromFile(levelFiles);
                }
            }
            _levelInfos.Sort((level1, level2) => level1.LevelNumber.CompareTo(level2.LevelNumber));
            MainMenuUIManager.Instance.LoadLevelsToUI();
        }
    
        public void AddLevelsFromFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            LevelInfo levelInfo = new LevelInfo(
                int.Parse(lines[0].Substring(lines[0].IndexOf(' ') + 1)),
                int.Parse(lines[1].Substring(lines[1].IndexOf(' ') + 1)),
                int.Parse(lines[2].Substring(lines[2].IndexOf(' ') + 1)),
                int.Parse(lines[3].Substring(lines[3].IndexOf(' ') + 1)),
                lines[4].Substring(lines[4].IndexOf(' ') + 1).Replace(",", "")
            );
            _levelInfos.Add(levelInfo);
        }
    }
}