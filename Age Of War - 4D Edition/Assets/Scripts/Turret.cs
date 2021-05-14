using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Turret: ScriptableObject
{
    public Sprite artwork;

    public float range;
    public float fireRate;
    public float force;
    public int damage;

    public float shootpointOffsetX;

    public int price;
}
