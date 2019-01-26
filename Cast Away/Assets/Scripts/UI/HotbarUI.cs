using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarUI : MonoBehaviour
{
    [SerializeField]
    private List<HotbarSlot> mSlots;
    public List<HotbarSlot> Slots { get { return mSlots; } }

    private int mCurrentSelectedIndex;
    // Use this for initialization
    void Start()
    {
        mSlots = new List<HotbarSlot>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            mSlots.Add(this.transform.GetChild(i).GetComponent<HotbarSlot>());
        }
    }

    public void SetSelected(int aSelectedIndex)
    {
        if (aSelectedIndex <= 0)
        {
            aSelectedIndex = 0;
        }
        Debug.Log("Setting selecteditem " + aSelectedIndex);
        mSlots[mCurrentSelectedIndex].OnDeselect();
        mSlots[aSelectedIndex].SetSelected();
        mCurrentSelectedIndex = aSelectedIndex;
    }

    public void SetItem(int aSlotIndex, Item aItem)
    {
        if (mSlots[aSlotIndex] != null)
        {
            mSlots[aSlotIndex].SetItem(aItem);
        }
    }


}
