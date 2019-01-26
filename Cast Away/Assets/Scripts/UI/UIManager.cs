using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    public HotbarUI HotBar { get { return mHotBar; } }

    [SerializeField]
    private CraftingResources requirementsObject;
    public CraftingResources CraftingRequirementsObject { get { return requirementsObject; } }

    [SerializeField]
    private Image blackout;

    [SerializeField]
    private TMPro.TextMeshProUGUI gameOverText;

    private void Start()
    {
        blackout.color = new Color(0, 0, 0, 0);
    }
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

    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }

    public void SetBlackoutProgress(float progress)
    {
        if (blackout)
        {
            blackout.color = new Color(0, 0, 0, progress);
        }
    }
}
