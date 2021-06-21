using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    public float cameraSpeed;

    private bool goingRight = true;

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > 35) goingRight = false;
        if (transform.position.x < 0) goingRight = true;

        if (goingRight) MoveRight();
        else MoveLeft();
    }

    void MoveRight()
    {
        Vector3 pos = transform.position;
        pos.x += cameraSpeed * Time.deltaTime;
        transform.position = pos;
    }

    void MoveLeft()
    {
        Vector3 pos = transform.position;
        pos.x -= cameraSpeed * Time.deltaTime;
        transform.position = pos;
    }
}
