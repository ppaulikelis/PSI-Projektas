using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    GameObject unitObject;
    Unit unitData;

    public bool isOnCooldown = false;
    public float cooldown = 0f;

    public void Update()
    {
        if(isOnCooldown)
        {
            cooldown -= Time.deltaTime;
        }
        if(cooldown <= 0)
        {
            isOnCooldown = false;
        }
        
    }

    public void GenerateUnit(Unit unit)
    {
        if(!isOnCooldown)
        {
            unitObject = new GameObject("Unit",typeof(SpriteRenderer),typeof(UnitControls));
            unitObject.transform.position = transform.position;
     
            unitObject.GetComponent<UnitControls>().unit = unit;
            unitData = unitObject.GetComponent<UnitControls>().unit;

            unitObject.GetComponent<SpriteRenderer>().sprite = unitData.artwork;
            cooldown = (float)unitData.spawnCooldown;
            isOnCooldown = true;
        }
    }
}

public class UnitControls : MonoBehaviour
{
    public Unit unit;
}

