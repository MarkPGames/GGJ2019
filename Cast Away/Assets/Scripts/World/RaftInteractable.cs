using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftInteractable : BaseInteractable
{
    public bool waitForCraftable = false;

    public CraftableInteractable craftingWait;

    private Collider2D mCollider;

    private void Start()
    {
        mCollider = this.GetComponent<Collider2D>();
    }
    public override void Interact(/*There should be a callback here, too late for that*/)
    {
        //throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (waitForCraftable)
        {
            mCollider.enabled = isReady();
        }
    }
    public bool isReady()
    {
        Item nullItem;
        if (waitForCraftable)
        {
            if (craftingWait.isCompleted(out nullItem))
            {
                return true;
            }
        }
        return false;
    }


    private IEnumerator travelToIslandRoutine()
    {
        yield return null;
    }
}
