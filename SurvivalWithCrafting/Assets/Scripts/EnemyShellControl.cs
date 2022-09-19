using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShellControl : MonoBehaviour
{
    public enum EnemyState { NORMAL, SHIELD, ATTACK, STUN };
    public EnemyState state;
    private EnemyState lastState;

    private Rigidbody2D rb;
    private Transform target;
    public int lifes, shieldLife;
    private float timeAttack, rateAttack, timeStun;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform; // para recojer algo segun su tag (no tener nada con el mismo tag)
    }

    
    void Update()
    {
        switch (state)
        {
            case EnemyState.SHIELD:
                rb.isKinematic = false;
                if (Vector2.Distance(transform.position, target.position) < 3)
                {
                    if (Vector2.Distance(transform.position, target.position) < 1.4f)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                        timeAttack += Time.deltaTime;
                        if(timeAttack >= rateAttack)
                        {
                            timeAttack = 0;
                            rateAttack = Random.Range(0.4f, 1.3f);
                            state = lastState;
                            state = EnemyState.ATTACK;
                        }
                    }
                    else
                    {
                        timeAttack = 0;
                        Vector2 finalPos = new Vector2(target.position.x, transform.position.y);
                        Vector2 dir = finalPos - (Vector2)transform.position;
                        dir.Normalize();
                        if (Mathf.Abs(finalPos.x - transform.position.x) > 0.2)
                            rb.velocity = new Vector2(dir.x * 8, rb.velocity.y);
                        else
                            rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                break;
            case EnemyState.NORMAL:
                rb.isKinematic = false;
                break;
            case EnemyState.ATTACK:
                rb.isKinematic = true;
                GetComponent<SpriteRenderer>().color = Color.red; //enrealidad esto seria la animación
                timeAttack += Time.deltaTime;
                if (timeAttack >= 0.5f)
                {
                    timeAttack = 0;
                    state = lastState;
                    GetComponent<SpriteRenderer>().color = Color.white;
                }
                break;
            case EnemyState.STUN:
                rb.isKinematic = true;
                GetComponent<SpriteRenderer>().color = Color.green;
                timeStun += Time.deltaTime;
                if(timeStun >= 2)
                {
                    timeStun = 0;
                    state = lastState;
                    GetComponent<SpriteRenderer>().color = Color.white;
                }
                break;
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            GetDamage();
        }
        //print(rb.velocity.x);
    }

    public void GetDamage()
    {
        if(lastState == EnemyState.NORMAL)
        {
            lifes -= 5;
            if (lifes <= 0)
            {
                Destroy(gameObject);
            }
        }
        else if(lastState == EnemyState.SHIELD)
        {
            if(state == EnemyState.ATTACK)
            {
                state = EnemyState.STUN;
            }
            else if(state == EnemyState.STUN)
            {
                if(shieldLife > 0)
                {
                    shieldLife -= 5;
                    if(shieldLife <= 0)
                    {
                        lastState = EnemyState.NORMAL;
                    }
                }
                else
                {
                    lifes -= 5;
                    if(lifes <= 0)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
