using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroySelf : MonoBehaviour
{
    private float damageAmount;
    private float knockback;
    private float effectiveArea;

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Enemy")
        {
            collision.GetComponent<HealthSystem>().damage(damageAmount);
            collision.GetComponent<IEnemy>().Knockback(knockback, this.transform.position);
        }

    }
    
    public void SetDamageAmount(float damage)
    {
        damageAmount = damage;
    }

    public void SetEffectiveArea(float area)
    {
        effectiveArea = area;
        transform.localScale *= area;
    }

    public void SetKnockBack(float kc)
    {
        knockback = kc;
    }
}
