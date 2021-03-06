using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour
{
    public Unit[] currentUnits;

    Queue<Unit> unitsQueue;
    Unit currentUnit;
    public GameObject values;
    public Slider slider;
    public Toggle[] toggles;
    private bool isTraining = false;
    private float cooldown = 0f;

    void Start()
    {
        unitsQueue = new Queue<Unit>();
    }

    void Update()
    {
        if (!isTraining && unitsQueue.Count > 0)  // if queue has units left: set cooldown to its training time, start training
        {
            currentUnit = unitsQueue.Peek();
            cooldown = (float)currentUnit.trainingTime;
            isTraining = true;
        }
   
       if(isTraining) // if training is in progress: reduce cooldown, set slider value, set UI "boxes" values
        {
            cooldown -= Time.deltaTime;

            slider.value = (float)(currentUnit.trainingTime - cooldown) / (float)currentUnit.trainingTime * slider.maxValue;
            for (int i = 0; i < 5; i++)
            {
                if (i < unitsQueue.Count)
                {
                    toggles[i].isOn = true;
                }
                else
                {
                    toggles[i].isOn = false;
                }
            }

            if (cooldown <= 0)  // if training is over: create new GameObject, set its data and sprite, remove unit from queue, reset slider and UI "box" value, end training
            {
                GameObject newObject = new GameObject(currentUnit.name, typeof(UnitControls), typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Animator));
                newObject.transform.position = transform.position;

                newObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                newObject.GetComponent<UnitControls>().unitData = currentUnit;
                newObject.GetComponent<SpriteRenderer>().sprite = currentUnit.artwork;

                unitsQueue.Dequeue();

                toggles[0].isOn = false;
                slider.value = 0;
                isTraining = false;
            }
        }
    }

    public void GenerateUnit(int index) // code to be run when button is pressed
    {
        if(unitsQueue.Count < 5 && values.GetComponent<Values>().gold >= currentUnits[index].cost)
        {
            values.GetComponent<Values>().gold -= currentUnits[index].cost;
            unitsQueue.Enqueue(currentUnits[index]);
        }
    }
}
