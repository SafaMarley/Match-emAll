using Managers.LevelScene;
using Managers.MainScene;
using Statics;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class LevelInformationUI : MonoBehaviour
    {
        [SerializeField] private Text levelInfoText;
        [SerializeField] private Text highScoreText;
        [SerializeField] private Button playButton;
        [SerializeField] private Image playButtonLock;
        [SerializeField] private Text playButtonText;
        private LevelInfo _levelInfo;
    
        public void Initialize(LevelInfo levelInfo)
        {
            playButton.onClick.AddListener(OnLevelButtonClick);
            _levelInfo = levelInfo;
            levelInfoText.text = "Level " + _levelInfo.LevelNumber + " - " + _levelInfo.MoveCount + " Moves";
            if (levelInfo.IsAccessible)
            {
                ActivateSelf();
            }
            else
            {
                DeactivateSelf();
            }
        }

        private void OnLevelButtonClick()
        {
            GameState.SelectedLevelInfo = _levelInfo;
            MainMenuUIManager.Instance.OnLevelButtonClicked();
        }

        public void ActivateSelf()
        {
            highScoreText.text = "High Score: " + PlayerPrefManager.GetHighScore(_levelInfo.LevelNumber);
            playButton.interactable = true;
            playButtonText.gameObject.SetActive(true);
            playButtonLock.gameObject.SetActive(false);
        }

        private void DeactivateSelf()
        {
            highScoreText.text = "Locked Level";
            playButton.interactable = false;
            playButtonText.gameObject.SetActive(false);
            playButtonLock.gameObject.SetActive(true);
        }
    }
}
