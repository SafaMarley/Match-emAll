using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Managers.Base;
using Managers.LevelScene;
using Statics;
using UnityEngine;
using UnityEngine.Networking;

namespace Managers.MainScene
{
    [Serializable]
    struct LevelURLInfo
    {
        public string levelTag;
        public int levelCountFrom;
        public int levelCountTo;
    }

    public class DownloadManager : MonoSingleton<DownloadManager>
    {
        [SerializeField] private string downloadURLBase;
        [SerializeField] private List<LevelURLInfo> levelInfosList = new List<LevelURLInfo>();
        private string _persistentPath;
        private int _filesToPrepare;
        private int _filesReady;

        public void BeginDownload()
        {
            _persistentPath = Application.persistentDataPath;
        
            foreach (LevelURLInfo levelURLInfo in levelInfosList)
            {
                _filesToPrepare += levelURLInfo.levelCountTo - levelURLInfo.levelCountFrom + 1;
            }
        
            foreach (LevelURLInfo info in levelInfosList)
            {
                for (int i = info.levelCountFrom; i <= info.levelCountTo; i++)
                {
                    string filePath = Path.Combine(_persistentPath, info.levelTag + i);
                
                    if (!File.Exists(filePath))
                    {
                        StartCoroutine(DownloadFile(downloadURLBase, info.levelTag + i, filePath, OnDownloadEndAction));
                    }
                    else
                    {
                        OnDownloadEndAction(filePath);
                    }
                }
            }
        }
    
        private IEnumerator DownloadFile(string fileURLBase, string fileName, string filePath, Action<string> downloadEndAction)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(fileURLBase + fileName))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    _filesToPrepare--;
                    Debug.LogError("File " + fileName + " download failed. Error: " + webRequest.error);
                }
                else
                {
                    File.WriteAllBytes(filePath, webRequest.downloadHandler.data);
                    downloadEndAction(filePath);
                }
                webRequest.Dispose();
            }
        }

        private void OnDownloadEndAction(string filePath)
        {
            _filesReady++;
            if (_filesToPrepare == _filesReady)
            {
                PlayerPrefManager.SetInt("LevelsReady", 1);
                LevelManager.Instance.LoadLevelsFromPersistent();
            }
        }
    }
}