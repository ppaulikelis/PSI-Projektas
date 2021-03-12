using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    GameObject unitPrefab;

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

    public void GenerateUnit(GameObject prefab)
    {
        if(!isOnCooldown)
        {
            unitPrefab = Instantiate(prefab, transform.position, transform.rotation);
            UnitControls script = unitPrefab.GetComponent<UnitControls>();
            Unit unitData = script.unit;
            cooldown = unitData.spawnCooldown;
            isOnCooldown = true;
        }
    }
}
