using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Unit : ScriptableObject
{
    public float health;
    public float damage;

    public float movementSpeed;
    public float spawnCooldown;

    public int cost;
}
