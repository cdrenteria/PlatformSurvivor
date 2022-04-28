using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeProjectileController : ProjectileBase, ProjectileActionsInterface
{
    // Start is called before the first frame update
    void Start()
    {
        enemiesHit = 0;
        rb = gameObject.GetComponent<Rigidbody2D>();
        despawnTime = 5f;
    }

    private void FixedUpdate()
    {
        timeSinceShot += Time.deltaTime;
        if (timeSinceShot > despawnTime || rb.velocity == Vector2.zero)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "level")
        {
            Destroy(gameObject);
        }
        else if (collision.transform.gameObject.tag == "Enemy")
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
