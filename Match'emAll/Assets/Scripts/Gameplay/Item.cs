using Managers.LevelScene;
using UnityEngine;

namespace Gameplay
{
    public enum ItemType
    {
        None,
        Red = 100,
        Green = 150,
        Blue = 200,
        Yellow = 250
    }
    
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
            LeanTween.scale(image.gameObject, Vector2.one * 3f, 0.5f).setEaseOutElastic();
        }
    }
}
