using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitControls : MonoBehaviour
{
    public Unit unitData;
    public UnitHealthBar healthBar;

    public bool isEnemy;
    public int isEnemyInt;
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
        trainingTime = unitData.trainingTime;
        movementSpeed = unitData.movementSpeed;
        cost = unitData.cost;
        if (isEnemy)
        {
            isEnemyInt = -1;
            gameObject.tag = "Enemy";
        }
        else
        {
            isEnemyInt = 1;
            gameObject.tag = "Friendly";
        }

        // adding health bar on top of unit from Resources\Bar
        GameObject temp = (GameObject)Instantiate(Resources.Load("Bar"));
        healthBar = temp.GetComponent<UnitHealthBar>();
        temp.transform.SetParent(gameObject.transform);
        barRenderers = temp.transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer rend in barRenderers)
        {
            rend.enabled = false;
        }
    }

    void Update()
    {
        // death
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (!isAttacking)
        {
            if (enemyScript != null)
            {
                StartCoroutine(ApplyDamage(enemyScript, 1));
            }
            else if (enemyBase != null)
            {
                StartCoroutine(ApplyDamage(enemyBase, 1));
            }
        }


        if (isMoving)
        {
            transform.Translate(Vector2.right * Time.deltaTime * movementSpeed * isEnemyInt);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.51f * isEnemyInt,0,0), Vector2.right*isEnemyInt, 0.5f);

        if(hit)
        {
            if(hit.collider.tag == "Friendly" && !isEnemy || hit.collider.tag == "Enemy" && isEnemy)
            {
                isMoving = false;
                //StartCoroutine(WaitBeforeMoving(0.5f));
            }

            if(hit.collider.tag == "Enemy" && !isEnemy || hit.collider.tag == "Friendly" && isEnemy)
            {
                isMoving = false;
                enemyScript = hit.collider.gameObject.GetComponent<UnitControls>();
            }

            if(hit.collider.tag == "Enemy Base" && !isEnemy || hit.collider.tag == "Player Base" && isEnemy)
            {
                isMoving = false;
                enemyBase = hit.collider.gameObject;
            }
        } 
        else
        {
            isMoving = true;
        }
    }

    public void ChangeHealth(int newHealth)
    {
        health = newHealth;
        healthBar.SetHealth(health, unitData.health);
    }

    IEnumerator ApplyDamage(UnitControls enemy, float cooldown)
    {
        isMoving = false;
        isAttacking = true;
        while (enemy != null)
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

    /*IEnumerator WaitBeforeMoving(float seconds)
    {
        isMoving = false;
        yield return new WaitForSeconds(seconds);
        isMoving = true;
    }*/

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
