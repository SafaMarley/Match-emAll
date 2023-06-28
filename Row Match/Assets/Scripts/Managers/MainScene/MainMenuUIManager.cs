using Gameplay;
using Managers.Base;
using Managers.LevelScene;
using Statics;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.MainScene
{
    public class MainMenuUIManager : MonoSingleton<MainMenuUIManager>
    {   
        [Header("Menu Buttons")]
        [SerializeField] private Button levelsButton;
    
        [Header("Menu Panels")]
        [SerializeField] private RectTransform levelsPanel;

        [Header("LevelContentHolder")]
        [SerializeField] private RectTransform levelContentHolder;
    
        [Header("Prefabs")]
        [SerializeField] private LevelInformationUI levelPrefab;

        public void Initialize()
        {
            levelsButton.onClick.AddListener(DisplayLevelPanel);
            if (GameState.CurrentGameState != State.Launched)
            {
                DisplayLevelPanel();
            }
        }

        private void DisplayLevelPanel()
        {
            levelsPanel.gameObject.SetActive(true);
        }
    
        public void LoadLevelsToUI()
        {
            foreach (LevelInfo levelInfo in LevelManager.Instance.LevelInfos)
            {
                Instantiate(levelPrefab, levelContentHolder).Initialize(levelInfo);
            }
        }
    }
}
