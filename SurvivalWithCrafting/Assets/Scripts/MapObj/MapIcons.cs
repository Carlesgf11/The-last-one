using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIcons : MonoBehaviour
{
    public bool playerIn;
    private Color color0;
    public LayerMask player;
    void Start()
    {
        color0.a = 0;
        GetComponent<Renderer>().material.color = color0;
    }

    
    void Update()
    {
        playerIn = Physics2D.OverlapCircle(transform.position, 3, player);
        if (playerIn == true)
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
        else
        {
            GetComponent<Renderer>().material.color = color0;
        }
    }
}
