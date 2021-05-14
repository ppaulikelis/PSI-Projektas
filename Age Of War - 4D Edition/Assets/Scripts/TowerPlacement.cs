using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{

    public Tower tower;
    public Values values;
    public Turret turretData;
    public GameObject bullet;

    [Range(0f, 1f)]
    public float penaltyPercent = 0.5f;

    [HideInInspector]
    public int shouldSpawn = -1;   // -1: no turret buying, 0,1,2: buy turret indexed 0,1,2
    int towerCount = 0;


    void Update()
    {
        if(shouldSpawn > -1)
        {
            BuyTurret(shouldSpawn);
            shouldSpawn = -1;
        }
    }

    // buys tower if conditions are met at index (where 0-bottom,1-middle,2-top)
    public void BuyTurret(int index)
    {
        if(index < 0 || index > 2)
        {
            Debug.Log("Wrong index in BuyTurret(index)");
            return;
        }
        TurretController[] tc = this.GetComponentsInChildren<TurretController>();   // gets all turretControllers (even if they're not enabled)
        if(values.gold >= turretData.price && tc.Length > index && tc[index].enabled == false)
        {
            // add gold, remove "click-icon-to-buy", remove price text, make it fully visible, enable it
            values.gold -= turretData.price;
            tc[index].transform.GetComponent<TurretButton>().enabled = false;
            tc[index].GetComponentInChildren<MeshRenderer>().enabled = false;
            tc[index].transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            tc[index].enabled = true;
        }
    }

    // sells highest tower and gives back gold with penalty
    public void SellTurret()
    {
        TurretController[] tc = this.GetComponentsInChildren<TurretController>();
        if(tc.Length > 0)
        {
            for (int i = tc.Length-1; i >= 0; i--)
            {
                if (tc[i].enabled == true)
                {
                    tc[i].enabled = false; 
                    tc[i].GetComponentInChildren<MeshRenderer>().enabled = true;
                    tc[i].transform.GetComponent<TurretButton>().enabled = true;
                    tc[i].transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                    values.gold += turretData.price - (int)(turretData.price * penaltyPercent);
                    break;
                }
            }
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

                GameObject turretButton = new GameObject("Button", typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(TurretButton), typeof(TurretController));  // turret creation
                turretButton.transform.SetParent(newTower.transform);
                SetValues(turretButton);

                GameObject text = new GameObject("Price Text", typeof(MeshRenderer), typeof(TextMesh)); // price text creation
                TextMesh textMesh = text.GetComponent<TextMesh>();
                textMesh.characterSize = 0.05f;
                textMesh.fontSize = 100;
                textMesh.text = turretData.price.ToString();
                textMesh.color = new Color32(255, 215, 0, 255);
                text.transform.SetParent(turretButton.transform);
                text.transform.localPosition = Vector2.zero;

                towerCount++;  
            }
        }
        else
        {
            Debug.Log("This tower is full upgraded!");
        }
    }

    // sets all values for TurretController script using turretData, adds "click-icon-to-buy" function, changes visual cues to see if turret is bought
    private void SetValues(GameObject turret)
    {
        TurretController script = turret.GetComponent<TurretController>();  // script setup
        script.turretData = turretData;
        script.UpdateVariables();
        Transform newShootpoint = new GameObject("Shootpoint").transform;
        newShootpoint.SetParent(turret.transform);
        script.shootPoint = newShootpoint;
        script.shootPoint.localPosition = new Vector2(turretData.shootpointOffsetX, 0);
        script.targetTag = "Enemy";
        script.bullet = bullet;
        script.enabled = false;
       
        turret.transform.localPosition = Vector2.zero;  // moving turret to middle of tower block

        SpriteRenderer rend = turret.GetComponent<SpriteRenderer>();    // setting up sprite (half transparent, sorting order)
        rend.sprite = turretData.artwork;
        rend.color = new Color(1, 1, 1, 0.5f);
        rend.sortingOrder = towerCount + 1;
        turret.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
        turret.GetComponent<BoxCollider2D>().isTrigger = true;

        TurretButton turrButScript = turret.GetComponent<TurretButton>();   // setting up fake turret button ("click-icon-to-buy" without using UI buttons)
        turrButScript.values = values;
        turrButScript.towerPlacement = this;
        turrButScript.index = towerCount;
    }
}
