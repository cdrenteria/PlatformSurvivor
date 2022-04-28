using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyController : MonoBehaviour, IDamageable, IEnemy
{
    public AudioSource hitSFX;
    private SpriteRenderer GFX;
    public GameObject expPrefab;
    private GameObject Player;
    private Rigidbody2D rb;
    private AIDestinationSetter enemyAI;
    private bool dead = false;
    private HealthSystem healthSystem;
    // Start is called before the first frame update
    void Start()
    {
        healthSystem = gameObject.GetComponent<HealthSystem>();
        GFX = gameObject.GetComponent<SpriteRenderer>();
        hitSFX = gameObject.GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player");
        enemyAI = gameObject.GetComponentInChildren<AIDestinationSetter>();
        enemyAI.target = Player.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet") && !dead)
        {
            hitSFX.PlayOneShot(hitSFX.clip);
            if (healthSystem.health <= 0f)
            {
                Die();
            }

        } else if (collision.gameObject.tag == "Player")
        {
            collision.TryGetComponent<PlayerHealthSystem>(out PlayerHealthSystem playerHealth);
            playerHealth.playerHealthSystem.damage(1);
        }
    }

    public void Die()
    {
        GFX.enabled = false;
        dead = true;
        GameObject experienceInstance = Instantiate(expPrefab, transform.position, transform.rotation);
        experienceInstance.GetComponent<ExperienceController>().setExperienceTier(1);
        Destroy(gameObject, hitSFX.clip.length);
    }

    public void Damage(float damage)
    {
        healthSystem.damage(damage);
    }

    public void Knockback(float knockback, Vector2 sourceVector)
    {
        rb.velocity = Vector2.zero;
        print("Knockback Start: " + transform.position);  
        rb.AddForce((sourceVector - new Vector2(transform.position.x, transform.position.y)) * -knockback, ForceMode2D.Impulse);
        print("Knockback End: " + transform.position);
    }
}
