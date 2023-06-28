using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoSingleton<GameplayUIManager>
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text movesLeftText;

    private void OnEnable()
    {
        EventManager.OnPlayerMove += OnPlayerMove;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerMove -= OnPlayerMove;
    }

    private void OnPlayerMove()
    {
        movesLeftText.text = SwapManager.Instance.MoveCount.ToString();
    }


    public void Initialize()
    {
        scoreText.text = "0";
        movesLeftText.text = GameState.SelectedLevelInfo.MoveCount.ToString();
    }
    
}
