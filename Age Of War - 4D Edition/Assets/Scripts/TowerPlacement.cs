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
    public int shouldSpawn;   // -1: no turret buying, 0,1,2: buy turret indexed 0,1,2
    int towerCount = 0;

    private void Start()
    {
        shouldSpawn = -1;
    }

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
                    tc[shouldSpawn].GetComponentInChildren<MeshRenderer>().enabled = false;
                    tc[shouldSpawn].transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    tc[shouldSpawn].enabled = true;
                }
            }
            shouldSpawn = -1;
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

    public void BuildTurretWithButton(int index)
    {
        TurretController[] tc = this.GetComponentsInChildren<TurretController>();
        if(values.gold >= turretData.price && tc.Length > index && tc[index].enabled == false)
        {
            values.gold -= turretData.price;
            tc[index].transform.GetComponent<TurretButton>().enabled = false;
            tc[index].GetComponentInChildren<MeshRenderer>().enabled = false;
            tc[index].transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            tc[index].enabled = true;
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

                GameObject text = new GameObject("Price Text", typeof(MeshRenderer), typeof(TextMesh));
                TextMesh textMesh = text.GetComponent<TextMesh>();
                textMesh.characterSize = 0.05f;
                textMesh.fontSize = 100;
                textMesh.text = turretData.price.ToString();
                textMesh.color = new Color32(255, 215, 0, 255);
                text.transform.SetParent(turretButton.transform);
                text.transform.localPosition = Vector2.zero;
      

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

    private void SetValues(GameObject turret)
    {
        TurretController script = turret.GetComponent<TurretController>();  // setting up TurretController script using turretData
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

        TurretButton turrButScript = turret.GetComponent<TurretButton>();   // setting up fake turret button (not using UI)
        turrButScript.values = values;
        turrButScript.towerPlacement = this;
        turrButScript.index = towerCount;
    }
}
