using Managers.Base;
using Statics;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.LevelScene
{
    public class GameplayUIManager : MonoSingleton<GameplayUIManager>
    {
        [Header("GUI Text Boxes")]
        [SerializeField] private RectTransform highScoreTextBox;
        [SerializeField] private RectTransform scoreTextBox;
        [SerializeField] private RectTransform movesLeftTextBox;
        [SerializeField] private RectTransform gameOverTextBox;
        
        [Header("GUI Texts")]
        [SerializeField] private Text highScoreText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text movesLeftText;
        [SerializeField] private Text gameOverText;
        
        private void OnEnable()
        {
            EventManager.OnPlayerMove += OnPlayerMove;
            EventManager.OnPlayerScore += OnPlayerScore;
        }
    
        private void OnDisable()
        {
            EventManager.OnPlayerMove -= OnPlayerMove;
            EventManager.OnPlayerScore -= OnPlayerScore;
        }
    
        private void OnPlayerMove()
        {
            movesLeftText.text = SwipeManager.Instance.MoveCount.ToString();
        }

        private void OnPlayerScore()
        {
            scoreText.text = MatchManager.Instance.Score.ToString();
        }

        public void Initialize()
        {
            LevelInfo tempLevelInfo = GameState.SelectedLevelInfo;
            movesLeftText.text = tempLevelInfo.MoveCount.ToString();
            highScoreText.text = PlayerPrefManager.GetHighScore(tempLevelInfo.LevelNumber).ToString();
            scoreText.text = "0";

            TweenController.DisplayInGameGUI(scoreTextBox.gameObject, -250, true, null, LeanTweenType.easeOutBounce);
            TweenController.DisplayInGameGUI(movesLeftTextBox.gameObject, 250, true, null, LeanTweenType.easeOutBounce);
            TweenController.DisplayInGameGUI(highScoreTextBox.gameObject, -700, false, null, LeanTweenType.easeOutBounce);
        }
        
        public void DisplayLevelEndUI(string endReason)
        {
            gameOverText.text = endReason;
            TweenController.DisplayInGameGUI(gameOverTextBox.gameObject, 0, true, null, LeanTweenType.easeOutBounce);
            TweenController.DisplayInGameGUI(gameOverTextBox.gameObject, 1000, true, () => BoardManager.Instance.EndLevel(), LeanTweenType.easeOutExpo, 2f);
        }
    }
}
