using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ClusterBombWeapon : WeaponBase, WeaponActionsInterface
{

    [Header("Weapon Specific Fields")]
    private GameObject target;
    public List<GameObject> enemies;
    private GameObject player;
    private Vector2 playerPosition;
    private System.Random rnd;
    GameObject bullet;
    Rigidbody2D bulletRB;

    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();
        player = GameObject.FindGameObjectWithTag("Player");
        sourceTransform = player.transform.GetChild(0);
        playerPosition = player.transform.position;
        InitializeItemFromScriptableObject();
        InitializeUpgradeList();
        InitializeWeaponSpecificLists();
        timeSinceLastFired = 2f;
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

    public void IncreaseProjectileAmount()
    { projectileAmount++; }
    public void IncreasePassThroughAmount()
    { passThroughAmount++; }
    public void IncreaseProjectileDamage()
    { projectileDamage += .25f; }
    public void IncreaseEffectiveArea()
    { effectiveArea += .25f; }
    public void IncreaseSpeed()
    { speed += .25f; }
    public void DecreaseCoodown()
    { cooldown *= .9f; }
    public void IncreaseKnockback(float amount)
    { knockback += amount; }
    public void LevelUp()
    {
        itemLevel++;
        upgradeProgression[itemLevel]();
    }
    public void Fire()
    {
        //Find all enemies and store them in a List of enmies
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        target = enemies[rnd.Next(enemies.Count)];
        
        //Instantiate a projectile and add force in the direction of the enemy
    
        InstantiateAndInitializeBullet();
        FireBullet(target);

        timeSinceLastFired = 0;
    }


    private void FireBullet(GameObject target)
    {
        bulletRB = bullet.GetComponent<Rigidbody2D>();
        Vector3 direction = target.transform.position - sourceTransform.position;
        bulletRB.velocity = new Vector2(direction.x, direction.y).normalized * speed;
    }

    private GameObject InstantiateAndInitializeBullet()
    {
        bullet = GameObject.Instantiate(projectilePrefab, sourceTransform.position, sourceTransform.rotation);
        ExplosionProjectileController bulletPC = bullet.GetComponent<ExplosionProjectileController>();
        bulletPC.SetKnockback(knockback);
        bulletPC.SetProjectileAmount(projectileAmount);
        bulletPC.SetDamage(projectileDamage);
        bulletPC.SetEffectiveArea(effectiveArea);
        return bullet;
    }
    private void InitializeWeaponSpecificLists()
    {
        enemies = new List<GameObject>();
    }

    public void InitializeUpgradeList()
    {
        upgradeProgression = new List<Action>();
        upgradeProgression.Add(IncreaseProjectileAmount);
        upgradeProgression.Add(IncreaseProjectileDamage);
        upgradeProgression.Add(IncreaseEffectiveArea);
        upgradeProgression.Add(IncreaseSpeed);
    }
}
