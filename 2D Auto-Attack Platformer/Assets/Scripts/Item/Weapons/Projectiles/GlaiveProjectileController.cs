using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlaiveProjectileController : ProjectileBase, ProjectileActionsInterface
{
    private List<GameObject> enemies;
    private RicochetGlaiveWeapon parentController;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        enemiesHit = 0;
        despawnTime = 5f;
    }
    private void Update()
    {
        transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z+5f, 0);
    }

    private void GetNewTarget(Collider2D collision)
    {
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        enemies.Remove(collision.gameObject);
        foreach (GameObject enemy in enemies)
        {
            if (target == null)
            {
                target = enemy;
            }
            else
            {
                if (Vector2.Distance(transform.position, target.transform.position) > Vector2.Distance(transform.position, enemy.transform.position))
                {
                    target = enemy;
                }
            }
        }
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
            GetNewTarget(collision);
            rb.velocity = Vector2.zero;
            rb.velocity = (target.transform.position - transform.position).normalized * parentController.speed;
        }

    }

    public void SetParent(GameObject parent)
    {
        parentController = parent.GetComponent<RicochetGlaiveWeapon>();
    }
}
