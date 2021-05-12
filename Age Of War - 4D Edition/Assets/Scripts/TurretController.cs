using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float range;
    public Transform target;
    public string targetTag;
    Vector2 turretDirection;
    public GameObject bullet;
    public float fireRate;
    float nextTimeToFire = 0;
    public Transform shootPoint;
    public float force;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        if (targetTag.Equals("Friendly"))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            shootPoint.localPosition = new Vector3(shootPoint.localPosition.x * -1, shootPoint.localPosition.y * -1, shootPoint.localPosition.z);
        }
    }

    void UpdateTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestTarget = null;

        foreach (GameObject target in targets)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
            if(distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                nearestTarget = target;
            }
        }

        if (nearestTarget != null && shortestDistance <= range)
        {
            target = nearestTarget.transform;
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        GameObject bulletIns = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        bulletIns.GetComponent<Bullet>().targetTag = targetTag;
        bulletIns.GetComponent<Bullet>().damage = damage;
        if (targetTag.Equals("Friendly"))
        {
            bulletIns.GetComponent<Rigidbody2D>().AddForce(turretDirection * force);
        }
        else bulletIns.GetComponent<Rigidbody2D>().AddForce(turretDirection * force);
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            turretDirection = (Vector2)target.position - (Vector2)transform.position;
            if (targetTag.Equals("Friendly"))
            {
                transform.right = turretDirection * -1;
            }
            else transform.right = turretDirection;
            if(Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }
}
