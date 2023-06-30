


using Gameplay;
using Managers.Base;

namespace Managers.LevelScene
{
    public class MatchManager : MonoSingleton<MatchManager>
    {
        private int _score;
        public int Score { get => _score; }

        public void CheckRowForMatch(int index)
        {
            ItemType itemType = ItemType.None;
            foreach (BoardCell boardCell in BoardManager.Instance.GetRow(index))
            {
                if (itemType == ItemType.None)
                {
                    itemType = boardCell.ItemInside.ItemType;
                }
                else if (boardCell.ItemInside.ItemType != itemType)
                {
                    return;
                }
            }

            foreach (BoardCell boardCell in BoardManager.Instance.GetRow(index))
            {
                _score += (int) itemType;
                EventManager.OnPlayerScore.Invoke();
                boardCell.Deactivate();
            }
            BoardManager.Instance.CheckIfBoardMatchable();
        }
    }
}
