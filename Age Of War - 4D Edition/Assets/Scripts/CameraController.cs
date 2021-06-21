using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public float xLimit = 12f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.mousePosition.x >= Screen.width - panBorderThickness && Input.mousePosition.y <= Screen.height - 150 && Input.mousePosition.y >= 80)
        {
            pos.x += panSpeed * Time.deltaTime; 
        }
        if (Input.mousePosition.x <= panBorderThickness && Input.mousePosition.y <= Screen.height - 150 && Input.mousePosition.y >= 80)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, 0, xLimit);

        transform.position = pos;
    }
}
