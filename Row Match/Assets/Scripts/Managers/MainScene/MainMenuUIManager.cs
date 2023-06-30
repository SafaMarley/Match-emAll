using System.Collections.Generic;
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

        [Header("Required Components")]
        [SerializeField] private ScrollRect levelContentHoldersScrollRect;
        [SerializeField] private Text celebrationPanelHighScoreText;

        [Header("Animation Speeds")]
        private const float OpenPanelSpeed = .1f;
        private const float ClosePanelSpeed = .1f;
        private const float CelebrationPanelDuration = 5f;
        private const float ButtonBounceDuration = .5f;

        private List<LevelInformationUI> _levelInformationUIs = new List<LevelInformationUI>();
        
        public void Initialize()
        {
            //levelsPanelsOpenButton.onClick.AddListener(delegate { DisplayPanel(levelsPanel, LevelButtonActionOnComplete); } );
            //levelsPanelsExitButton.onClick.AddListener( delegate { HidePanel(levelsPanel); });

            LeanTween.scale(levelsPanelsOpenButton.gameObject, Vector2.one, ButtonBounceDuration).setEaseOutBounce();
            
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
            LeanTween.scale(levelsPanel, Vector2.one, OpenPanelSpeed)
                .setOnComplete(LevelsButtonActionOnComplete);
        }

        private void HideLevelsPanel()
        {
            LeanTween.scale(levelsPanel, Vector2.zero, ClosePanelSpeed).setOnComplete(() => levelsPanel.gameObject.SetActive(false));
            foreach (LevelInformationUI levelInformationUI in _levelInformationUIs)
            {
                levelInformationUI.transform.localScale = Vector3.zero;
            }
        }
        
        private void DisplayCelebrationPanel()
        {
            celebrationPanel.gameObject.SetActive(true);
            LeanTween.scale(celebrationPanel, Vector2.one, OpenPanelSpeed)
                .setOnComplete(HideCelebrationPanel);
        }
        
        private void HideCelebrationPanel()
        {
            LeanTween.scale(celebrationPanel, Vector2.zero, ClosePanelSpeed).setDelay(CelebrationPanelDuration).setOnComplete(() =>
            {
                celebrationPanel.gameObject.SetActive(false);
                DisplayLevelsPanel();
            });
        }

        private void LevelsButtonActionOnComplete()
        {
            levelContentHoldersScrollRect.normalizedPosition = new Vector2(0, 1);
            foreach (LevelInformationUI levelInformationUI in _levelInformationUIs)
            {
                LeanTween.scale(levelInformationUI.gameObject, Vector2.one, ButtonBounceDuration).setEaseOutBounce();
            }
        }
    
        public void LoadLevelsToUI()
        {
            foreach (LevelInfo levelInfo in LevelManager.Instance.LevelInfos)
            {
                LevelInformationUI levelInformationUIInstance = Instantiate(levelPrefab, levelContentHolder);
                _levelInformationUIs.Add(levelInformationUIInstance);
                levelInformationUIInstance.Initialize(levelInfo);
            }
        }

        public void OnLevelButtonClicked()
        {
            LeanTween.scale(levelsPanel.gameObject, Vector2.zero, ClosePanelSpeed).setOnComplete(() => GameManager.Instance.OnPlayButtonClicked());
        }
    }
}
