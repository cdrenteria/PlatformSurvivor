using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public ItemScriptableObject itemScriptableObject;
    public int itemLevel;
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;

    //Weapon Creation
    public virtual void InitializeItemFromScriptableObject()
    {
        //initialize fields from scriptable Object
        this.itemName = itemScriptableObject.itemName;
        this.itemDescription = itemScriptableObject.itemDescription;
        this.itemSprite = itemScriptableObject.itemSprite;
        this.itemLevel = itemScriptableObject.itemLevel;
    }
}
