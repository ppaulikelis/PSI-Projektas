using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
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
        if (values.gold >= tower.cost && !tower.isBuilt)
        {
            values.gold -= tower.cost;
            GameObject newTower = new GameObject("Tower Placement", typeof(SpriteRenderer));
            newTower.GetComponent<SpriteRenderer>().sprite = tower.artwork;
            newTower.transform.position = new Vector3(-12, tower.yPlacement, 0);
            tower.isBuilt = true;
        }
        else if (tower.isBuilt)
        {
            Debug.Log("This tower is already built!");
        }
        else
        {
            Debug.Log("Not enough gold to buy this upgrade!");
        }
    }

}
