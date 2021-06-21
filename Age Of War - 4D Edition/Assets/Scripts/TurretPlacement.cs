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
    public GameObject[] buttons = new GameObject[4];

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
            SetCancelEnable(true);
            isSelling = false;

            mousePosition = Input.mousePosition;    // creates an inactive turret object that follows mouse till it's placed or canceled
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
            SetCancelEnable(true);
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

    // cancels all actions (buying mode, selling mode)
    public void CancelActions()
    {
        isBuying = false;
        isSelling = false;
        if(newTurret != null)
        {
            TurretController[] allControllers = FindObjectsOfType<TurretController>();
            foreach(var turret in allControllers)
            {
                if(turret.enabled == false)
                {
                    Destroy(turret.gameObject);
                }
            }
            newTurret = null;
        }
        SetCancelEnable(false);
    }

    // toggles buying buttons/cancel button visibility
    void SetCancelEnable(bool isOn)
    {
        for (int i = 0; i < buttons.Length-1; i++)
        {
            buttons[i].SetActive(!isOn);
        }
        buttons[buttons.Length - 1].SetActive(isOn);
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

    public void CancelButtonClick()
    {
        CancelActions();
    }

    public void ReplaceTurrets(Turret newTurret)
    {
        for (int i = 0; i < turretsData.Length; i++)
        {
            turretsData[i] = newTurret;
        }
    }
}
