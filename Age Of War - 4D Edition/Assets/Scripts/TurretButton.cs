using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TurretButton: MonoBehaviour, IPointerClickHandler
{
    public int index = 0;
    public TowerPlacement towerPlacement;
    public Values values;

    void Start()
    {
        AddEventSystem();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        towerPlacement.shouldSpawn = index;
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