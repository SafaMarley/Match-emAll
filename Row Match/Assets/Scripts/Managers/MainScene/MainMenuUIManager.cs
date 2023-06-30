using System;
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
        [SerializeField] private Button levelsPanelsOpenButton;
        [SerializeField] private Button levelsPanelsExitButton;
    
        [Header("Menu Panels")]
        [SerializeField] private RectTransform levelsPanel;
        [SerializeField] private RectTransform celebrationPanel;

        [Header("LevelContentHolder")]
        [SerializeField] private RectTransform levelContentHolder;
    
        [Header("Prefabs")]
        [SerializeField] private LevelInformationUI levelPrefab;

        [Header("Needed Components")]
        [SerializeField] private ScrollRect levelContentHoldersScrollRect;
        [SerializeField] private Text celebrationPanelHighScoreText;
        
        public void Initialize()
        {
            //levelsPanelsOpenButton.onClick.AddListener(delegate { DisplayPanel(levelsPanel, LevelButtonActionOnComplete); } );
            //levelsPanelsExitButton.onClick.AddListener( delegate { HidePanel(levelsPanel); });

            levelsPanelsOpenButton.onClick.AddListener(DisplayLevelsPanel);
            levelsPanelsExitButton.onClick.AddListener(HideLevelsPanel);
            
            if (GameState.CurrentGameState == State.LevelFailed)
            {
                DisplayLevelsPanel();
            }
            if (GameState.CurrentGameState == State.LevelCompleted)
            {
                int tempFinishedLevelNumber = GameState.SelectedLevelInfo.LevelNumber;
                celebrationPanelHighScoreText.text = PlayerPrefManager.GetHighScore(tempFinishedLevelNumber).ToString();
                if (tempFinishedLevelNumber < LevelManager.Instance.LevelInfos.Count && !LevelManager.Instance.LevelInfos[tempFinishedLevelNumber].IsAccessible)
                {
                    Debug.Log("Open up the next lock");
                }
                else
                {
                    DisplayCelebrationPanel();
                }
            }
        }
        
        private void DisplayLevelsPanel()
        {
            levelsPanel.gameObject.SetActive(true);
            LeanTween.scale(levelsPanel, Vector2.one, 0.1f)
                .setOnComplete(LevelsButtonActionOnComplete);
        }

        private void HideLevelsPanel()
        {
            LeanTween.scale(levelsPanel, Vector2.zero, 0.1f).setOnComplete(() => levelsPanel.gameObject.SetActive(false));
        }
        
        private void DisplayCelebrationPanel()
        {
            celebrationPanel.gameObject.SetActive(true);
            LeanTween.scale(celebrationPanel, Vector2.one, 0.1f)
                .setOnComplete(HideCelebrationPanel);
        }
        
        private void HideCelebrationPanel()
        {
            LeanTween.scale(celebrationPanel, Vector2.zero, 0.1f).setDelay(10f).setOnComplete(() =>
            {
                celebrationPanel.gameObject.SetActive(false);
                DisplayLevelsPanel();
            });
        }
        
        /*
        private void CelebrationButtonActionOnComplete()
        {
            HidePanel(celebrationPanel, () => DisplayPanel(levelsPanel, LevelButtonActionOnComplete));
        }
        */

        private void LevelsButtonActionOnComplete()
        {
            levelContentHoldersScrollRect.normalizedPosition = new Vector2(0, 1);
        }

        private void DisplayPanel(RectTransform rectTransform, Action actionOnComplete)
        {
            rectTransform.gameObject.SetActive(true);
            LeanTween.scale(rectTransform, Vector2.one, 0.1f).setOnComplete(actionOnComplete);
        }
        
        // private void HidePanel(RectTransform rectTransform)
        // {
        //     LeanTween.scale(rectTransform, Vector2.zero, 0.1f);
        // }
        //
        // private void HidePanel(RectTransform rectTransform, Action actionOnComplete)
        // {
        //     LeanTween.scale(rectTransform, Vector3.zero, 0.1f).setDelay(3f).setOnComplete(() => rectTransform.gameObject.SetActive(false));
        //     DisplayPanel(levelsPanel, LevelButtonActionOnComplete);
        // }
    
        public void LoadLevelsToUI()
        {
            foreach (LevelInfo levelInfo in LevelManager.Instance.LevelInfos)
            {
                Instantiate(levelPrefab, levelContentHolder).Initialize(levelInfo);
            }
        }
    }
}
