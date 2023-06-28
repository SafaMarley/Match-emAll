using System.Collections.Generic;
using Gameplay;
using Managers.Base;

namespace Managers.LevelScene
{
    public class MatchManager : MonoSingleton<MatchManager>
    {
        private readonly Dictionary<ItemType, int> _itemTypePoints = new Dictionary<ItemType, int>()
        {
            {ItemType.Red, 100},
            {ItemType.Green, 150},
            {ItemType.Blue, 200},
            {ItemType.Yellow, 250}
        };
    
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
                _score += _itemTypePoints[itemType];
                EventManager.OnPlayerScore.Invoke();
                boardCell.Deactivate();
            }
        }
    }
}
