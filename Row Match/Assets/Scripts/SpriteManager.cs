using UnityEngine;

public class SpriteManager : MonoSingleton<SpriteManager>
{
    [SerializeField] private Sprite itemRed;
    [SerializeField] private Sprite itemRedCheck;

    [SerializeField] private Sprite itemGreen;
    [SerializeField] private Sprite itemGreenCheck;

    [SerializeField] private Sprite itemBlue;
    [SerializeField] private Sprite itemBlueCheck;

    [SerializeField] private Sprite itemYellow;
    [SerializeField] private Sprite itemYellowCheck;

    public Sprite GetItemImage(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Red:
                return itemRed;
            case ItemType.Green:
                return itemGreen;
            case ItemType.Blue:
                return itemBlue;
            case ItemType.Yellow:
                return itemYellow;
        }
        return null;
    }
    
    public Sprite GetItemCheckImage(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Red:
                return itemRedCheck;
            case ItemType.Green:
                return itemGreenCheck;
            case ItemType.Blue:
                return itemBlueCheck;
            case ItemType.Yellow:
                return itemYellowCheck;
        }
        return null;
    }
}
