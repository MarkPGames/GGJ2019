using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeChildrenAlpha : MonoBehaviour
{

    public void SetAlpha(Color newColor)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).GetComponent<SpriteRenderer>().color = newColor;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
