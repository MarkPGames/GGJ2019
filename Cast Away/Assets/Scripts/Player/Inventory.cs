using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public Item[] mItems;
    public int mItemIndex;

    public void ChangeItemIndex(int aNewINdex)
    {

    }

    public Item GetItem()
    {
        return mItems[mItemIndex];
    }

}
