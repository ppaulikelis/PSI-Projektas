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
    public bool isAnimating = false;

    private Animator animator;
    private UnitControls closeFriendlyController;

    private bool isRanged;
    private int health;
    private int damage;
    private float hitboxIncrease;
    private float movementSpeed;
    private int rewardGold;
    private int rewardExperience;

    private SpriteRenderer[] barRenderers;

    GameObject enemyBase;
    UnitControls enemyUnit;

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
        if (isMoving)   // move left/right
        {
            transform.Translate(Vector2.right * Time.deltaTime * movementSpeed * isEnemyInt);
        }
        if (closeFriendlyController != null)    // if hit slower friendly unit, match speed till it's dead
        {
            movementSpeed = closeFriendlyController.movementSpeed;
        }
        else
        {
            movementSpeed = unitData.movementSpeed;
        }

        // combat
        RaycastHit2D closeHit = Physics2D.Raycast(transform.position + new Vector3( (1.01f + hitboxIncrease) / 2 * isEnemyInt, 0, 0),
            Vector2.right * isEnemyInt, 0.1f);  // hardcoded value 0.1f can be changed to change melee attack range
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + new Vector3((1.01f + hitboxIncrease) / 2 * isEnemyInt, 0, 0),
            Vector2.right * isEnemyInt, 5f);    // hardcoded value 5f can be changed to change ranged attack range
        isMoving = !isAttacking;
        if (closeHit)   // if hit in melee range happened:
        {
            if (HasHitFriendly(closeHit.collider.tag))  // A1: if it was a friendly unit, stop movement
            {
                closeFriendlyController = closeHit.collider.GetComponent<UnitControls>();
                isMoving = false;
            }
            else if (HasHitEnemy(closeHit.collider.tag)) // B1: if it was enemy, hit it
            {
                SetupDamageDealing();
                enemyUnit = closeHit.collider.GetComponent<UnitControls>();
            }
            else if (HasHitEnemyBase(closeHit.collider.tag))    // B2: if it was enemy base, hit it
            {
                SetupDamageDealing();
                enemyBase = closeHit.collider.gameObject;
            }
            else if(HasHitOwnBase(closeHit.collider.tag) && !isMoving)  // B edge case: unit is stuck inside own base, start movement
            {
                isMoving = true;
            }
        }
        else // A2: if it was a friendly unit but it's no longer in range, start movement
        {
            isMoving = true;
        }

        if (isRanged && hits.Length > 0)    // C: if unit is ranged deal with attacking over long range
        {
            if(isAttacking && enemyBase != null)    // if unit is attacking base and enemy unit is spawned in the gap, prioritize unit death
            {
                foreach (var currentHit in hits)
                {
                    if (HasHitEnemy(currentHit.collider.tag))
                    { 
                        enemyBase = null;
                        StopAllCoroutines();
                        break;
                    }
                }
            }
        
            foreach (var currentHit in hits)    // run trough all units seen by ray
            {
                if (HasHitEnemy(currentHit.collider.tag))   // attack first visible enemy
                {
                    SetupDamageDealing();
                    enemyUnit = currentHit.collider.GetComponent<UnitControls>();
                    break;
                }
                else if (HasHitEnemyBase(currentHit.collider.tag)) // attack first visible enemy base
                {
                    SetupDamageDealing();
                    enemyBase = currentHit.collider.gameObject;
                    break;
                }

                if (isAttacking)    // if unit is already attacking, stop movement and exit loop 
                {
                    isMoving = false;
                    break;
                }
            }
        }

        // animations
        animator.SetBool("Attacking", isAnimating);
        animator.SetBool("Walking", isMoving);
    }

    // checks for hit types
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

    // applies damage to enemy unit every "cooldown" seconds (cooldown should be animation length - 1 second)
    IEnumerator ApplyDamage(UnitControls enemy, float cooldown)
    {
        isMoving = false;
        isAttacking = true;
        while (gameObject != null && enemy != null)
        {
            enemy.health -= damage;
            enemy.healthBar.SetHealth(enemy.health, enemy.unitData.health);
            if (gameObject != null && enemy != null)
            {
                yield return new WaitForSeconds(cooldown);
            }
        }
        isMoving = true;
        isAttacking = false;
        isAnimating = false;
        yield return null;
    }

    // applies damage to enemy base every "cooldown" seconds (cooldown should be animation length - 1 second)
    IEnumerator ApplyDamage(GameObject enemyBase, float cooldown)
    {
        isMoving = false;
        isAttacking = true;
        while (gameObject != null && enemyBase != null)
        {
            Base baseScript = enemyBase.GetComponent<Base>();
            baseScript.TakeDamage(damage);
            if (gameObject != null && enemyAI != null)
            {
                yield return new WaitForSeconds(cooldown);
            }
        }
        isMoving = true;
        isAttacking = false;
        isAnimating = false;
        yield return null;
    }

    // can be used to take damage from outside sources (super attacks, turrets)
    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health, unitData.health);
    }

    // actions triggered in middle of attack animation (can be used to apply damage, particle effects, sounds)
    public void AnimationAttack() 
    {
        if(!isAttacking)
        {
            if (enemyUnit != null)
            { 
                StartCoroutine(ApplyDamage(enemyUnit, 1f));
            }
            else if (enemyBase != null)
            {
                StartCoroutine(ApplyDamage(enemyBase, 1f));
            }
        }
    }  

    // repetetive method to set variables
    private void SetupDamageDealing()
    {
        isAttacking = false;
        isMoving = false;
        isAnimating = true;
        StopAllCoroutines();
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
