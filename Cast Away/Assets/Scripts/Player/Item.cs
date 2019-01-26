using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public enum ItemType
{
    RESOURCE,
    TOOL,
}

public enum ToolType
{
    AXE,
    SPEAR
}

public delegate void VoidDelegate();

[System.Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "Inventory/List", order = 1)]

public class Item : ScriptableObject
{
    public string objectName = "Item";
    [SerializeField]
    private ItemType mItemType = ItemType.RESOURCE;
    public ItemType ItemType { get { return mItemType; } }

    [SerializeField]
    private HarvestableResource mResourceType = HarvestableResource.WOOD;
    public HarvestableResource ResourceType { get { return mResourceType; } }


    [SerializeField]
    private HarvestableResource mSecondaryResource = HarvestableResource.LEAF;
    public HarvestableResource SecondaryResourceType { get { return mSecondaryResource; } }


    [SerializeField]
    private ToolType mToolType = ToolType.AXE;
    public ToolType ToolType { get { return mToolType; } }

    [SerializeField]
    private Sprite mSprite;
    public Sprite ItemSprite { get { return mSprite; }}

    public int itemAmount = 0;

    public event VoidDelegate ItemChangeEvent;
    //Copy ctor
    public Item(Item aOriginal)
    {
        this.objectName = aOriginal.objectName;
        this.mItemType = aOriginal.mItemType;
        this.mResourceType = aOriginal.mResourceType;
        this.mSprite = aOriginal.mSprite;
        this.itemAmount = aOriginal.itemAmount;
    }

    public void UseItem()
    {
        switch (mItemType)
        {
            case ItemType.RESOURCE:
                Debug.Log("Im a resource, try and do some crafting");
                //Do nothing
                break;
            case ItemType.TOOL:
                Debug.Log("I'm a tool do something");
                //TODO
                break;
            default:
                break;
        }
    }

    public void IncreaseQuantity(int aQtyIncrease)
    {
        itemAmount += aQtyIncrease;
        if (ItemChangeEvent != null)
        {
            ItemChangeEvent();
        }
    }

    public void DecreaseQuantity(int aQtyDecrease)
    {
        itemAmount -= aQtyDecrease;
        if (ItemChangeEvent != null)
        {
            ItemChangeEvent();
        }
    }
}

#if UNITY_EDITOR

public class MakeScriptableObject
{
    [MenuItem("Assets/Create/Item scriptable object")]
    public static void CreateMyAsset()
    {
        Item asset = ScriptableObject.CreateInstance<Item>();
        AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Item.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}


#endif