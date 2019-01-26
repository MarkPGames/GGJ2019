using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private static UIManager mInstance;
    public static UIManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<UIManager>();
            }
            return mInstance;
        }
    }

    [SerializeField]
    private ProgressBar mProgressBar;

    [SerializeField]
    private HotbarUI mHotBar;
    public HotbarUI HotBar { get { return mHotBar; }}

    [SerializeField]
    private CraftingResources requirementsObject;
    public CraftingResources CraftingRequirementsObject { get { return requirementsObject; } }


    public ProgressBar GetProgressBar(GameObject aFollowObject)
    {
        ProgressBar createdBar = null;
        if (mProgressBar)
        {
            createdBar = Instantiate(mProgressBar, this.transform);
        }
        if (createdBar != null)
        {
            createdBar.transform.position = Camera.main.WorldToScreenPoint(aFollowObject.transform.position);
        }
        return createdBar;
    }
}
