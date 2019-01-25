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
    [SerializeField]
    private int m_iHarvestAmount = 0;
    public int HarvestAmount { get { return m_iHarvestAmount; } }

    public override void Interact()
    {
        Debug.Log("IM A RESOURCE");
    }

}
