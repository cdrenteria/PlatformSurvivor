using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserEyesWeapon : WeaponBase, WeaponActionsInterface
{
    [Header("Weapon Specific Fields")]

    private GameObject player;
    private playerController playerController;

    GameObject bullet;
    Rigidbody2D bulletRB;

    // Start is called before the first frame update
    void Start()
    {
        InitializeItemFromScriptableObject();
        player = GameObject.FindGameObjectWithTag("Player");
        sourceTransform = player.transform.GetChild(0);
        playerController = player.GetComponent<playerController>();
        sourceTransform = player.transform.GetChild(0);
        InitializeUpgradeList();
        timeSinceLastFired = 1f;
        itemLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastFired += Time.deltaTime;
        if (canFire())
        {
            Fire();
        }
    }
    public void IncreasePassThroughAmount()
    { passThroughAmount++; 
    }
    public void IncreaseProjectileDamage()
    { projectileDamage += .25f; }
    public void IncreaseEffectiveArea()
    { effectiveArea += .25f; }
    public void IncreaseProjectileAmount()
    { projectileAmount++; }
    public void IncreaseSpeed()
    { speed += .15f; }
    public void DecreaseCoodown()
    { cooldown *= .9f; }
    public void IncreaseKnockback(float amount)
    { knockback += amount; }
    public void LevelUp()
    {
        itemLevel++;
        upgradeProgression[itemLevel-1]();
    }
    public void Fire()
    {
        for (int x = 0; x < projectileAmount; x++)
        {
            //Instantiate Bullet
            InstantiateAndInitializeBullet(x);
            //FireBullet
            FireBullet();
        }
        timeSinceLastFired = 0f;
    }

  
    private void FireBullet()
    {
        bulletRB = bullet.GetComponent<Rigidbody2D>();
        //fire bullet in the direction that player last moved according to the speed
        bulletRB.velocity =  new Vector2(playerController.moveDirection.x, 0).normalized* speed;
    }

    private GameObject InstantiateAndInitializeBullet(int x)
    {
        bullet = GameObject.Instantiate(projectilePrefab, new Vector2(sourceTransform.position.x, sourceTransform.position.y+x), sourceTransform.rotation);
        KnifeProjectileController bulletPC = bullet.GetComponent<KnifeProjectileController>();
        bulletPC.SetDamage(projectileDamage);
        bulletPC.SetPassThroughAmount(passThroughAmount);
        bulletPC.SetEffectiveArea(effectiveArea);
        return bullet;
    }

    public void InitializeUpgradeList()
    {
        upgradeProgression = new List<Action>();
        upgradeProgression.Add(IncreaseProjectileAmount);
        upgradeProgression.Add(IncreaseProjectileDamage);
        upgradeProgression.Add(IncreasePassThroughAmount);
        upgradeProgression.Add(IncreaseProjectileAmount);
    }

}
