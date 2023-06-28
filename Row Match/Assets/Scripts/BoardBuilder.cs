using UnityEngine;

public class BoardBuilder : MonoSingleton<BoardBuilder>
{
    [SerializeField] private BoardCell boardCellPrefab;
    private BoardCell[,] _boardCells;
    private static LevelInfo _levelInfo;

    private void Start()
    {
        _levelInfo = GameState.SelectedLevelInfo;
        SwapManager.Instance.Initialize(_levelInfo.MoveCount);
        GameplayUIManager.Instance.Initialize();

        BuildBoard();
        AssignCellNeighbours();
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
}
