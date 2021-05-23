using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage { get; set; }
    public string targetTag { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -7.5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals(targetTag))
        {
            UnitControls targetScript = collider.GetComponent<UnitControls>();
            targetScript.TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
