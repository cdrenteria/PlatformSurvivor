using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandProjectileController : ProjectileBase, ProjectileActionsInterface
{
    // Start is called before the first frame update
    void Start()
    {
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
        
        if (collision.gameObject.tag == "level" )
        {
            Destroy(gameObject);
        } else if (collision.transform.gameObject.tag == "Enemy")
        {
            enemiesHit++;
            collision.GetComponent<HealthSystem>().damage(damageAmount);
            if (enemiesHit >= passThroughAmount)
            {
                Destroy(gameObject);
            }
        } 
    }

}
