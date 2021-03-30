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
    public bool isAttacking;

    private int health;
    private int damage;
    public int movementSpeed;
    private int trainingTime;
    private int cost;

    private SpriteRenderer[] barRenderers;
    private Rigidbody2D rigid;
    private UnitControls enemyScript;
    private GameObject enemyBase;

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

        if(!isAttacking)
        {
            if(enemyScript != null)
            {
                StartCoroutine(ApplyDamage(enemyScript, 1));
            }
            else if(enemyBase != null)
            {
                StartCoroutine(ApplyDamage(enemyBase, 1));
            }
        }
        
    }

    public void changeHealth(int newHealth)
    {
        health = newHealth;
        healthBar.SetHealth(health, unitData.health);
    }

    IEnumerator ApplyDamage(UnitControls enemy, float cooldown)
    {
        isMoving = false;
        isAttacking = true;
        while(enemy != null && gameObject != null)
        {
            enemy.changeHealth(enemy.health - damage);
            yield return new WaitForSeconds(cooldown);
        }
        isMoving = true;
        isAttacking = false;
        yield return null;
    }

    IEnumerator ApplyDamage(GameObject enemyBase, float cooldown)
    {
        isMoving = false;
        isAttacking = true;
        while (enemyBase != null && gameObject != null)
        {
            Base baseScript = enemyBase.GetComponent<Base>();
            baseScript.currentHealth -= damage;
            baseScript.healthBar.SetHealth(baseScript.currentHealth);
            yield return new WaitForSeconds(cooldown);
        }
        isMoving = true;
        isAttacking = false;
        yield return null;
    }

    IEnumerator StopAndWaitSeconds(float n)
    {
        isMoving = false;
        yield return new WaitForSeconds(n);
        isMoving = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 dir = (collision.gameObject.transform.position - gameObject.transform.position).normalized;
        if((dir.x > 0 && collision.gameObject.tag == "Friendly") || (dir.x < 0 && collision.gameObject.tag == "Enemy"))
        {
            isMoving = false;
        }

        if ((!isEnemy && collision.gameObject.tag == "Enemy") || (isEnemy && collision.gameObject.tag == "Friendly"))
        {
            enemyScript = collision.gameObject.GetComponent<UnitControls>();
        }

        if ((!isEnemy && collision.gameObject.tag == "Enemy Base") || (isEnemy && collision.gameObject.tag == "Player Base"))
        {
            enemyBase = collision.gameObject;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Vector3 dir = (collision.gameObject.transform.position - gameObject.transform.position).normalized;
        if ((dir.x > 0 && collision.gameObject.tag == "Friendly") || (dir.x < 0 && collision.gameObject.tag == "Enemy"))
        {
            StartCoroutine(StopAndWaitSeconds(0.5f));
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
