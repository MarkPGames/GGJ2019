using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public Item[] defaultItems;
    public List<Item> m_Items;

    public int mItemIndex = 0;

    public void Init()
    {
        for (int i = 0; i < defaultItems.Length; i++)
        {
            m_Items.Add(Object.Instantiate(defaultItems[i]));
            m_Items[i].ItemChangeEvent += OnItemChange;
        }
    }

    public bool ContainsItem(Item compare)
    {
        for (int i = 0; i < m_Items.Count; i++)
        {
            switch (compare.ItemType)
            {
                case ItemType.RESOURCE:
                    if (compare.ResourceType == m_Items[i].ResourceType && m_Items[i].ItemType == compare.ItemType)
                    {
                        return true;
                    }
                    break;
                case ItemType.TOOL:
                    if (compare.ToolType == m_Items[i].ToolType && m_Items[i].ItemType == compare.ItemType)
                    {
                        return true;
                    }
                    break;
            }
        }
        return false;
    }

    private void OnItemChange()
    {
        UpdateItems();
    }

    public void SetItemIdx(int aNewIdx)
    {
        if (aNewIdx <=0)
        {
            aNewIdx = 0;
        }
        mItemIndex = aNewIdx;
        UIManager.Instance.HotBar.SetSelected(mItemIndex);
    }

    public void IncrementInventory()
    {
        int tempIdx = ++mItemIndex;
        if (tempIdx >= m_Items.Count)
        {
            mItemIndex = 0;
        }

        SetItemIdx(mItemIndex);
    }

    public void DecrementInventory()
    {
        int tempIdx = --mItemIndex;
        if (tempIdx < 0)
        {
            tempIdx = m_Items.Count - 1;
        }
        SetItemIdx(mItemIndex);
    }

    public Item GetCurrentItem()
    {
        if (m_Items.Count > 0)
        {
            return m_Items[mItemIndex];
        }
        return null;
    }


public void UpdateItems()
{
    bool removedItem = false;
    for (int i = 0; i < m_Items.Count; i++)
    {
        if (m_Items[i].itemAmount <= 0)
        {
            removedItem = true;
            m_Items.Remove(m_Items[i]);
        }
    }
    for (int i = 0; i < m_Items.Count; i++)
    {
        UIManager.Instance.HotBar.SetItem(i, m_Items[i]);
    }
    for (int i = m_Items.Count; i < UIManager.Instance.HotBar.Slots.Count; i++)
    {
        UIManager.Instance.HotBar.Slots[i].ResetToNull();
    }
    if (removedItem)
    {
        SetItemIdx(m_Items.Count - 1);
    }
}

public void AddNewItem(Item aNewItem)
{
    m_Items.Add(aNewItem);
    aNewItem.ItemChangeEvent += OnItemChange;
    UpdateItems();
}
}
