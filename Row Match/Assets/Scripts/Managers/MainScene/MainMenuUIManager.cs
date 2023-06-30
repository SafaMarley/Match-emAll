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
        [SerializeField] private Button celebrationContinueButton;
    
        [Header("Menu Panels")]
        [SerializeField] private RectTransform levelsPanel;
        [SerializeField] private RectTransform celebrationPanel;

        [Header("LevelContentHolder")]
        [SerializeField] private RectTransform levelContentHolder;
    
        [Header("Prefabs")]
        [SerializeField] private LevelInformationUI levelPrefab;

        public void Initialize()
        {
            levelsButton.onClick.AddListener(DisplayLevelPanel);
            celebrationContinueButton.onClick.AddListener(HideCelebrationPanel);
            if (GameState.CurrentGameState != State.Launched)
            {
                DisplayLevelPanel();
            }
            if (GameState.CurrentGameState == State.LevelCompleted)
            {
                DisplayCelebrationPanel();
            }
        }

        private void DisplayLevelPanel()
        {
            levelsPanel.gameObject.SetActive(true);
        }

        private void DisplayCelebrationPanel()
        {
            celebrationPanel.gameObject.SetActive(true);
        }
        
        private void HideCelebrationPanel()
        {
            celebrationPanel.gameObject.SetActive(false);
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
