using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Age : ScriptableObject
{
    public Unit[] units;
    public Tower tower;
    public Turret turret;

    public GameObject superAttackObject;

    public Sprite baseSprite;
    public int baseMaxHealth;

    public int experienceNeeded;
}
