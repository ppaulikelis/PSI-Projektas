using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    public bool isPlayer;
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        //FOR TESTING
        //if (Input.GetKeyDown(KeyCode.LeftArrow) && isPlayer)
        //{
        //    TakeDamage(20);
        //}
        //else if(Input.GetKeyDown(KeyCode.RightArrow) && !isPlayer)
        //{
        //    TakeDamage(20);
        //}

        if (isPlayer && currentHealth <= 0)
        {
            SceneControl.LoadScene("Loser");
        }
        else if(!isPlayer && currentHealth <= 0)
        {
            SceneControl.LoadScene("Winner");
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
