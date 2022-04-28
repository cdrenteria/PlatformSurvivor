using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class descendPlatform : MonoBehaviour
{
    private CapsuleCollider2D rb;
    private float input;
    private PlatformEffector2D platformObject;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        input = Input.GetAxisRaw("Vertical");

        if (input < 0 && platformObject != null && Input.GetKeyDown(KeyCode.Space))
        {
            print("descend");
            StartCoroutine(descend(platformObject));
        }
    }

    private IEnumerator descend(PlatformEffector2D platform)
    {
        platform.rotationalOffset = 180;
        yield return new WaitForSeconds(.25f);
        platform.rotationalOffset = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlatformEffector2D platform))
        {
            platformObject = platform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlatformEffector2D platform) == platformObject)
        {
            platformObject = null;
        }
    }
}
