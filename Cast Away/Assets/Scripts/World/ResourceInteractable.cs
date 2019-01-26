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
    private HarvestableResource mSecondaryResource;
    public HarvestableResource SecondaryResource { get { return mSecondaryResource; } }
    [SerializeField]
    private int m_iHarvestAmount = 0;
    public int HarvestAmount { get { return m_iHarvestAmount; } }

    [SerializeField]
    private bool mbToolRequired = false;
    public bool ToolRequired { get { return mbToolRequired; } }

    public float HarvestingTime = 5f;

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


}
