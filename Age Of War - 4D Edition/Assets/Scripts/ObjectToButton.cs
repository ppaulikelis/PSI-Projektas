using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectToButton: MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    TurretPlacement turret;

    void Start()
    {
        turret = FindObjectOfType<TurretPlacement>();
        AddEventSystem();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        turret.isOnGreen = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        turret.isOnGreen = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Values values = turret.values;
        if (turret.isBuilding)
        {
            TurretController[] controllers = FindObjectsOfType<TurretController>();
            foreach(var contr in controllers)
            {
                if(contr.enabled == false)
                {
                    if(values.gold >= contr.turretData.price)
                    {
                        values.gold -= contr.turretData.price;
                        contr.enabled = true;
                        contr.transform.SetParent(transform.parent);
                        contr.transform.localPosition = Vector2.zero;
                        Destroy(gameObject);
                        turret.SetGreenBoxEnable(false);
                        break;
                    }
                    else
                    {
                        turret.SetGreenBoxEnable(false);
                        Destroy(contr.gameObject);
                        Debug.Log("Not enough gold to buy turret");
                    }
                }
              
            }

            turret.isOnGreen = false;
            turret.isBuilding = false;
        } 
    }

    void AddEventSystem()
    {
        GameObject eventSystem = null;
        GameObject tempObj = GameObject.Find("EventSystem");
        if (tempObj == null)
        {
            eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
        else
        {
            if ((tempObj.GetComponent<EventSystem>()) == null)
            {
                tempObj.AddComponent<EventSystem>();
            }

            if ((tempObj.GetComponent<StandaloneInputModule>()) == null)
            {
                tempObj.AddComponent<StandaloneInputModule>();
            }
        }
    }

}