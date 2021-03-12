using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Unit : ScriptableObject
{
    public Sprite artwork;

    public int health;
    public int damage;

    public int movementSpeed;
    public float spawnCooldown;

    public int cost;
}
