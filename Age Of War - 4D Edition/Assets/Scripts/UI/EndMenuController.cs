using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenuController : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject exitMenu;

    private void Awake()
    {
        Time.timeScale = 1f;
        LeanTween.reset();
    }

    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(startMenu, new Vector3(1, 1, 1), 0.25f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitMenu.activeSelf)
            {
                CloseActiveMenu();
                OpenStartMenu();
            }
            else
            {
                CloseActiveMenu();
                OpenExitMenu();
            }
        }
    }

    public void OpenStartMenu()
    {
        CloseActiveMenu();
        startMenu.SetActive(true);
        LeanTween.scale(startMenu, new Vector3(1, 1, 1), 0.25f);
    }

    public void OpenExitMenu()
    {
        CloseActiveMenu();
        exitMenu.SetActive(true);
        LeanTween.scale(exitMenu, new Vector3(1, 1, 1), 0.25f);
    }

    void CloseActiveMenu()
    {
        if (startMenu.activeSelf)
        {
            LeanTween.scale(startMenu, new Vector3(0.5f, 0.5f, 1), 0.25f).setOnComplete(DisableStartMenu);
        }
        else
        {
            LeanTween.scale(exitMenu, new Vector3(0.5f, 0.5f, 1), 0.25f).setOnComplete(DisableExitMenu);
        }
    }

    void DisableStartMenu()
    {
        startMenu.SetActive(false);
    }

    void DisableExitMenu()
    {
        exitMenu.SetActive(false);
    }
}
