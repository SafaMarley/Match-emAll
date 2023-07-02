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

        [Header("Prefabs")]
        [SerializeField] private LevelInformationUI levelPrefab;

        [Header("Required Components")]
        [SerializeField] private RectTransform levelContentHolder;
        [SerializeField] private ScrollRect levelContentHoldersScrollRect;
        [SerializeField] private Text celebrationPanelHighScoreText;
        [SerializeField] private RectTransform starImageTransform;
        

        private const float CelebrationPanelDuration = 5f;
        private const float ButtonBounceDuration = .5f;
        private const float ButtonBounceDelay = .1f;
        
        private List<LevelInformationUI> _levelInformationUIs = new List<LevelInformationUI>();

        public void Initialize()
        {
            TweenController.DisplayPanel(levelsPanelsOpenButton.gameObject, .5f);

            levelsPanelsOpenButton.onClick.AddListener(delegate
            {
                TweenController.HidePanel(levelsPanelsOpenButton.gameObject, () => TweenController.DisplayPanel(levelsPanel.gameObject, LevelPanelActivationOnComplete));
            });
            
            levelsPanelsExitButton.onClick.AddListener(delegate
            {
                TweenController.HidePanel(levelsPanel.gameObject, LevelPanelDeactivationOnComplete);
            });
            
            if (GameState.CurrentGameState == State.LevelFailed)
            {
                TweenController.DisplayPanel(levelsPanel.gameObject, LevelPanelActivationOnComplete);
            }
            
            if (GameState.CurrentGameState == State.LevelCompleted)
            {
                int tempFinishedLevelNumber = GameState.SelectedLevelInfo.LevelNumber;
                celebrationPanelHighScoreText.text = PlayerPrefManager.GetHighScore(tempFinishedLevelNumber).ToString();
                if (tempFinishedLevelNumber < LevelManager.Instance.LevelInfos.Count && !LevelManager.Instance.LevelInfos[tempFinishedLevelNumber].IsAccessible)
                {
                    LevelManager.Instance.LevelInfos[tempFinishedLevelNumber].ActivateLevel();
                    _levelInformationUIs[tempFinishedLevelNumber].ActivateSelf();
                    PlayerPrefManager.PlayerLevelUp();
                    TweenController.DisplayPanel(celebrationPanel.gameObject, CelebrationPanelActivationOnComplete);
                }
                else
                {
                    TweenController.DisplayPanel(celebrationPanel.gameObject, CelebrationPanelActivationOnComplete);
                }
            }
        }

        private void LevelPanelActivationOnComplete()
        {
            levelContentHoldersScrollRect.normalizedPosition = new Vector2(0, 1);
            for (int i = 0; i < _levelInformationUIs.Count; i++)
            {
                LeanTween.scale(_levelInformationUIs[i].gameObject, Vector2.one, ButtonBounceDuration).setEaseOutBounce().setDelay(i * ButtonBounceDelay);
            }
        }
        private void LevelPanelDeactivationOnComplete()
        {
            foreach (LevelInformationUI levelInformationUI in _levelInformationUIs)
            {
                LeanTween.cancel(levelInformationUI.gameObject);
                levelInformationUI.transform.localScale = Vector3.zero;
            }
            TweenController.DisplayPanel(levelsPanelsOpenButton.gameObject);
        }
        
        private void CelebrationPanelActivationOnComplete()
        {
            LeanTween.scale(starImageTransform.gameObject, Vector2.one * 2.5f, 1f).setEase(LeanTweenType.easeOutElastic).setOnComplete(() => 
                LeanTween.scale(starImageTransform.gameObject, Vector2.one, 1f).setDelay(1f).setEase(LeanTweenType.easeInOutElastic));
            TweenController.HidePanel(celebrationPanel.gameObject, CelebrationPanelDeactivationOnComplete, CelebrationPanelDuration);
        }
        
        private void CelebrationPanelDeactivationOnComplete()
        {
            TweenController.DisplayPanel(levelsPanel.gameObject, LevelPanelActivationOnComplete);
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
            TweenController.HidePanel(levelsPanel.gameObject, GameManager.Instance.OnPlayButtonClicked);
        }
    }
}
