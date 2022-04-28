using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoppingPower : PassiveBase, PassiveActionsInterface
{
    private List<GameObject> playerWeaponSlots = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InitializeItemFromScriptableObject();
        playerWeaponSlots.AddRange(GameObject.FindGameObjectsWithTag("WeaponSlot"));
    }

    public void LevelUp()
    {
        itemLevel++;
        ApplyPassiveEffect();
    }

    public void ApplyPassiveEffect()
    {
        for (int y = 0; y < playerWeaponSlots.Count; y++)
        {
            Transform currentWeaponSlot = playerWeaponSlots[y].transform;
            //Check that there is a weapon in the slot
            if (currentWeaponSlot.childCount > 0)
            {
                currentWeaponSlot.GetChild(0).gameObject.GetComponent<WeaponActionsInterface>().IncreaseKnockback(this.itemScriptableObject.knockback);
            }
        }

    }

}
