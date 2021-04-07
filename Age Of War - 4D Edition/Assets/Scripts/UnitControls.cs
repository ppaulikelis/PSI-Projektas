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
    private float hitboxIncrease;
    private int movementSpeed;
    private int trainingTime;
    private int cost;

    private SpriteRenderer[] barRenderers;
    private Rigidbody2D rigid;
    private UnitControls enemyScript;
    private GameObject enemyBase;

    private void Start()
    {
        // setting variables from Unit scriptable object
        health = unitData.health;
        damage = unitData.damage;
        hitboxIncrease = unitData.hitboxIncrease;
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

        // adding health bar on top of unit from Resources\Bar
        GameObject temp = (GameObject)Instantiate(Resources.Load("Bar"));
        healthBar = temp.GetComponent<UnitHealthBar>();
        temp.transform.SetParent(gameObject.transform);
        barRenderers = temp.transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer rend in barRenderers)
        {
            rend.enabled = false;
        }

        // setting RigidBody2D as variable as it will be needed in a lot of parts of code
        rigid = gameObject.GetComponent<Rigidbody2D>();

        // set BoxCollider size
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1 + hitboxIncrease, 1 + hitboxIncrease);

        // add coresponding tags
        if(isEnemy)
        {
            gameObject.tag = "Enemy";
        }
        else
        {
            gameObject.tag = "Friendly";
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
        // death
        if(health <= 0)
        {
            Destroy(gameObject);
        }

        // attacking (only works if enemyScript or enemyBase is set after collision)
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

    // changes unit health and modifies health bar
    public void ChangeHealth(int newHealth)
    {
        health = newHealth;
        healthBar.SetHealth(health, unitData.health);
    }

    // IEnumerators to apply cooldown between attacks (waits first) and movement
    IEnumerator ApplyDamage(UnitControls enemy, float cooldown)
    {
        isMoving = false;
        isAttacking = true;
        while(enemy != null)
        {
            yield return new WaitForSeconds(cooldown);
            if (gameObject != null && enemy != null) 
            {
                enemy.ChangeHealth(enemy.health - damage);
            }

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
            yield return new WaitForSeconds(cooldown);
            Base baseScript = enemyBase.GetComponent<Base>();
            baseScript.TakeDamage(damage);
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

    // If collision is detected: a) stop if it was your friendly unit b) stop and fight till death if it was your enemy
    // Works for both sides (player and computer)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float dir = (collision.gameObject.transform.position.x - gameObject.transform.position.x);
        if((dir > 0 && collision.gameObject.CompareTag("Friendly")) || (dir < 0 && collision.gameObject.CompareTag("Enemy")))
        {
            isMoving = false;
        }

        if ((!isEnemy && collision.gameObject.CompareTag("Enemy")) || (isEnemy && collision.gameObject.CompareTag("Friendly")))
        {
            enemyScript = collision.gameObject.GetComponent<UnitControls>();
        }
        if ((!isEnemy && collision.gameObject.CompareTag("Enemy Base")) || (isEnemy && collision.gameObject.CompareTag("Player Base")))
        {
            enemyBase = collision.gameObject;
        }
    }

    // If collision is left: a) if collision was with a friendly unit wait certain ammount of time
    private void OnCollisionExit2D(Collision2D collision)
    {
        float dir = (collision.gameObject.transform.position.x - gameObject.transform.position.x);
        if ((dir > 0 && collision.gameObject.CompareTag("Friendly")) || (dir < 0 && collision.gameObject.CompareTag("Enemy")))
        {
            StartCoroutine(StopAndWaitSeconds(0.5f));
        }
    }
    
    // OnMouse enables/disables healthbars on units when mouse is moved on top 
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
