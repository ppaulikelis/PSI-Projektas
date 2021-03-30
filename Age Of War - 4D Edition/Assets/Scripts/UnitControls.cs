using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitControls : MonoBehaviour
{
    public Unit unitData;
    public UnitHealthBar healthBar;

    public bool isEnemy;
    public bool isMoving = true;
    public bool canAttack = true;

    private int health;
    private int damage;
    public int movementSpeed;
    private int trainingTime;
    private int cost;

    private SpriteRenderer[] barRenderers;
    private Rigidbody2D rigid;

    private void Start()
    {
        health = unitData.health;
        damage = unitData.damage;
        if (isEnemy)
        {
            movementSpeed = unitData.movementSpeed * -1;
        } 
        else
        {
            movementSpeed = unitData.movementSpeed;
        }     
        trainingTime = unitData.trainingTime;
        cost = unitData.cost;

        GameObject temp = (GameObject)Instantiate(Resources.Load("Bar"));
        healthBar = temp.GetComponent<UnitHealthBar>();
        temp.transform.SetParent(gameObject.transform);

        rigid = gameObject.GetComponent<Rigidbody2D>();

        barRenderers = temp.transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer rend in barRenderers)
        {
            rend.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        // movement
        if (isMoving)
        {
            rigid.velocity = new Vector2(movementSpeed * Time.deltaTime * 50, 0);
        }
        else
        {
            rigid.velocity = Vector2.zero;
        }
    }

    void Update()
    { 
        // temp code for testing
        if (Input.GetKeyDown(KeyCode.W))
        {
            health--;
            healthBar.SetHealth(health, unitData.health);
        }

        // death
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void changeHealth(int newHealth)
    {
        health = newHealth;
        healthBar.SetHealth(health, unitData.health);
    }

    IEnumerator StopAndWaitSeconds(float n)
    {
        isMoving = false;
        yield return new WaitForSeconds(n);
        isMoving = true;
    }

    IEnumerator AttackCooldown(UnitControls enemyScript, float n)
    {
        canAttack = false;
        yield return new WaitForSeconds(n);
        if (health > 0)
        {
            enemyScript.changeHealth(enemyScript.health - damage);
        }
        canAttack = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((!isEnemy && collision.gameObject.tag == "Enemy") || (isEnemy && collision.gameObject.tag == "Friendly"))
        {
            for (int k = 0; k < collision.contacts.Length; k++)
            {
                if (Vector3.Angle(collision.contacts[k].normal, Vector3.left) <= 1)
                {
                    rigid.velocity = Vector2.zero;
                    StartCoroutine(StopAndWaitSeconds(0.5f));
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if((!isEnemy && collision.gameObject.tag == "Enemy") || (isEnemy && collision.gameObject.tag == "Friendly"))
        {
            if(collision.gameObject.GetComponent<UnitControls>() != null)
            {
                if(canAttack)
                {
                    StartCoroutine(AttackCooldown(collision.gameObject.GetComponent<UnitControls>(), 1));
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        foreach(SpriteRenderer rend in barRenderers)
        {
            rend.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        foreach (SpriteRenderer rend in barRenderers)
        {
            rend.enabled = false;
        }
    }
}
