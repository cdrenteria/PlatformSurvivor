using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewPassive", menuName = "ScriptableObjects/PassiveScriptableObject", order = 2)]
public class PassiveScriptableObject : ScriptableObject
{
    [Header("Weapon Stats")]
    public int passiveLevel;
    public List<string> upgradeProgressionText;

    [Header("Passive Resources")]
    public string passiveName;
    public string passiveDescription;
    public Sprite passiveSprite;
}
