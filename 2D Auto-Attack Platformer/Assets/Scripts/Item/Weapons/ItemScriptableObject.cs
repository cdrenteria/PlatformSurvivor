using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    [Header("Stats")]
    public int projectileAmount;
    public int passThroughAmount;
    public float projectileDamage;
    public float effectiveArea;
    public float speed;
    public float cooldown;
    public float knockback;
    public int itemLevel;
    public List<string> upgradeProgressionText;

    [Header("Resources")]
    public int id;
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public GameObject projectilePrefab;


}

