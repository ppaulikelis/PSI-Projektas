using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitControls : MonoBehaviour
{
    public Unit unitData;
    public UnitHealthBar healthBar;

    private int health;
    private int damage;

    private int movementSpeed;
    private int trainingTime;

    private int cost;

    private void Start()
    {
        health = unitData.health;
        damage = unitData.damage;
        movementSpeed = unitData.movementSpeed;
        trainingTime = unitData.trainingTime;
        cost = unitData.cost;

        GameObject temp = (GameObject)Instantiate(Resources.Load("Bar"));
        healthBar = temp.GetComponent<UnitHealthBar>();
        temp.transform.SetParent(gameObject.transform);
    }

    void Update()  
    {
        gameObject.transform.position += Vector3.right * unitData.movementSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.W))
        {
            health--;
            healthBar.SetHealth(health, unitData.health);
        }

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
