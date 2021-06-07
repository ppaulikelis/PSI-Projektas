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
    public float movementSpeed;
    public float trainingTime;
    public int cost;

    public int rewardGold;
    public int rewardExperience;
}
