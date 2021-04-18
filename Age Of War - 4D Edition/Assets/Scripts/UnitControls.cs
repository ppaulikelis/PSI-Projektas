using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitControls : MonoBehaviour
{
    public Unit unitData;
    public UnitHealthBar healthBar;

    public bool isEnemy = false;
    private int isEnemyInt;
    public bool isMoving = true;
    public bool isAttacking = false;

    private bool isRanged;
    private int health;
    private int damage;
    private float hitboxIncrease;
    private int movementSpeed;
    private int trainingTime;
    private int cost;

    private SpriteRenderer[] barRenderers;

    //private bool DEBUG;

    private void Start()
    {
        // setting variables from Unit scriptable object
        isRanged = unitData.isRanged;
        health = unitData.health;
        damage = unitData.damage;
        hitboxIncrease = unitData.hitboxIncrease;
        movementSpeed = unitData.movementSpeed;
        trainingTime = unitData.trainingTime;
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

        /*var vision = new GameObject("debug", typeof(SpriteRenderer));
        vision.GetComponent<SpriteRenderer>().sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        vision.transform.SetParent(gameObject.transform);
        vision.transform.localPosition = new Vector2( ((1.01f + hitboxIncrease) / 2 + 5f) * isEnemyInt, 0);
        DEBUG = true;*/
    }

    void Update()
    {
        // death
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        // movement
        if (isMoving)
        {
            transform.Translate(Vector2.right * Time.deltaTime * movementSpeed * isEnemyInt);
        }

        RaycastHit2D closeHit = Physics2D.Raycast(transform.position + new Vector3( (1.01f + hitboxIncrease) / 2 * isEnemyInt, 0, 0), Vector2.right * isEnemyInt, 0.01f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + new Vector3((1.01f + hitboxIncrease) / 2 * isEnemyInt, 0, 0), Vector2.right * isEnemyInt, 5f);

        if (closeHit)
        {
            if (HasHitFriendly(closeHit.collider.tag))
            {
                isMoving = false;
            }
            else if (!isAttacking)
            {
                if (HasHitEnemy(closeHit.collider.tag))
                {
                    StartCoroutine(ApplyDamage(closeHit.collider.GetComponent<UnitControls>(), 1f, false));
                }
                else if (HasHitEnemyBase(closeHit.collider.tag))
                {
                    StartCoroutine(ApplyDamage(closeHit.collider.gameObject, 1f, false));
                }
            }
            else
            {
                isMoving = false;
            }
        }
        else
        {
            isMoving = true;
        }

        if (isRanged && hits.Length > 0)
        {
            foreach (var currentHit in hits)
            {
                if (!isAttacking)
                {
                    if (HasHitEnemy(currentHit.collider.tag))
                    {
                        StartCoroutine(ApplyDamage(currentHit.collider.GetComponent<UnitControls>(), 1f, true));
                    }
                    else if (HasHitEnemyBase(currentHit.collider.tag))
                    {
                        StartCoroutine(ApplyDamage(currentHit.collider.gameObject, 1f, true));
                    }
                    break;
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

    IEnumerator ApplyDamage(UnitControls enemy, float cooldown, bool isRanged)
    {
        isMoving = isRanged;
        isAttacking = true;
        while (enemy != null)
        {
            yield return new WaitForSeconds(cooldown);
            if (gameObject != null && enemy != null)
            {
                enemy.health -= damage;
                enemy.healthBar.SetHealth(enemy.health, enemy.unitData.health);
            }

        }
        isMoving = true;
        isAttacking = false;
        yield return null;
    }

    IEnumerator ApplyDamage(GameObject enemyBase, float cooldown, bool isRanged)
    {
        isMoving = isRanged;
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
