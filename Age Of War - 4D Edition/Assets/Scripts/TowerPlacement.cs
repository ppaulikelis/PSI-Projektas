using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    int towerCount = 0;
    public Tower tower;
    public Values values;
    public Turret turretData;
    public GameObject bullet;

    public int shouldSpawn = -1;

    void Update()
    {
        if(shouldSpawn > -1)
        {
            if (values.gold >= turretData.price)
            {
                values.gold -= turretData.price;

                TurretController[] tc = this.GetComponentsInChildren<TurretController>();
                if (tc[shouldSpawn].enabled == false)
                {
                    tc[shouldSpawn].transform.GetComponent<TurretButton>().enabled = false;
                    tc[shouldSpawn].transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    tc[shouldSpawn].transform.GetComponent<SpriteRenderer>().sortingOrder = towerCount + 2;
                    tc[shouldSpawn].enabled = true;
                }
            }
            shouldSpawn = -1;
        }
    }

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
                GameObject newTower = new GameObject("Tower" + towerCount, typeof(SpriteRenderer));
                newTower.GetComponent<SpriteRenderer>().sprite = tower.artwork;
                newTower.transform.position = new Vector3(tower.xPlacement[towerCount], tower.yPlacement[towerCount], 0);
                newTower.transform.SetParent(gameObject.transform);

                GameObject turretButton = new GameObject("Button", typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(TurretButton), typeof(TurretController));
                TurretController script = turretButton.GetComponent<TurretController>();
                script.turretData = turretData;
                script.UpdateVariables();
                Transform newShootpoint = new GameObject("Shootpoint").transform;
                newShootpoint.SetParent(turretButton.transform);
                script.shootPoint = newShootpoint;
                script.shootPoint.localPosition = new Vector2(turretData.shootpointOffsetX,0);
                script.targetTag = "Enemy";
                script.bullet = bullet;
                script.enabled = false;
                turretButton.transform.SetParent(newTower.transform);
                turretButton.transform.localPosition = Vector2.zero;
                turretButton.GetComponent<SpriteRenderer>().sprite = turretData.artwork;
                turretButton.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                turretButton.GetComponent<SpriteRenderer>().sortingOrder = towerCount+1;
                turretButton.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                turretButton.GetComponent<BoxCollider2D>().isTrigger = true;
                turretButton.GetComponent<TurretButton>().values = values;
                turretButton.GetComponent<TurretButton>().towerPlacement = this;
                turretButton.GetComponent<TurretButton>().index = towerCount;


                if (towerCount < 3) 
                {
                    towerCount++;
                }    
            }
        }
        else
        {
            Debug.Log("This tower is full upgraded!");
        }

    }
}
