using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour
{
    public Item itemScriptableObject;
    public abstract void Interact();

}
