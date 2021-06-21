using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgeChanger : MonoBehaviour
{
    public Age[] ages;
    public int currentIndex = 0;
    public Values values;

    public GameObject playerBase;
    public GameObject unitSpawner;
    public GameObject superAttack;
    public GameObject towerPlacement;
    public GameObject turretPlacement;

    public Image unit1Icon;
    public Text unit1Price;
    public Image unit2Icon;
    public Text unit2Price;
    public Image unit3Icon;
    public Text unit3Price;
    public Image turret1Icon;
    public Text turret1Price;
    public Image turret2Icon;
    public Text turret2Price;
    public Image turret3Icon;
    public Text turret3Price;
    public Image towerIcon;
    public Text towerPrice;
    public Image specialAttackIcon1;
    public Image specialAttackIcon2;
    public Image specialAttackIcon3;
    public Text evolvePrice;


    private void Start()
    {
        if (playerBase == null) GameObject.FindGameObjectWithTag("Player Base");
        if (unitSpawner == null) GameObject.Find("Player Unit Spawner");
        if (superAttack == null) GameObject.Find("Super Attack Spawner");
        if (towerPlacement == null) GameObject.Find("Tower/Turret Placement");
        if (turretPlacement == null) GameObject.Find("Tower/Turret Placement");
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

            towerPlacement.GetComponent<TowerPlacement>().ReplaceTowers(currentAge.tower);
            turretPlacement.GetComponent<TurretPlacement>().ReplaceTurrets(currentAge.turret);

            //UI
            //Units
            unit1Icon.sprite = currentAge.units[0].artwork;
            unit1Price.text = currentAge.units[0].cost.ToString();
            unit2Icon.sprite = currentAge.units[1].artwork;
            unit2Price.text = currentAge.units[1].cost.ToString();
            unit3Icon.sprite = currentAge.units[2].artwork;
            unit3Price.text = currentAge.units[2].cost.ToString();
            //Turrets
            turret1Icon.sprite = currentAge.turret.artwork;
            turret1Price.text = currentAge.turret.price.ToString();
            turret2Icon.sprite = currentAge.turret.artwork;
            turret2Price.text = currentAge.turret.price.ToString();
            turret3Icon.sprite = currentAge.turret.artwork;
            turret3Price.text = currentAge.turret.price.ToString();
            //Towers
            towerIcon.sprite = currentAge.tower.artwork;
            int index = towerPlacement.GetComponent<TowerPlacement>().towerCount;
            if (index < 3) towerPrice.text = currentAge.tower.cost[index].ToString();
            else towerPrice.text = "---";

            //Specials
            specialAttackIcon1.sprite = currentAge.superAttackObject.GetComponent<SpriteRenderer>().sprite;
            specialAttackIcon2.sprite = currentAge.superAttackObject.GetComponent<SpriteRenderer>().sprite;
            specialAttackIcon3.sprite = currentAge.superAttackObject.GetComponent<SpriteRenderer>().sprite;
            try
            {
                evolvePrice.text = ages[currentIndex + 1].experienceNeeded.ToString();
            }
            catch(Exception e)
            {
                evolvePrice.text = "---";
                Debug.Log("End of Ages" + e.ToString());
            }
        }
    }
}
