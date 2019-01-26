using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState
{
    NORMAL,
    HARVESTING,
    SLEEPING,
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_fMovementSpeed;

    private Rigidbody2D _rigidBody;

    [SerializeField]
    private Inventory mInventory;

    private PlayerState mState = PlayerState.NORMAL;
    private IEnumerator runningRoutine = null;

    [SerializeField]
    private float raycastRange = 10;
    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {
        //Initiate any default items

        mInventory.Init();
        for (int i = 0; i < mInventory.m_Items.Count; i++)
        {
            UIManager.Instance.HotBar.SetItem(i, mInventory.m_Items[i]);
        }
        mInventory.SetItemIdx(0);
    }

    // Update is called once per frame
    void Update()
    {
        switch (mState)
        {
            case PlayerState.NORMAL:
                {
                    NormalState();
                }
                break;
            case PlayerState.HARVESTING:
                break;
            case PlayerState.SLEEPING:
                break;
        }
    }

    public void NormalState()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _rigidBody.velocity = input * m_fMovementSpeed;
        _rigidBody.angularVelocity = 0;
        if (input.magnitude > 0)
        {
            float radianToDegrees = Mathf.Atan2(-input.x, input.y) * 180 / Mathf.PI;
            this.transform.rotation = Quaternion.AngleAxis(radianToDegrees, Vector3.forward);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(this.transform.position, this.transform.up);
            RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction, raycastRange, 1 << 8);
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 100);
            if (hit2d)
            {
                BaseInteractable interactable;
                if ((interactable = hit2d.transform.gameObject.GetComponent<BaseInteractable>()))
                {
                    interactable.Interact();
                    if (interactable is ResourceInteractable)
                    {
                        ChangeState(PlayerState.HARVESTING, interactable);
                    }
                    else if(interactable is CraftableInteractable)
                    {
                        CraftableInteractable craftable = (interactable as CraftableInteractable);
                        craftable.Interact(mInventory.GetCurrentItem());

                        Item craftedItem = null;
                        if (craftable.isCompleted(out craftedItem))
                        {
                            if (mInventory.ContainsItem(craftable.itemScriptableObject))
                            {
                                for (int i = 0; i < mInventory.m_Items.Count; i++)
                                {
                                    if (mInventory.m_Items[i].ItemType == ItemType.TOOL && mInventory.m_Items[i].ToolType == craftedItem.ToolType)
                                    {
                                        mInventory.m_Items[i].IncreaseQuantity(1);
                                    }
                                }
                            }
                            else
                            {
                                //Create a new item
                                craftedItem.IncreaseQuantity(1);
                                mInventory.AddNewItem(craftedItem);
                            }
                        }
                    }
                    //else if (interactable is CraftableInteractable)
                    //{
                    //    //TODO
                    //}
                }
            }
        }

        for (int i = 0; i < mInventory.m_Items.Count; i++)
        {
            KeyCode potentialKeyPress = (KeyCode)Enum.Parse(typeof(KeyCode), string.Format("Alpha{0}", i + 1));
            if (Input.GetKeyDown(potentialKeyPress))
            {
                mInventory.SetItemIdx(i);
            }
        }
    }

    public void HarvestingState(BaseInteractable aHarvestingResource)
    {
        if (aHarvestingResource is ResourceInteractable)
        {
            ResourceInteractable resource = (ResourceInteractable)aHarvestingResource;
            runningRoutine = HarvestingRoutine(resource);
            StartCoroutine(runningRoutine);
        }
        else
        {
            ChangeState(PlayerState.NORMAL);
        }
    }

    private IEnumerator HarvestingRoutine(BaseInteractable aHarvestingResource)
    {
        float timer = 0;
        this._rigidBody.velocity = Vector3.zero;
        Debug.Log("HarvestingRoutine");
        ResourceInteractable resource = (ResourceInteractable)aHarvestingResource;
        float harvestingTime = resource.HarvestingTime;
        ProgressBar bar = UIManager.Instance.GetProgressBar(resource.gameObject);
        while (timer <= harvestingTime)
        {
            bar.SetProgress(timer / harvestingTime);
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(bar.gameObject);
        if (mInventory.ContainsItem(resource.itemScriptableObject))
        {
            for (int i = 0; i < mInventory.m_Items.Count; i++)
            {
                if (mInventory.m_Items[i].ItemType == ItemType.RESOURCE && mInventory.m_Items[i].ResourceType == resource.ResourceType)
                {
                    mInventory.m_Items[i].IncreaseQuantity(resource.Interact(mInventory.GetCurrentItem()));
                }
            }
        }
        else
        {
            //Create a new item
            Item newItem = Instantiate(resource.itemScriptableObject);
            newItem.IncreaseQuantity(resource.Interact(mInventory.GetCurrentItem()));
            mInventory.AddNewItem(newItem);
        }
        //Do the harvesting things

        ChangeState(PlayerState.NORMAL);
        yield return null;
    }

    private void ChangeState(PlayerState newState, BaseInteractable interactingWith = null)
    {
        switch (newState)
        {
            case PlayerState.NORMAL:
                break;
            case PlayerState.HARVESTING:
                HarvestingState(interactingWith);
                break;
            case PlayerState.SLEEPING:
                break;
            default:
                break;
        }
        mState = newState;
    }
}
