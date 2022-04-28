using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ProjectileActionsInterface
{
    void OnTriggerEnter2D(Collider2D collision);
    void SetDamage(float newDamageAmount);
    void SetPassThroughAmount(int newPassThroughAmount);
    void SetEffectiveArea(float newEffectiveArea);
    void SetSpeed(float newSpeed);
}
