using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    RESOURCE,
    TOOL
}
public class Item : MonoBehaviour
{
    private ItemType mItemType = ItemType.RESOURCE;
    public void UseItem()
    {
        switch (mItemType)
        {
            case ItemType.RESOURCE:
                //Do nothing
                break;
            case ItemType.TOOL:
                //TODO
                break;
            default:
                break;
        }
    }

}
