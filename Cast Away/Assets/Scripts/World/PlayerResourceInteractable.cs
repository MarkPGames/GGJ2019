using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceInteractable : BaseInteractable
{
    [SerializeField]
    private PlayerResoureType mReplenishType = PlayerResoureType.STAMINA;
    public PlayerResoureType ReplenishType { get { return mReplenishType; } }

    public bool mbRequireTool;
    public ToolType toolRequired = ToolType.SPEAR;
    public float replenishRate;

    public float m_fReplenishTime;

    public bool blackScreen;

    public bool waitForCraftable = false;

    public CraftableInteractable craftingWait;

    private Collider2D mCollider;

    private void Start()
    {
        mCollider = this.GetComponent<Collider2D>();
    }
    public override void Interact(/*There should be a callback here, too late for that*/)
    {
        Debug.Log("What's the point of this again?");
        //throw new System.NotImplementedException();
    }

    private void Update()
    {
        mCollider.enabled = isReady();
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
}
