using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HarvestableResource
{
    WOOD,
    STONE,
    LEAF,
    REED,
    STICK
}

public class ResourceInteractable : BaseInteractable
{
    [SerializeField]
    private HarvestableResource mResource;
    public HarvestableResource ResourceType { get { return mResource; } }

    public bool secondaryHarvest = false;

    public Item secondaryHarvestItem;
    [SerializeField]
    private HarvestableResource mSecondaryResource;
    public HarvestableResource SecondaryResourceType { get { return mSecondaryResource; } }

    [SerializeField]
    private int m_iHarvestAmount = 0;
    public int HarvestAmount
    {
        get
        {
            if (limitedResource)
            {
                if (m_iHarvestAmount >= maxHarvest)
                {
                    return maxHarvest;
                }
                //return maxHarvest - amountHarvested;
            }
            return m_iHarvestAmount;
        }
    }

    [SerializeField]
    private bool mbToolRequired = false;
    public bool ToolRequired { get { return mbToolRequired; } }
    public ToolType toolTypeRequired = ToolType.AXE;

    public float HarvestingTime = 5f;

    public bool limitedResource;

    [SerializeField]
    private int amountHarvested = 0;
    [SerializeField]
    private int maxHarvest = 2;


    public bool DepletedResource
    {
        get
        {
            if (amountHarvested >= maxHarvest)
            {
                return true;
            }
            return false;
        }
    }

    public override void Interact()
    {

    }

    public int Interact(Item interactedWith)
    {
        if (mbToolRequired)
        {
            if (interactedWith.ItemType == ItemType.TOOL && mbToolRequired)
            {
                return HarvestAmount;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return HarvestAmount;
        }
    }

    public void ItemHarvested()
    {
        amountHarvested += m_iHarvestAmount;
    }

}
