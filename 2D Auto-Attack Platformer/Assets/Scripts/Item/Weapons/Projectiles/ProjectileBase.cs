using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float despawnTime;
    public float timeSinceShot;

    public Rigidbody2D rb;

    public float damageAmount;
    public int passThroughAmount;
    public float effectiveArea;
    public float speed;

    public int enemiesHit;


    public void SetDamage(float newDamageAmount)
    {
        damageAmount = newDamageAmount;
    }


    public void SetPassThroughAmount(int newPassThroughAmount)

    {
        passThroughAmount = newPassThroughAmount;
    }

    public void SetEffectiveArea(float newEffectiveArea)
    {
        effectiveArea = newEffectiveArea;
        transform.localScale *= effectiveArea;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

}
