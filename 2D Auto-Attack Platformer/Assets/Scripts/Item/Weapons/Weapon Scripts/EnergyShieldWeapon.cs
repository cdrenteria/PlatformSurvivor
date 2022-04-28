using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnergyShieldWeapon : WeaponBase, WeaponActionsInterface
{


    // Update is called once per frame
    void Update()
    {
        InitializeUpgradeList();
        InitializeItemFromScriptableObject();
        timeSinceLastFired += Time.deltaTime;
    }

    //Interface Methods
    public void IncreaseProjectileAmount()
    { projectileAmount++; }
    public void IncreasePassThroughAmount()
    { passThroughAmount++; }
    public void IncreaseProjectileDamage()
    { projectileDamage += .25f; }
    public void IncreaseEffectiveArea()
    { effectiveArea += .25f; }
    public void IncreaseSpeed()
    { speed += .15f; }
    public void DecreaseCoodown()
    { cooldown *= .9f; }
    public void IncreaseKnockback(float amount)
    { knockback += amount; }
    public void LevelUp()
    {
        weaponLevel++;
        upgradeProgression[weaponLevel]();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<HealthSystem>().damage(projectileDamage);
            timeSinceLastFired = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && canFire())
        {
            collision.GetComponent<HealthSystem>().damage(projectileDamage);
            timeSinceLastFired = 0f;
        }
    }
    public void Fire()
    {
        throw new NotImplementedException("This weapon does not fire, nerd.");
    }

    public void InitializeUpgradeList()
    {
        upgradeProgression = new List<Action>();
        upgradeProgression.Add(IncreaseEffectiveArea);
        upgradeProgression.Add(IncreaseProjectileDamage);
        upgradeProgression.Add(IncreaseProjectileDamage);
        upgradeProgression.Add(IncreaseEffectiveArea);
    }
}
