using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public static bool popUpIsOpened = false;
    public GameObject popUp;

    public void Open()
    {
        popUp.SetActive(true);
        popUpIsOpened = true;
    }

    public void Close()
    {
        popUp.SetActive(false);
        popUpIsOpened = false;
    }
}
