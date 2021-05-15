using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgeChanger : MonoBehaviour
{
    public Age[] ages;
    public int currentIndex = 0;
    public Values values;

    public GameObject playerBase;
    public GameObject unitSpawner;
    public GameObject superAttack;
    public GameObject towerPlacement;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(values.experience >= ages[currentIndex].experienceNeeded)
            {
                currentIndex++;

                Age currentAge = ages[currentIndex];

                playerBase.GetComponent<SpriteRenderer>().sprite = currentAge.baseSprite;
                playerBase.GetComponent<Base>().ChangeMaxHealth(currentAge.baseMaxHealth);

                unitSpawner.GetComponent<UnitSpawner>().currentUnits = currentAge.units;

                superAttack.GetComponent<SuperAttack>().superAttackObjectPrefab = currentAge.superAttackObject;

                towerPlacement.GetComponent<TowerPlacement>().ReplaceTowers(currentAge.tower, currentAge.turret);
            }
        }
    }
}
