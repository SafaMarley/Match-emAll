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
            if (PlayerPrefManager.GetInt("PlayerLevel") == 0)
            {
                PlayerPrefManager.SetInt("PlayerLevel", 1);
            }

            LevelManager.Instance.LoadLevelsFromProject();

            if (PlayerPrefManager.GetInt("LevelsReady") == 0 && Application.internetReachability != NetworkReachability.NotReachable)
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
