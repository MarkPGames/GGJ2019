using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HotbarSlot : MonoBehaviour
{
    [SerializeField]
    private Image itemSprite;
    [SerializeField]
    private Text mQuantityText;
    private Image mSelectionSprite;

    private Sprite defaultSprite;
    public int mIdx;
    private void Start()
    {
        mSelectionSprite = this.GetComponent<Image>();
        defaultSprite = itemSprite.sprite;
    }
    public void SetItem(Item aItem)
    {
        itemSprite.sprite = aItem.ItemSprite;
        itemSprite.color = Color.white;
        mQuantityText.text = aItem.itemAmount.ToString();
    }

    public void SetSelected()
    {
        mSelectionSprite.color = Colors.Orange;
    }
    public void OnDeselect()
    {
        mSelectionSprite.color = Color.white;
    }

    public void ResetToNull()
    {
        mSelectionSprite.color = Color.white;
        itemSprite.color = Color.black;
        itemSprite.sprite = defaultSprite;
        mQuantityText.text = "0";
    }
}
