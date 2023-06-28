using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private SpriteRenderer image;
    private ItemType _itemType;
    public ItemType ItemType { get=> _itemType; }

    public void Initialize(ItemType itemType)
    {
        _itemType = itemType;
        image.sprite = SpriteProvider.Instance.getItemImage(_itemType);
    }
}
