using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovControl : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool explode;
    public GameObject molotovColl;
    public PlayerControl player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        explode = false;
        if (player.dir == 1)
        {
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            rb.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            rb.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
        }
    }

    
    void Update()
    {
        if (explode == true)
        {
            GameObject molotov = Instantiate(molotovColl, transform.position, Quaternion.identity);
            Destroy(molotov, 0.1f);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            SoundManager.PlaySound("BotellaRotaSound");
            explode = true;
        }
        if (other.tag == "Enemy") Destroy(gameObject);
    }
}
