using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStartControl : MonoBehaviour
{
    public Transform p1, p2;
    public Rigidbody2D rb;
    private bool isGrounded;
    public LayerMask groundMask;
    public int dir;

    public GameObject golpeNavaja;

    public Animator anim;

    public CameraControl2 cameraControl;
    private bool nextTerrain, toGameZone;

    public Camera myCamera;

    public List<Image> hotBarIcon;
    public List<Image> hotBarBase;
    private Color haveNoIcon;
    public GameObject changeScene;
    public Animator animChangeScene;

    void Start()
    {
        changeScene.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        nextTerrain = false;
        haveNoIcon.a = 0.3f;
        hotBarIcon[0].color = haveNoIcon;
        hotBarIcon[1].color = haveNoIcon;
        hotBarIcon[2].color = haveNoIcon;
        hotBarIcon[3].color = haveNoIcon;
        hotBarIcon[4].color = haveNoIcon;
        hotBarBase[0].color = Color.white;
        hotBarBase[1].color = haveNoIcon;
        hotBarBase[2].color = haveNoIcon;
        hotBarBase[3].color = haveNoIcon;
        hotBarBase[4].color = haveNoIcon;
        hotBarBase[5].color = haveNoIcon;
    }

    
    void Update()
    {
        PlayerMovement();
        RazorAttack();
        if(nextTerrain == true && Input.GetKeyDown(KeyCode.E))
        {
            changeScene.SetActive(true);
            animChangeScene.SetBool("changeScene", true);
            cameraControl.ChangeTerrain(1);
            Vector2 nextPos = new Vector2(68f, -6f);
            transform.position = nextPos;
            myCamera.orthographicSize = 8;
        }
        if (toGameZone == true && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(3);
        }
    }
    private void PlayerMovement()
    {
        isGrounded = Physics2D.OverlapArea(p1.position, p2.position, groundMask);
        anim.SetBool("isGrounded", isGrounded);
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * 12, rb.velocity.y);
        if (isGrounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * 22, ForceMode2D.Impulse);
            }
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            anim.SetBool("isMoving", true);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            dir = 1;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            anim.SetBool("isMoving", true);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            dir = -1;
        }
        if (Input.GetAxis("Horizontal") == 0) anim.SetBool("isMoving", false);
    }
    private void RazorAttack()
    {
        if (Input.GetMouseButtonDown(0))//cuando acabe la animación puedes volver a atacar
        {
            if (dir == 1)
            {
                GameObject razorHit = Instantiate(golpeNavaja, new Vector2(transform.position.x + 1, transform.position.y), Quaternion.identity);
                Destroy(razorHit, 0.05f);
                //playerRight = true;
            }
            else
            {
                GameObject razorHit = Instantiate(golpeNavaja, new Vector2(transform.position.x - 1, transform.position.y), Quaternion.identity);
                Destroy(razorHit, 0.05f);
                //playerRight = false;
            }
        }
    }
    private void GoToGameZone()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "GoTo1.1") nextTerrain = true;
        if (other.tag == "GoToGameZone") toGameZone = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "GoTo1.1") nextTerrain = false;
        if (other.tag == "GoToGameZone") toGameZone = false;
    }
}
