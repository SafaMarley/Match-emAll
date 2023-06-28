using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private SpriteRenderer image;
    private ItemType _itemType;
    public ItemType ItemType { get=> _itemType; }

    public void Initialize(ItemType itemType)
    {
        _itemType = itemType;
        image.sprite = SpriteManager.Instance.GetItemImage(_itemType);
    }

    public void Deactivate()
    {
        image.sprite = SpriteManager.Instance.GetItemCheckImage(_itemType);
    }
}
