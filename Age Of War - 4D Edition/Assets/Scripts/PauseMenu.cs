using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
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
                if (!PopUp.popUpIsOpened)
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SetGameIsPaused(false);
        backgroundMusic.GetComponent<AudioSource>().Play();
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        SetGameIsPaused(true);
        backgroundMusic.GetComponent<AudioSource>().Pause();
    }

    public void SetGameIsPaused(bool var)
    {
        GameIsPaused = var;
    }
}
