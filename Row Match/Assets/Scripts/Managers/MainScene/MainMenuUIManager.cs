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

        public void Initialize()
        {
            levelsPanelsOpenButton.onClick.AddListener(delegate { DisplayPanel(levelsPanel, LevelButtonActionOnComplete); } );
            levelsPanelsExitButton.onClick.AddListener( delegate { HidePanel(levelsPanel); });
            if (GameState.CurrentGameState == State.LevelFailed)
            {
                DisplayPanel(levelsPanel, LevelButtonActionOnComplete);
            }
            if (GameState.CurrentGameState == State.LevelCompleted)
            {
                DisplayPanel(celebrationPanel, CelebrationButtonActionOnComplete);
            }
        }

        private void CelebrationButtonActionOnComplete()
        {
            HidePanel(celebrationPanel, () => DisplayPanel(levelsPanel, LevelButtonActionOnComplete));
        }

        private void LevelButtonActionOnComplete()
        {
            levelContentHoldersScrollRect.normalizedPosition = new Vector2(0, 1);
        }

        private void DisplayPanel(RectTransform rectTransform, Action actionOnComplete)
        {
            rectTransform.gameObject.SetActive(true);
            LeanTween.scale(rectTransform, Vector2.one, 0.1f).setOnComplete(actionOnComplete);
        }
        
        private void HidePanel(RectTransform rectTransform)
        {
            LeanTween.scale(rectTransform, Vector2.zero, 0.1f);
        }

        private void HidePanel(RectTransform rectTransform, Action actionOnComplete)
        {
            LeanTween.scale(rectTransform, Vector3.zero, 0.1f).setOnComplete(actionOnComplete).setOnComplete(() => rectTransform.gameObject.SetActive(false));
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
