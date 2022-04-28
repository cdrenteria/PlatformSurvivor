using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class FlyingEnemyVariantOneController : MonoBehaviour, IDamageable, IEnemy
{
    private GraphNode knockbackNode;
    public AudioSource hitSFX;
    private SpriteRenderer GFX;
    public GameObject expPrefab;
    private GameObject Player;
    private AIDestinationSetter enemyAI;
    private bool dead = false;
    private HealthSystem healthSystem;
    private Rigidbody2D rb;

    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = .5f;

    [Header("Physics")]
    public float speed = 250f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = .8f;
    public float jumpModifier = .3f;
    public float jumpCheckOffSet = .1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool flipGFX = true;

    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Path p;
    // Start is called before the first frame update
    void Start()
    {
        speed = 250f;
        healthSystem = gameObject.GetComponent<HealthSystem>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        healthSystem.health = 1.5f;
        GFX = gameObject.GetComponent<SpriteRenderer>();
        hitSFX = gameObject.GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player");;
        target = Player.transform;
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
        this.GetComponent<Collider2D>().isTrigger = true;
    }


    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
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

        }
        else if (collision.gameObject.tag == "Player")
        {
            collision.TryGetComponent<PlayerHealthSystem>(out PlayerHealthSystem playerHealth);
            playerHealth.playerHealthSystem.damage(3);
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }
    private void PathFollow()
    {
        //Dont Follow if there is no path or if you are at the end of the path
        if (path == null || currentWaypoint >= path.vectorPath.Count) return;
        //Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        //Movement
        rb.velocity = force;

        //Get Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (p.error != true)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void Die()
    {
        GFX.enabled = false;
        dead = true;
        GameObject experienceInstance = Instantiate(expPrefab, transform.position, transform.rotation);
       // experienceInstance.GetComponent<ExperienceController>().setExperienceTier(2);
        Destroy(gameObject, hitSFX.clip.length);
    }

    public void Damage(float damage)
    {
        healthSystem.damage(damage);
    }

    public void Knockback(float knockback, Vector2 sourceVector)
    {
        rb.velocity = Vector2.zero;
        print("knockback value: " + knockback);
        print("Knockback Start: " + transform.position);
        rb.AddForce((sourceVector - new Vector2(transform.position.x, transform.position.y)) * -knockback, ForceMode2D.Impulse);
        print("Knockback End: " + transform.position);
    }
}
