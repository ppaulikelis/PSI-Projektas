using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Tower : ScriptableObject
{
    public Sprite artwork;
    public int cost;
    public float yPlacement;
    public float xPlacement;
    public bool isBuilt;
}
