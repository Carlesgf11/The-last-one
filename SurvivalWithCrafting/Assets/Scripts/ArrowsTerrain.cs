using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsTerrain : MonoBehaviour
{
    public bool playerIn;
    public GameObject Player;
    public LayerMask player;

    public Vector2 teleportPos;
    public List<GameObject> teleports;
    public CameraControl1 cameraControl;
    public int currentTerrain;

    public Animator anim;
    public GameObject changeScene;

    private Color color0;

    private void Start()
    {
        color0.a = 0;
        GetComponent<Renderer>().material.color = color0;
        changeScene.SetActive(false);
    }

    void Update()
    {
        playerIn = Physics2D.OverlapCircle(transform.position, 3, player);
        Teleport();
        if(playerIn == true)
        {
            GetComponent<Renderer>().material.color = Color.white;
            if (Input.GetKeyDown(KeyCode.E))
            {
                changeScene.SetActive(true);
                anim.SetBool("changeScene", true);
                Player.transform.position = teleportPos;
                cameraControl.ChangeTerrain(currentTerrain);
            }
        }
        else
        {
            GetComponent<Renderer>().material.color = color0;
        }
    }

    public void ChangeSceneFalse()
    {
        anim.SetBool("changeScene", false);
    }
    public void Teleport()
    {
        if (gameObject.tag == "ToOne")
        {
            teleportPos = teleports[5].transform.position;
            currentTerrain = 1;
        }
        if (gameObject.tag == "ToTwo")
        {
            teleportPos = teleports[10].transform.position;
            currentTerrain = 5;
        }
        if (gameObject.tag == "ToDenA")
        {
            teleportPos = teleports[0].transform.position;
            currentTerrain = 0;
        }
        if (gameObject.tag == "ToOneOne")
        {
            teleportPos = teleports[6].transform.position;
            currentTerrain = 2;
        }
        if (gameObject.tag == "ToOneTwo")
        {
            teleportPos = teleports[7].transform.position;
            currentTerrain = 3;
        }
        if (gameObject.tag == "ToOneThree")
        {
            teleportPos = teleports[8].transform.position;
            currentTerrain = 4;
        }
        if (gameObject.tag == "ToOneOne_")
        {
            teleportPos = teleports[2].transform.position;
            currentTerrain = 1;
        }
        if (gameObject.tag == "ToOneTwo_")
        {
            teleportPos = teleports[3].transform.position;
            currentTerrain = 1;
        }
        if (gameObject.tag == "ToOneThree_")
        {
            teleportPos = teleports[4].transform.position;
            currentTerrain = 1;
        }
        if (gameObject.tag == "ToTwoTwo")
        {
            teleportPos = teleports[13].transform.position;
            currentTerrain = 5;
        }
        if (gameObject.tag == "ToDenB")
        {
            teleportPos = teleports[1].transform.position;
            currentTerrain = 0;
        }
        if (gameObject.tag == "ToEnd")
        {
            teleportPos = teleports[14].transform.position;
            currentTerrain = 6;
        }
        if (gameObject.tag == "ToHole")
        {
            teleportPos = teleports[11].transform.position;
            currentTerrain = 5;
        }
    }

}
