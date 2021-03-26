using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    //void Update() { 
    //    if(Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        SceneManager.LoadScene("Start Menu");
    //    }
    //}

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quiting game");
    }
}
