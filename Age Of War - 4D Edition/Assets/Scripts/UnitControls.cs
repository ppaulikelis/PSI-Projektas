using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitControls : MonoBehaviour
{
    public Unit unitData;
    public UnitHealthBar healthBar;

    public Values values;
    public EnemyAI enemyAI;

    public bool isEnemy = false;
    private int isEnemyInt;
    public bool isMoving = true;
    public bool isAttacking = false;

    private Animator animator;
    public float lastWalk = 0;

    private bool isRanged;
    private int health;
    private int damage;
    private float hitboxIncrease;
    private float movementSpeed;
    private float trainingTime;
    private int cost;
    private int rewardGold;
    private int rewardExperience;

    private SpriteRenderer[] barRenderers;

    private void Start()
    {
        // setting variables from Unit scriptable object
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = unitData.animator;
        isRanged = unitData.isRanged;
        health = unitData.health;
        damage = unitData.damage;
        hitboxIncrease = unitData.hitboxIncrease;
        movementSpeed = unitData.movementSpeed;
        trainingTime = unitData.trainingTime;
        cost = unitData.cost;
        rewardGold = unitData.rewardGold;
        rewardExperience = unitData.rewardExperience;
        if (isEnemy)
        {
            isEnemyInt = -1;
            gameObject.tag = "Enemy";
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            isEnemyInt = 1;
            gameObject.tag = "Friendly";
        }

        // increase hitbox size
        GetComponent<BoxCollider2D>().size = new Vector3(1 + hitboxIncrease, 1 + hitboxIncrease, 1);

        // adding health bar on top of unit from Resources\Bar
        GameObject temp = (GameObject)Instantiate(Resources.Load("Bar"));
        healthBar = temp.GetComponent<UnitHealthBar>();
        temp.transform.SetParent(gameObject.transform);
        barRenderers = temp.transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer rend in barRenderers)
        {
            rend.enabled = false;
        }

        values = FindObjectOfType<Values>();
        enemyAI = FindObjectOfType<EnemyAI>();
    }

    void Update()
    {
        // animations
        animator.SetBool("Attacking", isAttacking);
        if(lastWalk <= 0)
        {
            lastWalk = 0.05f;
            animator.SetBool("Walking", isMoving);
        }
        if(lastWalk > 0)
        {
            lastWalk -= Time.deltaTime;
        }
     
      

        // death
        if (health <= 0)
        {
            if(gameObject.tag.Equals("Enemy")) // code to give player gold/experience when unit is killed
            {
                values.gold += rewardGold;
                values.experience += rewardExperience;
            }
            else if(gameObject.tag.Equals("Friendly"))  // gives enemy AI gold and experience when killing player units
            {
                enemyAI.gold += rewardGold;
                enemyAI.experience += rewardExperience;
            }

            Destroy(gameObject);
        }

        // movement
        if (isMoving)
        {
            transform.Translate(Vector2.right * Time.deltaTime * movementSpeed * isEnemyInt);
        }
     

        // combat
        RaycastHit2D closeHit = Physics2D.Raycast(transform.position + new Vector3( (1.01f + hitboxIncrease) / 2 * isEnemyInt, 0, 0),
            Vector2.right * isEnemyInt, 0.1f);  // hardcoded value 0.1f can be changed to change melee attack range
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + new Vector3((1.01f + hitboxIncrease) / 2 * isEnemyInt, 0, 0),
            Vector2.right * isEnemyInt, 5f);    // hardcoded value 5f can be changed to change ranged attack range
        if (closeHit)   // if hit in melee range happened:
        {
            if (HasHitFriendly(closeHit.collider.tag))  // A: if it was a friendly unit, stop movement
            {
                isMoving = false;
            }
            else if (!isAttacking) // B: else it is some sort of enemy
            {
                if (HasHitEnemy(closeHit.collider.tag)) // B1: if it was enemy, hit it
                {
                    StartCoroutine(ApplyDamage(closeHit.collider.GetComponent<UnitControls>(), 1f, false));
                }
                else if (HasHitEnemyBase(closeHit.collider.tag))    // B2: if it was enemy base, hit it
                {
                    StartCoroutine(ApplyDamage(closeHit.collider.gameObject, 1f, false));
                }
                else if(HasHitOwnBase(closeHit.collider.tag) && !isMoving)  // B edge case: unit is stuck inside own base, start movement
                {
                    isMoving = true;
                }
            }
            else // B edge case: if is attacking already but hit an enemy - stop movement
            {
                isMoving = false;
            } 
        }
        else // A2: if it was a friendly unit but it's no longer in range, start movement
        {
            isMoving = true;
        }

        if (isRanged && hits.Length > 0)    // C: if unit is ranged deal with attacking over long range
        {
            foreach (var currentHit in hits)    // run trough all units seen by ray
            {
                if (!isAttacking)
                {
                    if (HasHitEnemy(currentHit.collider.tag))   // attack first visible enemy
                    {
                        StartCoroutine(ApplyDamage(currentHit.collider.GetComponent<UnitControls>(), 1f, true));
                        break;
                    }
                    else if (HasHitEnemyBase(currentHit.collider.tag)) // attack first visible enemy base
                    {
                        StartCoroutine(ApplyDamage(currentHit.collider.gameObject, 1f, true));
                        break;
                    }
                }
            }
        }
    }

    private bool HasHitFriendly(string tag)
    {
        return tag.Equals("Friendly") && !isEnemy || tag.Equals("Enemy") && isEnemy;
    }

    private bool HasHitEnemy(string tag)
    {
        return tag.Equals("Enemy") && !isEnemy || tag.Equals("Friendly") && isEnemy;
    }

    private bool HasHitEnemyBase(string tag)
    {
        return tag.Equals("Enemy Base") && !isEnemy || tag.Equals("Player Base") && isEnemy;
    }

    private bool HasHitOwnBase(string tag)
    {
        return tag.Equals("Player Base") && !isEnemy || tag.Equals("Enemy Base") && isEnemy;
    }

    IEnumerator ApplyDamage(UnitControls enemy, float cooldown, bool shouldMove)
    {
        isMoving = shouldMove;
        isAttacking = true;
        while (enemy != null)
        {
            yield return new WaitForSeconds(cooldown);
            if (gameObject != null && enemy != null)
            {
                enemy.health -= damage;
                enemy.healthBar.SetHealth(enemy.health, enemy.unitData.health);
                enemy.animator.SetTrigger("Damaged");
            }
    
        }
        isMoving = true;
        isAttacking = false;
        yield return null;
    }

    IEnumerator ApplyDamage(GameObject enemyBase, float cooldown, bool shouldMove)
    {
        isMoving = shouldMove;
        isAttacking = true;
        while (gameObject != null && enemyBase != null)
        {
            yield return new WaitForSeconds(cooldown);
            Base baseScript = enemyBase.GetComponent<Base>();
            baseScript.TakeDamage(damage);
        }
        isMoving = true;
        isAttacking = false;
        yield return null;
    }

    // can be used to take damage from outside sources (super attacks, turrets)
    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health, unitData.health);
        //animator.SetTrigger("Damaged");
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
