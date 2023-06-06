using System;
using System.Collections.Generic;

public class Item
{
    public enum ItemType
    {
        largeBin,
        mediumBin,
        smallBin,
    }

    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.largeBin:
                return 50;
            case ItemType.mediumBin:
                return 20;
            case ItemType.smallBin:
                return 10;
        }
    }
}
