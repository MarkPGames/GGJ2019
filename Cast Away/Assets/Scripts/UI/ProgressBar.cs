using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private Slider mSlider;

    public Image ResourceImage;
    public void SetProgress(float ratio)
    {
        mSlider.value = ratio;
    }

    public void SetSprite(Sprite aSprite)
    {
        if (aSprite != null)
        {
            ResourceImage.sprite = aSprite;
        }
        else
        {
            ResourceImage.gameObject.SetActive(false);
        }
    }


}
