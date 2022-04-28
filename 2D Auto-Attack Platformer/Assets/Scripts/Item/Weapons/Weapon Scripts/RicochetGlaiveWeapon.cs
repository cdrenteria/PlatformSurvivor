using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class RicochetGlaiveWeapon : WeaponBase, WeaponActionsInterface
{

    [Header("Weapon Specific Fields")]
    private List<GameObject> targets;
    public List<GameObject> enemies;
    private GameObject player;
    private Vector2 playerPosition;

    GameObject bullet;
    Rigidbody2D bulletRB;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sourceTransform = player.transform.GetChild(0);
        playerPosition = player.transform.position;
        InitializeItemFromScriptableObject();
        InitializeUpgradeList();
        InitializeWeaponSpecificLists();
        timeSinceLastFired = 2f;
        weaponLevel = 1;
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
        weaponLevel++;
        upgradeProgression[weaponLevel]();
    }
    public void Fire()
    {
        //Find all enemies and store them in a List of enmies
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        for (int x = 0; x < enemies.Count; x++)
        {
            // If there are no current targets then add current enemy to targets
            if (targets.Count < projectileAmount)
            {
                targets.Add(enemies[x]);
                enemies.Remove(enemies[x]);
            }

            for (int y = 0; y < targets.Count; y++)
            {
                playerPosition = player.transform.position;
                //compare the distance between the target and enemy with the player
                if (CompareEnemyDistance(x, y))
                {
                    //swap target and enemy if enemy is closer than target
                    SwapEnemies(x, y);
                }
            }
        }
        //Instantiate a projectile and add force in the direction of the enemy
        for (int x = 0; x < targets.Count; x++)
        {
            InstantiateAndInitializeBullet();
            FireBullet(targets[x]);
        }
        timeSinceLastFired = 0;
        targets.Clear();
    }

    private bool CompareEnemyDistance(int x, int y)
    {
        return Vector2.Distance(playerPosition, targets[y].transform.position) > Vector2.Distance(playerPosition, enemies[x].transform.position);
    }

    private void SwapEnemies(int x, int y)
    {
        enemies.Add(targets[y]);
        targets.Remove(targets[y]);
        targets.Add(enemies[x]);
        enemies.Remove(enemies[x]);
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
        GlaiveProjectileController bulletPC = bullet.GetComponent<GlaiveProjectileController>();
        bulletPC.SetParent(this.gameObject);
        bulletPC.SetDamage(projectileDamage);
        bulletPC.SetPassThroughAmount(passThroughAmount);
        bulletPC.SetEffectiveArea(effectiveArea);
        return bullet;
    }
    private void InitializeWeaponSpecificLists()
    {
        targets = new List<GameObject>();
        enemies = new List<GameObject>();
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
