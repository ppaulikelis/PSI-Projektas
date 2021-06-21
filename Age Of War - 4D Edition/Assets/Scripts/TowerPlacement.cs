using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{

    public Tower tower;
    public Values values;
    public Turret turretData;
    public Text uiTowerCost;
    public GameObject greenBoxPrefab;
    public GameObject redBoxPrefab;

    [HideInInspector]
    public int towerCount = 0;



    // replaces towers and 1: replaces variables for future not owned turrets 2: updates new turret variables for when turrets are sold
    public void ReplaceTowers(Tower newTower)
    {
        this.tower = newTower;
        for (int i = 0; i < towerCount; i++)
        {
            this.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = newTower.artwork;
            this.transform.GetChild(i).position = new Vector3(newTower.xPlacement[i], newTower.yPlacement[i], 0);
        }
    }

    // builds tower and attaches turret, button to it and text (turret controls are disabled till turret is bought)
    public void BuildTower()
    {
        if (towerCount < 3) { 
            if (values.gold < tower.cost[towerCount])
            {
                Debug.Log("Not enough gold to buy this upgrade!");
            }

            else
            { 
                values.gold -= tower.cost[towerCount];

                GameObject newTower = new GameObject("Tower" + towerCount, typeof(SpriteRenderer));     // tower creation
                newTower.GetComponent<SpriteRenderer>().sprite = tower.artwork;
                newTower.transform.position = new Vector3(tower.xPlacement[towerCount], tower.yPlacement[towerCount], 0);
                newTower.transform.SetParent(gameObject.transform);
                towerCount++;

                GameObject greenBox = Instantiate(greenBoxPrefab,newTower.transform);
                greenBox.SetActive(false);
                GameObject redBox = Instantiate(redBoxPrefab, newTower.transform);
                redBox.SetActive(false);


                if (towerCount<3) uiTowerCost.text = tower.cost[towerCount].ToString();
                else uiTowerCost.text = "---";
            }
        }
        else
        {
            Debug.Log("This tower is full upgraded!");
        }
    }
}
