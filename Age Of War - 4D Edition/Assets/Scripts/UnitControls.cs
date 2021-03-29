using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControls : MonoBehaviour
{
    public Unit unitData;
    void Update()   // temporary code to test movement
    {
        gameObject.transform.position += Vector3.right * unitData.movementSpeed * Time.deltaTime;

        if (gameObject.transform.position.x >= 25)
        {
            Destroy(gameObject);
        }
    }
}
