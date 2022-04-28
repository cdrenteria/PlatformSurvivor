using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    public AudioSource experienceSFX;
    private SpriteRenderer outerGFX;
    private SpriteRenderer innerGFX;
    private int experienceTier;

    private void Start()
    {
        experienceSFX = gameObject.GetComponent<AudioSource>();
        outerGFX = gameObject.GetComponent<SpriteRenderer>();
        innerGFX = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out LevelSystem playerLevelSystem))
        {
            outerGFX.enabled = false;
            innerGFX.enabled = false;
            experienceSFX.PlayOneShot(experienceSFX.clip);
            playerLevelSystem.AddExperience(experienceTier);
            Destroy(gameObject, experienceSFX.clip.length);
        }
    }

    public void setExperienceTier(int tier)
    {
        experienceTier = tier;
    }
}
