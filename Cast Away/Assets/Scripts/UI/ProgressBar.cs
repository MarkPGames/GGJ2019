using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private Slider mSlider;

    public void SetProgress(float ratio)
    {
        mSlider.value = ratio;
    }
}
