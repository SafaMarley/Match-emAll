using Managers.Base;
using Managers.LevelScene;
using Statics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers.MainScene
{
    public class GameManager : MonoSingleton<GameManager>
    {
        void Start()
        {
            if (PlayerPrefManager.GetPlayerLevel() == 0)
            {
                PlayerPrefManager.SetPlayerLevel();
            }

            LevelManager.Instance.LoadLevelsFromProject();

            if (PlayerPrefManager.GetLevelStatus() == 0 && Application.internetReachability != NetworkReachability.NotReachable)
            {
                DownloadManager.Instance.BeginDownload();
            }
            else
            {
                LevelManager.Instance.LoadLevelsFromPersistent();
            }
        
            MainMenuUIManager.Instance.Initialize();
        }
    
        public void OnPlayButtonClicked()
        {
            SceneManager.LoadScene("Scenes/LevelScene");
        }
    }
}
