using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperAttackObject : MonoBehaviour
{
    public float speed = 7.0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, -speed);
    }

    void Update()
    {
        if(transform.position.y < -7.5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            UnitControls enemyScript = collision.collider.GetComponent<UnitControls>();
            enemyScript.TakeDamage(20);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
