using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    NORMAL,
    HARVESTING,
    FAINT,
    REPLENISHING,
    DISABLED,
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_fMovementSpeed;

    private Rigidbody2D _rigidBody;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Inventory mInventory;

    [SerializeField]
    private GameObject arrowObject;

    private PlayerState mState = PlayerState.NORMAL;
    public PlayerState CurrentState { get { return mState; } }
    private IEnumerator runningRoutine = null;

    PlayerResources mResources;
    [SerializeField]
    private float raycastRange = 10;

    [SerializeField]
    private float timeToDepleteResources = 5f;
    private float timer = 0;
    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
        mResources = this.GetComponent<PlayerResources>();
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
        mResources.DepleteResource(PlayerResoureType.THIRST);
        mResources.DepleteResource(PlayerResoureType.HUNGER);


        switch (mState)
        {
            case PlayerState.NORMAL:
                {
                    _rigidBody.isKinematic = false;
                    NormalState();
                }
                break;
            case PlayerState.DISABLED:
                {
                    _rigidBody.isKinematic = true;
                    _animator.SetBool("Moving", false);
                }
                break;
            default:
                _animator.SetBool("Moving", false);
                _rigidBody.isKinematic = false;
                break;
        }
    }
    Quaternion rotation;
    public void NormalState()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _rigidBody.velocity = input * m_fMovementSpeed;
        _rigidBody.angularVelocity = 0;
        if (input.magnitude > 0)
        {
            float radianToDegrees = Mathf.Atan2(-input.x, input.y) * 180 / Mathf.PI;
            float xAngle = Mathf.Atan2(0, input.x) * 180 / Mathf.PI;

            rotation = Quaternion.AngleAxis(radianToDegrees, Vector3.forward);

            float tempRadian = Mathf.Atan2(input.x, -input.y) * 180 / Mathf.PI;
            Quaternion temp = Quaternion.AngleAxis(tempRadian, Vector3.forward);
            arrowObject.transform.rotation = temp;
            this.transform.rotation = Quaternion.AngleAxis(xAngle, Vector3.up);
            _animator.SetBool("Moving", true);
        }
        else
        {
            _animator.SetBool("Moving", false);

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(this.transform.position, rotation * Vector3.up);
            RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction, raycastRange, 1 << 8);

            Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 100);
            if (hit2d)
            {
                BaseInteractable interactable;
                if ((interactable = hit2d.transform.gameObject.GetComponent<BaseInteractable>()))
                {
                    if (interactable is ResourceInteractable)
                    {
                        ResourceInteractable resource = (interactable as ResourceInteractable);
                        if (resource.ToolRequired)
                        {
                            if (mInventory.GetCurrentItem() != null)
                            {
                                if (mInventory.GetCurrentItem().ItemType == ItemType.TOOL && mInventory.GetCurrentItem().ToolType == resource.toolTypeRequired)
                                {
                                    ChangeState(PlayerState.HARVESTING, Vector3.zero, interactable);
                                }
                            }
                        }
                        else
                        {
                            ChangeState(PlayerState.HARVESTING, Vector3.zero, interactable);

                        }
                    }
                    else if (interactable is CraftableInteractable)
                    {
                        if (mInventory.GetCurrentItem())
                        {
                            CraftableInteractable craftable = (interactable as CraftableInteractable);
                            craftable.Interact(mInventory.GetCurrentItem());

                            Item craftedItem = null;
                            if (craftable.isCompleted(out craftedItem))
                            {
                                if (craftable.itemScriptableObject)
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
                                mResources.DepleteResource(PlayerResoureType.STAMINA);
                                if (craftable.DestroyOnceComplete)
                                {
                                    Destroy(craftable.gameObject);
                                }
                            }
                        }
                    }
                    else if (interactable is PlayerResourceInteractable)
                    {
                        PlayerResourceInteractable playerResource = (interactable as PlayerResourceInteractable);
                        if (playerResource.waitForCraftable)
                        {
                            if (playerResource.isReady())
                            {
                                ChangeState(PlayerState.REPLENISHING, hit2d.point, playerResource);
                            }
                        }
                        else
                        {
                            ChangeState(PlayerState.REPLENISHING, hit2d.point, playerResource);
                        }
                    }
                    else
                    {
                        interactable.Interact();
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
            ChangeState(PlayerState.NORMAL, Vector3.zero);
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

        if (resource.secondaryHarvest)
        {

            if (mInventory.ContainsItem(resource.secondaryHarvestItem))
            {
                for (int i = 0; i < mInventory.m_Items.Count; i++)
                {
                    if (mInventory.m_Items[i].ItemType == ItemType.RESOURCE && mInventory.m_Items[i].ResourceType == resource.SecondaryResourceType)
                    {
                        mInventory.m_Items[i].IncreaseQuantity(resource.Interact(mInventory.GetCurrentItem()));
                    }
                }
            }
            else
            {
                //Create a new item
                Item newItem = Instantiate(resource.secondaryHarvestItem);
                newItem.IncreaseQuantity(resource.Interact(mInventory.GetCurrentItem()));
                mInventory.AddNewItem(newItem);
            }
        }

        resource.ItemHarvested();
        if (resource.limitedResource)
        {
            if (resource.DepletedResource)
            {
                Destroy(resource.gameObject);
            }
        }
        mResources.DepleteResource(PlayerResoureType.STAMINA, true);
        //Do the harvesting things

        ChangeState(PlayerState.NORMAL, Vector3.zero);
        yield return null;
    }

    private void ReplenishingState(BaseInteractable interactable, Vector3 rayHitPoint)
    {
        if (interactable is PlayerResourceInteractable)
        {
            PlayerResourceInteractable resource = (PlayerResourceInteractable)interactable;
            if (resource.ReplenishType == PlayerResoureType.STAMINA)
            {
                mResources.DepleteResource(PlayerResoureType.HUNGER, true);
                mResources.DepleteResource(PlayerResoureType.THIRST, true);
            }
            runningRoutine = ResourceReplenishRoutine(resource, rayHitPoint);
            StartCoroutine(runningRoutine);
        }
        else
        {
            ChangeState(PlayerState.NORMAL, Vector3.zero);
        }
    }
    private IEnumerator ResourceReplenishRoutine(PlayerResourceInteractable interactable, Vector3 rayHitPoint)
    {
        this._rigidBody.velocity = Vector2.zero;

        yield return null;
        float timer = 0;
        ProgressBar bar = null;
        if (!interactable.blackScreen)
        {
            bar = UIManager.Instance.GetProgressBar(rayHitPoint);
            bar.SetSprite(interactable.ResourceSprite);
        }
        while (timer <= interactable.m_fReplenishTime)
        {
            this._rigidBody.velocity = Vector2.zero;
            timer += Time.deltaTime;
            if (interactable.blackScreen)
            {
                UIManager.Instance.SetBlackoutProgress(timer / interactable.m_fReplenishTime);
            }
            else
            {
                bar.SetProgress(timer / interactable.m_fReplenishTime);
            }
            yield return null;
        }


        if (interactable.blackScreen)
        {
            while (timer >= 0)
            {
                this._rigidBody.velocity = Vector2.zero;
                timer -= Time.deltaTime;
                UIManager.Instance.SetBlackoutProgress(timer / interactable.m_fReplenishTime);
                yield return null;
            }
        }
        if (bar != null)
        {
            Destroy(bar.gameObject);
        }
        mResources.ReplenishResource(interactable.ReplenishType, interactable.replenishRate);
        ChangeState(PlayerState.NORMAL, Vector3.zero);
    }

    private void FaintState()
    {
        runningRoutine = FaintRoutine();
        StartCoroutine(runningRoutine);
    }

    private IEnumerator FaintRoutine()
    {
        this._rigidBody.velocity = Vector2.zero;

        yield return null;
        float timer = 0;
        while (timer <= 2)
        {
            this._rigidBody.velocity = Vector2.zero;
            timer += Time.deltaTime;

            UIManager.Instance.SetBlackoutProgress(timer / 2);

            yield return null;
        }
        if (!mResources.GameOver)
        {
            while (timer >= 0)
            {
                this._rigidBody.velocity = Vector2.zero;
                timer -= Time.deltaTime;
                UIManager.Instance.SetBlackoutProgress(timer / 2);
                yield return null;
            }
            mResources.OnFainted();
            mResources.ReplenishResource(PlayerResoureType.STAMINA, 5);
            ChangeState(PlayerState.NORMAL, Vector3.zero);
        }
        else
        {
            UIManager.Instance.ShowGameOver();
            yield return null;
            timer = 0;
            while (timer <= 3)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    public void ChangeState(PlayerState newState, Vector3 raycastHitPoint, BaseInteractable interactingWith = null)
    {
        switch (newState)
        {
            case PlayerState.NORMAL:
                break;
            case PlayerState.HARVESTING:
                HarvestingState(interactingWith);
                break;
            case PlayerState.FAINT:
                FaintState();
                break;
            case PlayerState.REPLENISHING:
                ReplenishingState(interactingWith, raycastHitPoint);
                break;
            default:
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.angularVelocity = 0;
                _rigidBody.isKinematic = true;
                break;
        }
        mState = newState;
    }
}
