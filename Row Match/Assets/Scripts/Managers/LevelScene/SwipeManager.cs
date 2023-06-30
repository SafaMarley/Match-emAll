using Gameplay;
using Managers.Base;
using UnityEngine;

namespace Managers.LevelScene
{
    public class SwipeManager : MonoSingleton<SwipeManager>
    {
        [SerializeField] private Camera mainCamera;
        private int _moveCount;
        public int MoveCount { get => _moveCount; }

        public void Initialize(int moveCount)
        {
            _moveCount = moveCount;
        }
        public void ExecuteSwapAction(BoardCell boardCell, Vector3 mouseExitScreenPosition)
        {
            if (!boardCell.isAvailable)
            {
                return;
            }
            Vector3 mouseExitWorldPosition = mainCamera.ScreenToWorldPoint(mouseExitScreenPosition);
            Vector3 boardCellWorldPosition = boardCell.transform.position;
            Vector2 mouseExitDirection = mouseExitWorldPosition - boardCellWorldPosition;

            BoardCell boardCellOnSwapDirection;

            if (Mathf.Abs(mouseExitDirection.x) > Mathf.Abs(mouseExitDirection.y))
            {
                if (mouseExitDirection.x > 0)
                {
                    boardCellOnSwapDirection = boardCell.GetNeighbourCell(NeighbourCellDirection.Right);
                }
                else
                {
                    boardCellOnSwapDirection = boardCell.GetNeighbourCell(NeighbourCellDirection.Left);
                }
            }
            else
            {
                if (mouseExitDirection.y > 0)
                {
                    boardCellOnSwapDirection = boardCell.GetNeighbourCell(NeighbourCellDirection.Up);
                }
                else
                {
                    boardCellOnSwapDirection = boardCell.GetNeighbourCell(NeighbourCellDirection.Down);
                }
            }

            if (!boardCellOnSwapDirection || 
                !boardCellOnSwapDirection.isAvailable || 
                boardCell.ItemInside.ItemType == boardCellOnSwapDirection.ItemInside.ItemType)
            {
                return;
            }
        
            SwapItems(boardCell, boardCellOnSwapDirection);
        }
    
        private void SwapItems(BoardCell cellToSwap1, BoardCell cellToSwap2)
        {
            _moveCount--;
            EventManager.OnPlayerMove.Invoke();
            Item swapItem = cellToSwap1.ItemInside;
            cellToSwap1.SetItemInside(cellToSwap2.ItemInside);
            cellToSwap2.SetItemInside(swapItem);
            if (MoveCount == 0)
            {
                BoardManager.Instance.EndLevel();
            }
        }
    }
}
