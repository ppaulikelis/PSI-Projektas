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

    private void Start()
    {
        if (playerBase == null) GameObject.FindGameObjectWithTag("Player Base");
        if (unitSpawner == null) GameObject.Find("Player Unit Spawner");
        if (superAttack == null) GameObject.Find("Super Attack Spawner");
        if (towerPlacement == null) GameObject.Find("Tower Placement");
    }

    // Changes various age related variables
    public void ChangeAge()
    {
        if (currentIndex < ages.Length-1 && values.experience >= ages[currentIndex + 1].experienceNeeded)
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
