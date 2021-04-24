using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    int i = 0;
    // Start is called before the first frame update
    public Values values;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildTower(Tower tower)
    {

        if (i < 3) { 
            if (values.gold < tower.cost[i])
            {
                Debug.Log("Not enough gold to buy this upgrade!");
                return;
            }

            if (values.gold >= tower.cost[i])
            {
                values.gold -= tower.cost[i];
                GameObject newTower = new GameObject("Tower Placement", typeof(SpriteRenderer));
                newTower.GetComponent<SpriteRenderer>().sprite = tower.artwork;
                newTower.transform.position = new Vector3(tower.xPlacement[i], tower.yPlacement[i], 0);
                if (i < 3)
                    i++;
            }
        }
        else
        {
            Debug.Log("This tower is full upgraded!");
        }

    }
}
