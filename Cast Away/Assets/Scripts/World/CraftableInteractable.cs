using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CraftingRequirements
{
    public Item mResourceRequired;
    public int mResourceCountRequired;
    public CraftingResourceIndicator mIndicatorUI;

    [SerializeField]
    private int gatheredResources;
    public void AddResource(int aAmount)
    {
        gatheredResources += aAmount;
    }

    public int GetRemainingAmount()
    {
        return mResourceCountRequired - gatheredResources;
    }

    public bool IsCompleted()
    {
        if (gatheredResources >= mResourceCountRequired)
        {
            return true;
        }
        return false;
    }

    public void UpdateIndicator()
    {
        if (mIndicatorUI)
        {
            mIndicatorUI.SetCraftingAmount(GetRemainingAmount());
            if (GetRemainingAmount() <= 0)
            {
                Object.Destroy(mIndicatorUI.gameObject);
            }
        }
        Debug.Log("Remaining amount: " + GetRemainingAmount());
    }
}
public class CraftableInteractable : BaseInteractable
{
    public CraftingRequirements[] mRequirements;

    private CraftingResources ui_Resources;

    public bool InteractableCraft = false;
    public bool DestroyOnceComplete = true;
    private void Start()
    {
        ui_Resources = UIManager.Instance.CraftingRequirementsObject.GetResourceIndicatorContainerInstance(this.gameObject);
        UpdateRequirementLocation();
        for (int i = 0; i < mRequirements.Length; i++)
        {
            mRequirements[i].mIndicatorUI = ui_Resources.CreateResourceRequirement(mRequirements[i].mResourceRequired, mRequirements[i].mResourceCountRequired, ref mRequirements[i]);
        }
    }

    private void UpdateRequirementLocation()
    {
        int requirementsLeft = 0;
        for (int i = 0; i < mRequirements.Length; i++)
        {
            if (!mRequirements[i].IsCompleted())
            {
                requirementsLeft++;
            }
        }
        ui_Resources.UpdateLocation(requirementsLeft, this.gameObject);
    }

    private void Update()
    {
        UpdateRequirementLocation();
    }

    public override void Interact()
    {

    }

    public void Interact(Item aItemInteractedWith)
    {
        if (aItemInteractedWith.ItemType == ItemType.RESOURCE)
        {
            Debug.Log("Trying to craft");
            for (int i = 0; i < mRequirements.Length; i++)
            {
                if (mRequirements[i].mResourceRequired.ItemType == ItemType.RESOURCE && mRequirements[i].mResourceRequired.ResourceType == aItemInteractedWith.ResourceType)
                {
                    int remainingAmount = mRequirements[i].GetRemainingAmount();
                    if (aItemInteractedWith.itemAmount >= remainingAmount)
                    {
                        AddResource(remainingAmount, ref mRequirements[i]);
                        aItemInteractedWith.DecreaseQuantity(remainingAmount);
                    }
                    else
                    {
                        AddResource(aItemInteractedWith.itemAmount, ref mRequirements[i]);
                        aItemInteractedWith.DecreaseQuantity(aItemInteractedWith.itemAmount);
                    }
                    // AddResource(aItemInteractedWith.itemAmount)
                }
                mRequirements[i].UpdateIndicator();
                UpdateRequirementLocation();
            }
        }
    }

    public bool isCompleted(out Item item)
    {
        item = null;
        for (int i = 0; i < mRequirements.Length; i++)
        {
            if (!mRequirements[i].IsCompleted())
            {
                item = null;
                return false;
            }
        }
        if (!InteractableCraft)
        {
            Item itemInstnace = Instantiate(itemScriptableObject);
            item = itemInstnace;
        }
        this.GetComponent<Collider2D>().enabled = false;
        return true;
    }

    public void AddResource(int aAmount, ref CraftingRequirements craftingRequirement)
    {
        craftingRequirement.AddResource(aAmount);
        //TODO
    }

}
