using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPruebasControl : MonoBehaviour
{
    public enum EnemyStates { GO, ATTACK };
    public EnemyStates state;
    public PlayerControl player;
    public Rigidbody2D rb;
    private int lifes;

    public GameObject Player, radiusCentre;
    public Transform transformPlayer;
    private bool playerIn;
    public LayerMask playerMask;

    private float attackCount, attackRate;
    public GameObject golpeEnemy;

    public int dir;

    public List<PercentProperties> allItems;
    public int finalId, finalAmount;

    public Animator anim;
    private bool movement;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lifes = 5;
    }

    
    void Update()
    {
        EnemyDir();
        switch (state)
        {
            case EnemyStates.GO:
                GoUpdate();
                break;
            case EnemyStates.ATTACK:
                AttackUpdate();
                break;
        }
        if (lifes <= 0)
        {
            ProbInst();
            Destroy(gameObject);
        }
    }
    public void GetDamage()
    {
        SoundManager.PlaySound("Herido2Sound");
        if (transform.position.x > Player.transform.position.x)
        {
            rb.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
            rb.AddForce(Vector2.right * 20, ForceMode2D.Impulse);
        }
        if (transform.position.x < Player.transform.position.x)
        {
            rb.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
            rb.AddForce(Vector2.left * 20, ForceMode2D.Impulse);
        }
    }
    public void EnemyDir()
    {
        if (playerIn == true)
        {
            if (transform.position.x > Player.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                dir = -1;
            }
            if (transform.position.x < Player.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                dir = 1;
            }
        }
    }
    public void GoUpdate()
    {
        playerIn = Physics2D.OverlapCircle(transform.position, 8, playerMask);
        if(playerIn == true)
        {
            anim.SetBool("isMoving", true);
            Vector2 finalPos = Player.transform.position;
            finalPos.y = transform.position.y;
            if (dir == -1)
            {
                finalPos.x = Player.transform.position.x + 2;
                transform.position = Vector2.MoveTowards(transform.position, finalPos, 5 * Time.deltaTime);
                if (Vector2.Distance(transform.position, transformPlayer.position) < 3) //state = EnemyStates.ATTACK;
                {
                    anim.SetBool("isMoving", false);
                    AttackUpdate();
                }
            }
            if (dir == 1)
            {
                finalPos.x = Player.transform.position.x -2;
                transform.position = Vector2.MoveTowards(transform.position, finalPos, 5 * Time.deltaTime);
                if (Vector2.Distance(transform.position, transformPlayer.position) < 3) //state = EnemyStates.ATTACK;
                {
                    anim.SetBool("isMoving", false);
                    AttackUpdate();
                }
            }
        }
        else 
            anim.SetBool("isMoving", false);
    }
    public void AttackUpdate()
    {
        attackCount += Time.deltaTime;
        attackRate = 0.8f;
        if (attackCount >= attackRate)
        {
            anim.SetTrigger("isAttacking");
            if (dir == -1)
            {
                GameObject enemyHit = Instantiate(golpeEnemy, new Vector2(transform.position.x - 1.7f, transform.position.y), Quaternion.identity);
                Destroy(enemyHit, 0.1f);
                attackCount = 0;
            }
            if (dir == 1)
            {
                GameObject enemyHit = Instantiate(golpeEnemy, new Vector2(transform.position.x + 1.7f, transform.position.y), Quaternion.identity);
                Destroy(enemyHit, 0.1f);
                attackCount = 0;
            }
        }
    }
    private void ProbInst()
    {
        finalId = -1;
        finalAmount = 0;
        int totalPercent = 0;
        for (int i = 0; i < allItems.Count; i++)
        {
            totalPercent += allItems[i].percent;
        }
        int randomPercent = Random.Range(0, totalPercent);
        for (int i = 0; i < allItems.Count; i++)
        {
            randomPercent -= allItems[i].percent;
            if (randomPercent <= 0)
            {
                finalId = allItems[i].item.id;
                finalAmount = Random.Range(1, allItems[i].amount + 1);
                //print(allItems[i].item.ItemName);
                GameObject itemPref = Instantiate(allItems[i].pref, transform.position, Quaternion.identity);
                itemPref.GetComponent<ItemsControl>().amount = finalAmount;
                break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "RazorHit")
        {
            SoundManager.PlaySound("AcuchillarSound");
            GetDamage();
            lifes--;
        }
        if(other.tag == "Bullet")
        {
            GetDamage();
            lifes--;
        }
        if (other.tag == "Molotov")
        {
            GetDamage();
            lifes--;
        }
        if (other.tag == "Molotov") Destroy(gameObject);
    }
}
