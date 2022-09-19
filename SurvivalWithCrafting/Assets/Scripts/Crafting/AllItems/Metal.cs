using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : ItemsControl
{
    public Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
    }
}
