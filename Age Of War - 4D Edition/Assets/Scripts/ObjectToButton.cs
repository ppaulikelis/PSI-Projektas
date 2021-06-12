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


    /*public void OnPointerEnter(PointerEventData eventData)
    {
        turret.isOnGreen = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        turret.isOnGreen = false;
    }*/

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (gameObject.tag)
        {
            case "Buy Button":
                turretPlacement.isBuying = false;
                TurretController[] controllers = FindObjectsOfType<TurretController>();
                for (int i = 0; i < controllers.Length; i++)
                {
                    if (controllers[i].enabled == false)
                    {
                        if(turretPlacement.values.gold >= controllers[i].turretData.price)
                        {
                            turretPlacement.values.gold -= controllers[i].turretData.price;
                            controllers[i].enabled = true;
                            controllers[i].transform.SetParent(transform.parent);
                            controllers[i].transform.localPosition = Vector2.zero;
                            gameObject.SetActive(false);
                            turretPlacement.newTurret = null;
                            int builtIndex = int.Parse(transform.parent.name.Substring(5, 1));
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
                break;
            case "Sell Button":
                turretPlacement.isSelling = false;
                TurretController soldController = transform.parent.GetComponentInChildren<TurretController>();
                turretPlacement.values.gold += soldController.turretData.price - (int)(soldController.turretData.price * turretPlacement.penaltyPercent);
                Destroy(soldController.gameObject);
                int sellIndex = int.Parse(transform.parent.name.Substring(5, 1));
                gameObject.SetActive(false);
                turretPlacement.isBuilt[sellIndex] = false;
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