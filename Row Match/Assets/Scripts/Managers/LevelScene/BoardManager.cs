using System.Collections.Generic;
using System.Linq;
using Gameplay;
using Managers.Base;
using Statics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers.LevelScene
{
    public class BoardManager : MonoSingleton<BoardManager>
    {
        [SerializeField] private BoardCell boardCellPrefab;
        private BoardCell[,] _boardCells;
        private static LevelInfo _levelInfo;

        private void Start()
        {
            _levelInfo = GameState.SelectedLevelInfo;
            SwipeManager.Instance.Initialize(_levelInfo.MoveCount);
            GameplayUIManager.Instance.Initialize();

            BuildBoard();
            AssignCellNeighbours();
        }

        public BoardCell[] GetRow(int rowIndex)
        {
            return Enumerable.Range(0, _boardCells.GetLength(0)).Select(x => _boardCells[x, rowIndex]).ToArray();
        }

        private void BuildBoard()
        {
            _boardCells = new BoardCell[_levelInfo.GridWidth, _levelInfo.GridHeight];
            float gridOffsetX = _levelInfo.GridWidth / 4f - .25f;
            float gridOffsetY = _levelInfo.GridHeight / 4f - .25f;
            for (int i = 0; i < _levelInfo.GridHeight; i++)
            {
                for (int j = 0; j < _levelInfo.GridWidth; j++)
                {
                    ItemType itemType = ItemType.None;
                    switch (_levelInfo.GridContent[i * _levelInfo.GridWidth + j])
                    {
                        case 'r':
                            itemType = ItemType.Red;
                            break;
                        case 'g':
                            itemType = ItemType.Green;
                            break;
                        case 'b':
                            itemType = ItemType.Blue;
                            break;
                        case 'y':
                            itemType = ItemType.Yellow;
                            break;
                    }
                    _boardCells[j, i] = Instantiate(boardCellPrefab, new Vector3(j * .5f - gridOffsetX, i * .5f - gridOffsetY), Quaternion.identity, transform);
                    _boardCells[j, i].Initialize(j, i, itemType);
                }
            }
        }

        private void AssignCellNeighbours()
        {
            foreach (BoardCell boardCell in _boardCells)
            {
                if (boardCell.coordinateX != _levelInfo.GridWidth - 1)
                {
                    boardCell.AssignNeighbourCells(NeighbourCellDirection.Right, _boardCells[boardCell.coordinateX + 1, boardCell.coordinateY]);
                }
                if (boardCell.coordinateX != 0)
                {
                    boardCell.AssignNeighbourCells(NeighbourCellDirection.Left, _boardCells[boardCell.coordinateX - 1, boardCell.coordinateY]);
                }
                if (boardCell.coordinateY != _levelInfo.GridHeight - 1)
                {
                    boardCell.AssignNeighbourCells(NeighbourCellDirection.Up, _boardCells[boardCell.coordinateX, boardCell.coordinateY + 1]);
                }
                if (boardCell.coordinateY != 0)
                {
                    boardCell.AssignNeighbourCells(NeighbourCellDirection.Down, _boardCells[boardCell.coordinateX, boardCell.coordinateY - 1]);
                }
            }
        }

        public void EndLevel()
        {
            if (MatchManager.Instance.Score > PlayerPrefManager.GetHighScore(GameState.SelectedLevelInfo.LevelNumber))
            {
                PlayerPrefManager.SetHighScore(GameState.SelectedLevelInfo.LevelNumber, MatchManager.Instance.Score);
                GameState.CurrentGameState = State.LevelCompleted;
            }
            else
            {
                GameState.CurrentGameState = State.LevelFailed;
            }
            
            SceneManager.LoadScene("Scenes/MainScene");
        }

        public void CheckIfBoardMatchable()
        {
            Dictionary<ItemType, int> matchableCellData = new Dictionary<ItemType, int>()
            {
                {ItemType.Red, 0},
                {ItemType.Green, 0},
                {ItemType.Blue, 0},
                {ItemType.Yellow, 0}
            };
            
            for (int i = 0; i < _levelInfo.GridHeight; i++)
            {
                for (int j = 0; j < _levelInfo.GridWidth; j++)
                {
                    if (!GetRow(i)[j].isAvailable)
                    {
                        foreach (var key in matchableCellData.Keys.ToList())
                        {
                            matchableCellData[key] = 0;
                        }
                    }
                    else
                    {
                        if (++matchableCellData[_boardCells[j, i].ItemInside.ItemType] == _levelInfo.GridWidth)
                        {
                            Debug.Log(_boardCells[j, i].ItemInside.ItemType);
                            return;
                        }
                    }
                }
            }
            EndLevel();
        }
    }
}
