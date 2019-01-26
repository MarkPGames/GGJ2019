using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CraftingResourceIndicator : MonoBehaviour
{
    [SerializeField]
    private Image mImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI mText;

    public void SetCraftingAmount(int a_Amount)
    {
        mText.text = string.Format("x{0}", a_Amount);
    }

    public void SetResourceImage(Sprite aResourceImage)
    {
        mImage.sprite = aResourceImage;
    }

}
