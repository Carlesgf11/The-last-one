using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
 
    void Update()
    {
        transform.Translate(Vector2.right * 17 * Time.deltaTime);
        Destroy(gameObject, 0.7f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy") Destroy(gameObject);
    }
}
