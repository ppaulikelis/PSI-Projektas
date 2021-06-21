using System;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public bool isPlayer;
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;
    public Text value;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        value.text = string.Format("{0} / {1}", currentHealth, maxHealth);
        if(isPlayer)
        {
            gameObject.tag = "Player Base";
        }
        else
        {
            gameObject.tag = "Enemy Base";
        }
    }

    private void Update()
    {
        if (isPlayer && currentHealth <= 0)
        {
            SceneControl.LoadScene("Loser");
        }
        else if(!isPlayer && currentHealth <= 0)
        {
            SceneControl.LoadScene("Winner");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        value.text = string.Format("{0} / {1}", currentHealth, maxHealth);
    }

    public void ChangeMaxHealth(int maxHealth)
    {
        float increaseRatio = maxHealth / this.maxHealth;
        this.maxHealth = maxHealth;
        healthBar.SetMaxHealth(this.maxHealth);
        this.currentHealth = (int)(this.currentHealth * increaseRatio);
        healthBar.SetHealth(this.currentHealth);
        value.text = string.Format("{0} / {1}", currentHealth, maxHealth);
    }
}
