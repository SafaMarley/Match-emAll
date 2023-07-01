using System;
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
        [SerializeField] private RectTransform starImageTransform;

        [Header("Animation Speeds")]
        private const float OpenPanelSpeed = .1f;
        private const float ClosePanelSpeed = .1f;
        private const float CelebrationPanelDuration = 5f;
        private const float ButtonBounceDuration = .5f;
        private const float ButtonBounceDelay = .1f;

        private List<LevelInformationUI> _levelInformationUIs = new List<LevelInformationUI>();
        
        public void Initialize()
        {
            LeanTween.scale(levelsPanelsOpenButton.gameObject, Vector2.one, ButtonBounceDuration).setEaseOutBounce();

            levelsPanelsOpenButton.onClick.AddListener(delegate
            {
                DisplayPanel(levelsPanel, LevelPanelActivationOnComplete);
            });
            
            levelsPanelsExitButton.onClick.AddListener(delegate
            {
                HidePanel(levelsPanel, LevelPanelDeactivationOnComplete);
            });
            
            if (GameState.CurrentGameState == State.LevelFailed)
            {
                DisplayPanel(levelsPanel, LevelPanelActivationOnComplete);
            }
            if (GameState.CurrentGameState == State.LevelCompleted)
            {
                int tempFinishedLevelNumber = GameState.SelectedLevelInfo.LevelNumber;
                celebrationPanelHighScoreText.text = PlayerPrefManager.GetHighScore(tempFinishedLevelNumber).ToString();
                if (tempFinishedLevelNumber < LevelManager.Instance.LevelInfos.Count && !LevelManager.Instance.LevelInfos[tempFinishedLevelNumber].IsAccessible)
                {
                    Debug.Log("Open up the next lock");
                    DisplayPanel(celebrationPanel, CelebrationPanelActivationOnComplete);
                }
                else
                {
                    DisplayPanel(celebrationPanel, CelebrationPanelActivationOnComplete);
                }
            }
        }

        private void DisplayPanel(RectTransform transformToDisplay, Action chainAction, float delay = 0.0f)
        {
            transformToDisplay.gameObject.SetActive(true);
            LeanTween.scale(transformToDisplay, Vector2.one, OpenPanelSpeed).setOnComplete(chainAction).setDelay(delay);
        }
        
        private void HidePanel(RectTransform transformToDisplay, Action chainAction, float delay = 0.0f)
        {
            transformToDisplay.gameObject.SetActive(true);
            LeanTween.scale(transformToDisplay, Vector2.zero, ClosePanelSpeed).setOnComplete( () =>
            {
                transformToDisplay.gameObject.SetActive(false);
                chainAction();
            }).setDelay(delay);
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
                levelInformationUI.transform.localScale = Vector3.zero;
            }
        }
        
        private void CelebrationPanelActivationOnComplete()
        {
            LeanTween.scale(starImageTransform.gameObject, Vector2.one * 2.5f, 1f).setEase(LeanTweenType.easeOutElastic).setOnComplete(() => 
                LeanTween.scale(starImageTransform.gameObject, Vector2.one, 1f).setDelay(1f).setEase(LeanTweenType.easeInOutElastic));
            HidePanel(celebrationPanel, CelebrationPanelDeactivationOnComplete, CelebrationPanelDuration);
        }
        
        private void CelebrationPanelDeactivationOnComplete()
        {
            DisplayPanel(levelsPanel, LevelPanelActivationOnComplete);
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
