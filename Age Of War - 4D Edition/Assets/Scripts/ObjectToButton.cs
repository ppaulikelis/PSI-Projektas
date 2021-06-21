using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectToButton: MonoBehaviour, IPointerClickHandler//, IPointerEnterHandler, IPointerExitHandler
{
    TurretPlacement turretPlacement;

    void Start()
    {
        turretPlacement = FindObjectOfType<TurretPlacement>();
        AddEventSystem();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (gameObject.tag)
        {
            case "Buy Button":
                TurretController[] controllers = FindObjectsOfType<TurretController>(); // finds all controllers and works with inactive controller (the turret object that follow the mouse)
                for (int i = 0; i < controllers.Length; i++)
                {
                    if (controllers[i].enabled == false)
                    {
                        if(turretPlacement.values.gold >= controllers[i].turretData.price)
                        {
                            turretPlacement.values.gold -= controllers[i].turretData.price; // taking away gold
                            controllers[i].enabled = true;  // enabling turret and moving to middle of tower
                            controllers[i].transform.SetParent(transform.parent);
                            controllers[i].transform.localPosition = Vector2.zero;
                            gameObject.SetActive(false); // turning off green box
                            int builtIndex = int.Parse(transform.parent.name.Substring(5, 1));  // keeping track of built turrets in turretPlacement script bool array isBuilt
                            turretPlacement.isBuilt[builtIndex] = true;
                            break;
                        }
                        else
                        {
                            Destroy(controllers[i].gameObject);
                            Debug.Log("Not enough gold to buy turret");
                        }
                    }
                }
                turretPlacement.CancelActions();
                break;
            case "Sell Button":
                TurretController soldController = transform.parent.GetComponentInChildren<TurretController>();  // finds controller that has to be sold (the one with clicked red box)
                turretPlacement.values.gold += soldController.turretData.price - (int)(soldController.turretData.price * turretPlacement.penaltyPercent);   // take away gold
                Destroy(soldController.gameObject); // destroy sold controller
                gameObject.SetActive(false); // turning off red box
                int sellIndex = int.Parse(transform.parent.name.Substring(5, 1));   // keeping track of built turrets in isBuilt array
                turretPlacement.isBuilt[sellIndex] = false;
                turretPlacement.CancelActions();
                break;
            default:
                Debug.LogError("Not defined action (tag missing in " + gameObject.name+ " object)");
                break;
        }



        /*Values values = turret.values;
        if (turret.isBuying)
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
                        gameObject.SetActive(false);
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
            turret.isBuying = false;
        } */
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