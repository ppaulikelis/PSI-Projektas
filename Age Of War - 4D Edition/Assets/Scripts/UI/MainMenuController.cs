using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject settingsMenu;
    public GameObject exitMenu;
    public GameObject difficultyMenu;

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
            if (settingsMenu.activeSelf || exitMenu.activeSelf || difficultyMenu.activeSelf)
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

    public void OpenDifficultytMenu()
    {
        CloseActiveMenu();
        difficultyMenu.SetActive(true);
        LeanTween.scale(difficultyMenu, new Vector3(1, 1, 1), 0.25f);
    }

    public void OpenSettingsMenu()
    {
        CloseActiveMenu();
        settingsMenu.SetActive(true);
        LeanTween.scale(settingsMenu, new Vector3(1, 1, 1), 0.25f);
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
        else if (difficultyMenu.activeSelf)
        {
            LeanTween.scale(difficultyMenu, new Vector3(0.5f, 0.5f, 1), 0.25f).setOnComplete(DisableDifficultyMenu);
        }
        else if (settingsMenu.activeSelf)
        {
            LeanTween.scale(settingsMenu, new Vector3(0.5f, 0.5f, 1), 0.25f).setOnComplete(DisableSettingsMenu);
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

    void DisableDifficultyMenu()
    {
        difficultyMenu.SetActive(false);
    }

    void DisableSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }

    void DisableExitMenu()
    {
        exitMenu.SetActive(false);
    }
}
