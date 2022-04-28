using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponBase: ItemBase
{
    [Header("Weapon Stats")]
    public int projectileAmount;
    public int passThroughAmount;
    public float projectileDamage;
    public float effectiveArea;
    public float speed;
    public float cooldown;
    public float knockback;
    public float timeSinceLastFired;
    public List<Action> upgradeProgression;
    public int weaponLevel;

    [Header("Weapon Resources")]
    public int id;
    public GameObject projectilePrefab;
    public Transform sourceTransform;

    //Weapon Leveling
    public virtual string GetNextWeaponUprade()
    {
        if (upgradeProgression[weaponLevel + 1] != null)
        {
            print(this.upgradeProgression[weaponLevel + 1].Method.Name);
            //if (upgradeProgression[weaponLevel + 1].Method.Name == "IncreasePassThroughAmount")
            //{
            //    return "hi";
            //}
        }
        return "";
    }

    //Weapon Firing
    public bool canFire()
    {
        if (timeSinceLastFired >= cooldown) { return true; }
        else { return false; }
    }

    public override void InitializeItemFromScriptableObject()
    {
        base.InitializeItemFromScriptableObject();
        //initialize fields from scriptable Object

        this.projectileAmount = itemScriptableObject.projectileAmount;
        this.passThroughAmount = itemScriptableObject.passThroughAmount;
        this.projectileDamage = itemScriptableObject.projectileDamage;
        this.effectiveArea = itemScriptableObject.effectiveArea;
        this.speed = itemScriptableObject.speed;
        this.cooldown = itemScriptableObject.cooldown;
        this.id = itemScriptableObject.id;
        this.projectilePrefab = itemScriptableObject.projectilePrefab;
        this.knockback = itemScriptableObject.knockback;
    }
}
