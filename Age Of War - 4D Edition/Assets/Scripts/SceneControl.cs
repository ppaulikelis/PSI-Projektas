using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    void Update() { 
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!SceneManager.GetActiveScene().name.Equals("Game") && !SceneManager.GetActiveScene().name.Equals("Winner") && !SceneManager.GetActiveScene().name.Equals("Loser"))
            {
                SceneManager.LoadScene("Start Menu");
            }
        }
    }

    public static void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quiting game");
    }
}
