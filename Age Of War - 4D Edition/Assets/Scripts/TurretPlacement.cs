using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlacement : MonoBehaviour
{
    [HideInInspector]
    public bool isBuying = false;
    [HideInInspector]
    public bool isSelling = false;

    [Range(0f, 1f)]
    public float penaltyPercent = 0.25f;
    public Turret[] turretsData;

    [HideInInspector]
    public bool[] isBuilt = new bool[3] { false, false, false };
    [HideInInspector]
    public GameObject newTurret;
    [HideInInspector]
    int index;
    Vector2 mousePosition;

    public Values values;

    private void Start()
    {
        values = GameObject.FindObjectOfType<Values>();
    }

    private void Update()
    {
        if(isBuying)    // "building" mode
        {
            isSelling = false;

            mousePosition = Input.mousePosition;    // creates an inactive turret object that follows mouse till it's placed 
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            if(newTurret == null)
            {
                newTurret = new GameObject("Turret", typeof(TurretController), typeof(SpriteRenderer), typeof(Animator));
                SetupTurret(newTurret);
                newTurret.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
            newTurret.transform.position = mousePosition;
        }
        if(isSelling)   // "selling" mode
        {
            isBuying = false;
        }
        SetGreenBoxEnable(isBuying);
        SetRedBoxEnable(isSelling);
    }

    // sets up turret object with values from turretData[index]
    private void SetupTurret(GameObject turret)
    {
        TurretController script = turret.GetComponent<TurretController>();  // script setup
        script.turretData = turretsData[index];
        script.UpdateVariables();
        Transform newShootpoint = new GameObject("Shootpoint").transform;
        newShootpoint.SetParent(turret.transform);
        script.shootPoint = newShootpoint;
        script.shootPoint.localPosition = new Vector2(turretsData[index].shootpointOffsetX, 0);
        script.targetTag = "Enemy";
        script.bullet = turretsData[index].bullet;
        script.enabled = false;
    }

    // turns on all green boxes in spots where turrets can be bought
    public void SetGreenBoxEnable(bool isOn)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if(!isBuilt[i])
            {
                GameObject greenBox = gameObject.transform.GetChild(i).GetChild(0).gameObject;
                if (greenBox.name.Equals("Green Box(Clone)"))
                {
                    greenBox.SetActive(isOn);
                }
            }
        }
    }

    // turns on all red boxes in spots where turrets can be sold
    public void SetRedBoxEnable(bool isOn)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if(isBuilt[i])
            {
                GameObject greenBox = gameObject.transform.GetChild(i).GetChild(1).gameObject;
                if (greenBox.name.Equals("Red Box(Clone)"))
                {
                    greenBox.SetActive(isOn);
                }
            }
        }
    }

    // enters "buying" mode with UI button
    public void BuyButtonClick(int index)
    {
        if(values.gold >= turretsData[index].price)
        {
            this.index = index;
            isBuying = true;
        }
        else
        {
            Debug.Log("Not enough gold to buy turret");
        }
    }

    // enters "selling" mode with UI button
    public void SellButtonClick()
    {
        isSelling = true;
    }
}
