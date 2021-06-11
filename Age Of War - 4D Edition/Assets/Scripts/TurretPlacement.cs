using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlacement : MonoBehaviour
{
    public bool isBuilding = false;
    public bool isOnGreen = false;
    public bool isSelling = false;
    public Turret[] turretsData;
    public GameObject newTurret;
    public GameObject bullet;

    public int index;
    public Vector2 mousePosition;

    public Values values;

    private void Start()
    {
        values = GameObject.FindObjectOfType<Values>();
    }

    private void Update()
    {
        if(isBuilding)
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if(newTurret == null)
            {
                SetGreenBoxEnable(true);
                newTurret = new GameObject("Turret", typeof(TurretController), typeof(SpriteRenderer), typeof(Animator));
                SetValues(newTurret);
                newTurret.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }

            else if(Input.GetMouseButtonDown(0) && !isOnGreen)
            {
                SetGreenBoxEnable(false);
                isBuilding = false;
                Destroy(newTurret);
                return;
            }
            newTurret.transform.position = mousePosition;
        } 
        else
        {
            newTurret = null;
        }
    }

    private void SetValues(GameObject turret)
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

    public void SetGreenBoxEnable(bool isOn)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject greenBox = gameObject.transform.GetChild(i).GetChild(0).gameObject;
            if(greenBox.name.Equals("Green Box(Clone)"))
            {
                greenBox.SetActive(isOn);
            }
        }
    }

    public void BuildTurret(int index)
    {
        if(values.gold >= turretsData[index].price)
        {
            Debug.Log("byin");
            this.index = index;
            isBuilding = true;
        }
    }

    public void SellTurret()
    {
        isSelling = true;
    }
}
