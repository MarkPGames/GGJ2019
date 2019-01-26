using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingResources : MonoBehaviour
{
    [SerializeField]
    private CraftingResourceIndicator mIndicatorPrefab;

    private GameObject followingObject;

    private List<CraftingResourceIndicator> mIndicators = new List<CraftingResourceIndicator>();
    public CraftingResourceIndicator CreateResourceRequirement(Item aResourceType, int craftingAmount, ref CraftingRequirements trackingRequirements)
    {
        CraftingResourceIndicator indicator = Instantiate(mIndicatorPrefab, this.transform);
        indicator.SetCraftingAmount(craftingAmount);
        indicator.SetResourceImage(aResourceType.ItemSprite);
        mIndicators.Add(indicator);
        return indicator;
    }

    public CraftingResources GetResourceIndicatorContainerInstance(GameObject followObject)
    {
        CraftingResources instance = Instantiate(this, this.transform.parent);
        followingObject = followObject;
        //instance.transform.position = Camera.main.WorldToScreenPoint(followingObject.transform.position) + Vector3.up * 100;
        return instance;
    }


    public void UpdateLocation(int requirementsRemaining, GameObject followObject)
    {
        if (!followingObject)
        {
            followingObject = followObject;
        }
        
        this.transform.position = Camera.main.WorldToScreenPoint(followingObject.transform.position) + Vector3.up * (30 * requirementsRemaining);

    }



}
