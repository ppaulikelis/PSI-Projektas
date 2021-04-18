using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Unit : ScriptableObject
{
    public Sprite artwork;
    public RuntimeAnimatorController animator;

    public bool isRanged;
    public int health;
    public int damage;
    public float hitboxIncrease;
    public int movementSpeed;
    public int trainingTime;
    public int cost;
}
