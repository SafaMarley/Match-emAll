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
        
        [Header("GUI Texts")]
        [SerializeField] private Text highScoreText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text movesLeftText;

        private const float GUIMoveTimer = .5f;

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

            LeanTween.moveLocalX(scoreTextBox.gameObject, -250, GUIMoveTimer).setEaseOutBounce();
            LeanTween.moveLocalX(movesLeftTextBox.gameObject, 250, GUIMoveTimer).setEaseOutBounce();
            LeanTween.moveLocalY(highScoreTextBox.gameObject, -700, GUIMoveTimer).setEaseOutBounce();
        }
    }
}
