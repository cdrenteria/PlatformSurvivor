using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface WeaponActionsInterface
{
    void InitializeUpgradeList();
    void IncreaseProjectileAmount();
    void IncreasePassThroughAmount();
    void IncreaseProjectileDamage();
    void IncreaseEffectiveArea();
    void IncreaseSpeed();
    void DecreaseCoodown();
    void LevelUp();
    void Fire();
    void IncreaseKnockback( float amount);
}
