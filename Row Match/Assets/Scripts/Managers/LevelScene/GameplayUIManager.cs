using Managers.Base;
using Statics;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.LevelScene
{
    public class GameplayUIManager : MonoSingleton<GameplayUIManager>
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Text movesLeftText;

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
            scoreText.text = "0";
            movesLeftText.text = GameState.SelectedLevelInfo.MoveCount.ToString();
        }
    }
}
