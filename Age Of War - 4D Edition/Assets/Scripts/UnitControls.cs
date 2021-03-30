using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitControls : MonoBehaviour
{
    public Unit unitData;
    public UnitHealthBar healthBar;

    public bool isMoving;

    private int health;
    private int damage;
    private int movementSpeed;
    private int trainingTime;
    private int cost;

    private SpriteRenderer[] barRenderers;
    private Rigidbody2D rigid;

    private void Start()
    {
        health = unitData.health;
        damage = unitData.damage;
        movementSpeed = unitData.movementSpeed;
        trainingTime = unitData.trainingTime;
        cost = unitData.cost;

        isMoving = true;

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

    void Update()  
    {
        // movement
        if(isMoving)
        {
            rigid.velocity = Vector2.right * unitData.movementSpeed * Time.deltaTime * 500;
        }
        else
        {
            rigid.velocity = Vector2.zero;
        }
       

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

    IEnumerator StopAndWaitSeconds(float n)
    {
        isMoving = false;
        yield return new WaitForSeconds(n);
        isMoving = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
