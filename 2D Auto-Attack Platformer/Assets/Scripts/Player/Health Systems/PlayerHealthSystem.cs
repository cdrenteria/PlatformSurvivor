using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    public HealthSystem playerHealthSystem;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthSystem = gameObject.AddComponent<HealthSystem>();
        playerHealthSystem.setHealth(6f);
    }
}
