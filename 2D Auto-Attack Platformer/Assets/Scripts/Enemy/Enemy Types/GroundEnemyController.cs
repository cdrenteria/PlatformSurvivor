using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class GroundEnemyController : MonoBehaviour, IDamageable, IEnemy
{
    public AudioSource hitSFX;
    private SpriteRenderer GFX;
    public GameObject expPrefab;
    private GameObject Player;
    private bool dead = false;
    private HealthSystem healthSystem;
    private LayerMask platforms;



    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = .5f;

    [Header("Physics")]
    public float speed = 750f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = .8f;
    public float jumpModifier = 1f;
    public float jumpCheckOffSet = .1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool flipGFX = true;

    private Path path;
    private int currentWaypoint = 0;
    bool isGrounded = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Path p;


    // Start is called before the first frame update
    void Start()
    {
        platforms = LayerMask.GetMask("platforms");
        rb = gameObject.GetComponent<Rigidbody2D>();
        healthSystem = gameObject.GetComponent<HealthSystem>();
        healthSystem.health = .75f;
        GFX = gameObject.GetComponent<SpriteRenderer>();
        hitSFX = gameObject.GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player");
        target = Player.transform;
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }


    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
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
        isGrounded = Physics2D.Raycast(transform.position, -Vector3.up, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffSet, platforms);
        //Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        //jump
        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }
        //Movement
        rb.AddForce(force);

        //Get Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if ( distance < nextWaypointDistance)
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



    private void OnColliderEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bullet") && !dead)
        {
            hitSFX.PlayOneShot(hitSFX.clip);
            if (healthSystem.health <= 0f)
            {
                Die();
            }

        }
        else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.TryGetComponent<PlayerHealthSystem>(out PlayerHealthSystem playerHealth);
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
        rb.AddForce((sourceVector - new Vector2(transform.position.x, transform.position.y)) * -knockback*3f, ForceMode2D.Impulse);
    }

}
