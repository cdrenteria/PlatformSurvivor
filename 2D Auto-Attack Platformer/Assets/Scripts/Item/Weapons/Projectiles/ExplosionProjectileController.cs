using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionProjectileController : ProjectileBase, ProjectileActionsInterface
{
    private System.Random rnd;
    public GameObject projectilePrefab;
    private int projectileAmount;
    private float knockback;
    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();
        enemiesHit = 0;
        passThroughAmount = 1;
        damageAmount = .75f;
        rb = gameObject.GetComponent<Rigidbody2D>();
        despawnTime = 5f;
    }

    private void FixedUpdate()
    {
        timeSinceShot += Time.deltaTime;
        if (timeSinceShot > despawnTime)
        {
            Destroy(gameObject);
        }
        if (rb.velocity == Vector2.zero)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "level")
        {
            Explode();
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<IEnemy>().Knockback(knockback, this.transform.position);
            collision.GetComponent<HealthSystem>().damage(damageAmount);
            Explode();
            enemiesHit++;
            Destroy(gameObject);

        }
    }

    private void Explode()
    {
        for (int x = 0; x < projectileAmount; x++)
        {
            int randomX = rnd.Next(-3, 3);
            int randomY = rnd.Next(-3,3);
            Vector2 explosionPosition = transform.position + new Vector3(randomX, randomY, 0);
            GameObject explosion = Instantiate(projectilePrefab, explosionPosition, transform.rotation);
            destroySelf explosionController = explosion.GetComponent<destroySelf>();
            explosionController.SetDamageAmount(damageAmount);
            explosionController.SetEffectiveArea(effectiveArea);
            explosionController.SetKnockBack(knockback);
        }
    }

    public void SetProjectileAmount(int amount)
    {
        projectileAmount = amount;
    }
    public void SetKnockback(float kb)
    {
        knockback = kb;
    }
}
