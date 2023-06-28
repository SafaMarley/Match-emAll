using System.Collections.Generic;
using UnityEngine;

public enum NeighbourCellDirection
{
    Right,
    Left,
    Up,
    Down
}

public class BoardCell : MonoBehaviour
{
    public bool isAvailable;
    public int coordinateX;
    public int coordinateY;

    private bool _isSelected;
    private Dictionary<NeighbourCellDirection, BoardCell> _neighbours = new Dictionary<NeighbourCellDirection, BoardCell>();
     
    private Item _itemInside;
    public Item ItemInside { get => _itemInside; }
    
    [SerializeField] private Item itemPrefab;

    public void Initialize(int x, int y, ItemType itemType)
    {
        isAvailable = true;
        coordinateX = x;
        coordinateY = y;
        gameObject.name = "boardCell: " + x + " : " + y;
        _itemInside = Instantiate(itemPrefab, transform);
        _itemInside.Initialize(itemType);
    }

    public void SetItemInside(Item itemInside)
    {
        _itemInside = itemInside;
        Transform itemInsideTransform = _itemInside.transform;
        itemInsideTransform.SetParent(transform);
        itemInsideTransform.localPosition = Vector3.zero;
    }

    public void AssignNeighbourCells(NeighbourCellDirection neighbourCellDirection, BoardCell boardCell)
    {
        _neighbours.Add(neighbourCellDirection, boardCell);
    }

    public BoardCell GetNeighbourCell(NeighbourCellDirection neighbourCellDirection)
    {
        return _neighbours.ContainsKey(neighbourCellDirection) ? _neighbours[neighbourCellDirection] : null;
    }

    public void Deactivate()
    {
        isAvailable = false;
        ItemInside.Deactivate();
    }
    
    private void OnMouseExit()
    {
        if (_isSelected)
        {
            SwapManager.Instance.ExecuteSwapAction(this, Input.mousePosition);
            _isSelected = false;
        }
    }

    private void OnMouseUp()
    {
        _isSelected = false;
    }

    private void OnMouseDown()
    {
        _isSelected = true;
    }
}
