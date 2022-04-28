using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UpgradeMenu : MonoBehaviour
{
    private Transform currentUpgradeSlot;
    public List<GameObject> updateItems;
    private Transform playerWeaponSlots;
    private Transform playerPassiveSlots;

    public void UpdateMenu(List<GameObject> updateItems)
    {
        //Loop through all UpgradeSlots
        for ( int x = 0; x < 3; x ++)
        {
            currentUpgradeSlot = transform.GetChild(x);

         
            //Determine if the player has the item
            CheckForItemInPlayerInventory(x, updateItems);
            //if they do then pull this information from their instanced weapon
            UpdateItemMenuSlots(updateItems, x);

        }
    }

    private void CheckForItemInPlayerInventory(int x, List<GameObject> updateItems)
    {
        GameObject currentItem = updateItems[x];
        playerWeaponSlots = GameObject.FindGameObjectWithTag("Player").transform.Find("Weapons");
        playerPassiveSlots = GameObject.FindGameObjectWithTag("Player").transform.Find("Passives");

        CheckWeaponSlots(x, updateItems, currentItem, playerWeaponSlots);
        CheckPassiveSlots(x, updateItems, currentItem, playerPassiveSlots);
    }

    private static void CheckPassiveSlots(int x, List<GameObject> updateItems, GameObject currentItem, Transform playerPassiveSlots)
    {
        for (int y = 0; y < playerPassiveSlots.childCount; y++)
        {
            Transform currentPassiveSlot = playerPassiveSlots.GetChild(y);
            if (currentPassiveSlot.childCount > 0)
            {
                if (currentPassiveSlot.GetChild(0).GetComponent<ItemBase>().itemName == currentItem.GetComponent<ItemBase>().itemName)
                {
                    print("You already have: " + currentItem.GetComponent<ItemBase>().itemName);
                    updateItems[x] = currentPassiveSlot.GetChild(0).gameObject;
                }
            }
        }
    }

    private static void CheckWeaponSlots(int x, List<GameObject> updateItems, GameObject currentItem, Transform playerWeaponSlots)
    {
        for (int y = 0; y < playerWeaponSlots.childCount; y++)
        {
            Transform currentWeaponSlot = playerWeaponSlots.GetChild(y);
            //Check that there is a weapon in the slot
            if (currentWeaponSlot.childCount > 0)
            {
                //compare weapon names for equality
                if (currentWeaponSlot.GetChild(0).gameObject.GetComponent<ItemBase>().itemName == currentItem.GetComponent<ItemBase>().itemName)
                {
                    //swap generic weapon for instance from players inventory
                    updateItems[x] = currentWeaponSlot.GetChild(0).gameObject;
                }
            }
        }
    }

    private void UpdateItemMenuSlots(List<GameObject> updateItems, int x)
    {
        GameObject item = Instantiate(updateItems[x].gameObject,
        currentUpgradeSlot.Find("Item"));
        ItemBase currentUpdateItem = item.GetComponent<ItemBase>();
        currentUpgradeSlot.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().text = currentUpdateItem.itemScriptableObject.itemName;
        currentUpgradeSlot.Find("Description").gameObject.GetComponent<TextMeshProUGUI>().text = currentUpdateItem.itemScriptableObject.upgradeProgressionText[currentUpdateItem.itemLevel];

        //print(updateItems[x].GetComponent<WeaponBase>().weaponLevel);
        //currentUpgradeSlot.Find("Image").gameObject.GetComponent<Image>().sprite = updateItems[x].GetComponent<WeaponBase>().weaponScriptableObject.weaponSprite;
    }

    public void DisableMenu()
    {
        //Loop through all UpgradeSlots and remove items
        for (int x = 0; x < 3; x++)
        {
            currentUpgradeSlot = transform.GetChild(x);
            if (currentUpgradeSlot.transform.Find("Item").childCount > 0)
            {
                Destroy(currentUpgradeSlot.transform.Find("Item").GetChild(0).gameObject);
            }
            
        }
        gameObject.SetActive(false);
        //unpause the game 
        Time.timeScale = 1f;
    }

    public void addItemToPlayer(GameObject item)
    {
        //find choice in updateitems
        item = item.transform.GetChild(0).gameObject;
        print(item.GetComponent<ItemBase>().itemName);
        //increase level of held items

        if (item.TryGetComponent(out WeaponBase weaponController))
        {
            for (int y = 0; y < playerWeaponSlots.childCount; y++)
            {
                Transform currentWeaponSlot = playerWeaponSlots.GetChild(y);
                //Check that there is a weapon in the slot
                if (currentWeaponSlot.childCount > 0)
                {
                    //compare weapon names for equality
                    if (currentWeaponSlot.GetChild(0).gameObject.GetComponent<ItemBase>().itemName == item.GetComponent<ItemBase>().itemName)
                    {
                        print("upgrading: " + currentWeaponSlot.GetChild(0).gameObject.GetComponent<ItemBase>().itemName);
                        currentWeaponSlot.GetChild(0).gameObject.GetComponent<WeaponActionsInterface>().LevelUp();
                        break;
                    }
                }
                else
                {
                    item.transform.position = currentWeaponSlot.transform.position;
                    item.transform.parent = currentWeaponSlot;
                    item.GetComponent<WeaponActionsInterface>().LevelUp();
                    break;
                }
            }
        } else
        {
            for (int y = 0; y < playerPassiveSlots.childCount; y++)
            {
                Transform currentPassiveSlot = playerPassiveSlots.GetChild(y);
                if (currentPassiveSlot.childCount > 0)
                {
                    if (currentPassiveSlot.GetChild(0).GetComponent<ItemBase>().itemName == item.GetComponent<ItemBase>().itemName)
                    {
                        print("Upgrading: " + item.GetComponent<ItemBase>().itemName);
                        currentPassiveSlot.GetChild(0).gameObject.GetComponent<PassiveActionsInterface>().LevelUp();
                    }
                }
                else
                {
                    item.transform.position = currentPassiveSlot.transform.position;
                    item.transform.parent = currentPassiveSlot;
                    item.GetComponent<PassiveBase>().itemLevel = 1;
                    item.GetComponent<PassiveActionsInterface>().ApplyPassiveEffect();
                    break;
                }
            }
        }
        
    }
}
