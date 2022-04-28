using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class WeaponScriptableObject : ScriptableObject
{
    [Header("Weapon Stats")]
    public int projectileAmount;
    public int passThroughAmount;
    public float projectileDamage;
    public float effectiveArea;
    public float speed;
    public float cooldown;
    public int weaponLevel;
    public float knockback;
    public List<string> upgradeProgressionText;

    [Header("Weapon Resources")]
    public int id;
    public string weaponName;
    public string weaponDescription;
    public Sprite weaponSprite;
    public GameObject projectilePrefab;

}
public enum FireType
{
    LOCK_ON,
    LAUNCH_PROJECTILES,
    STATIC_AREA,
    AOE_THROWABLE,
    DIRECTIONAL_THROWABLE
}

