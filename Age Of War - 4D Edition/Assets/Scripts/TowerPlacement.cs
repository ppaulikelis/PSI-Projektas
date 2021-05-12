using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    int towerCount = 0;
    // Start is called before the first frame update
    public Values values;

    public void BuildTower(Tower tower)
    {
        if (towerCount < 3) { 
            if (values.gold < tower.cost[towerCount])
            {
                Debug.Log("Not enough gold to buy this upgrade!");
            }

            else
            { 
                values.gold -= tower.cost[towerCount];
                GameObject newTower = new GameObject("Tower Placement", typeof(SpriteRenderer));
                newTower.GetComponent<SpriteRenderer>().sprite = tower.artwork;
                newTower.transform.position = new Vector3(tower.xPlacement[towerCount], tower.yPlacement[towerCount], 0);
                newTower.transform.SetParent(gameObject.transform);

                GameObject turretButton = new GameObject("Button", typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(TurretButton));
                turretButton.transform.SetParent(newTower.transform);
                turretButton.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                turretButton.GetComponent<BoxCollider2D>().isTrigger = true;
                turretButton.transform.localPosition = Vector2.zero;
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
