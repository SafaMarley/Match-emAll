using UnityEngine;

public class SpriteProvider : MonoSingleton<SpriteProvider>
{
    [SerializeField] private Sprite itemRed;
    [SerializeField] private Sprite itemRedCheck;

    [SerializeField] private Sprite itemGreen;
    [SerializeField] private Sprite itemGreenCheck;

    [SerializeField] private Sprite itemBlue;
    [SerializeField] private Sprite itemBlueCheck;

    [SerializeField] private Sprite itemYellow;
    [SerializeField] private Sprite itemYellowCheck;

    public Sprite getItemImage(ItemType itemType)
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
}
