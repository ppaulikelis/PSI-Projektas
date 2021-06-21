using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject exitMenu;
    public GameObject topPanel;
    public GameObject bottomPanel;
    public GameObject pauseMenuBackground;
    public GameObject backgroundMusic;

    void Start()
    {
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                if(settingsMenu.activeSelf || exitMenu.activeSelf)
                {
                    CloseActiveMenu();
                    OpenMainMenu();
                }
                else Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;

        LeanTween.scale(mainMenu, new Vector3(0.5f, 0.5f, 1), 0.25f).setOnComplete(DisableMainMenu);
        topPanel.SetActive(true);
        bottomPanel.SetActive(true);
        pauseMenuBackground.SetActive(false);

        SetGameIsPaused(false);
        backgroundMusic.GetComponent<AudioSource>().Play();
    }

    public void Pause()
    {
        Time.timeScale = 0f;

        mainMenu.SetActive(true);
        pauseMenuBackground.SetActive(true);
        topPanel.SetActive(false);
        bottomPanel.SetActive(false);
        SetGameIsPaused(true);

        LeanTween.scale(mainMenu, new Vector3(1, 1, 1), 0.25f).setIgnoreTimeScale(true);
        backgroundMusic.GetComponent<AudioSource>().Pause();
    }

    public void SetGameIsPaused(bool var)
    {
        GameIsPaused = var;
    }

    public void OpenMainMenu()
    {
        CloseActiveMenu();
        mainMenu.SetActive(true);
        LeanTween.scale(mainMenu, new Vector3(1, 1, 1), 0.25f).setIgnoreTimeScale(true);
    }

    public void OpenSettingsMenu()
    {
        CloseActiveMenu();
        settingsMenu.SetActive(true);
        LeanTween.scale(settingsMenu, new Vector3(1, 1, 1), 0.25f).setIgnoreTimeScale(true);
    }

    public void OpenExitMenu()
    {
        CloseActiveMenu();
        exitMenu.SetActive(true);
        LeanTween.scale(exitMenu, new Vector3(1, 1, 1), 0.25f).setIgnoreTimeScale(true);
    }

    void CloseActiveMenu()
    {
        if (mainMenu.activeSelf)
        {
            LeanTween.scale(mainMenu, new Vector3(0.5f, 0.5f, 1), 0.25f).setOnComplete(DisableMainMenu).setIgnoreTimeScale(true);
        }
        else if (settingsMenu.activeSelf)
        {
            LeanTween.scale(settingsMenu, new Vector3(0.5f, 0.5f, 1), 0.25f).setOnComplete(DisableSettingsMenu).setIgnoreTimeScale(true);
        }
        else
        {
            LeanTween.scale(exitMenu, new Vector3(0.5f, 0.5f, 1), 0.25f).setOnComplete(DisableExitMenu).setIgnoreTimeScale(true);
        }
    }

    void DisableMainMenu()
    {
        mainMenu.SetActive(false);
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
