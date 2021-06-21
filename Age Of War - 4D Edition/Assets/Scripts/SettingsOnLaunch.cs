using UnityEngine;
using UnityEngine.Audio;

public class SettingsOnLaunch : MonoBehaviour
{
    public AudioMixer audioMixer;

    void Start()
    {
        //Fullscreen
        if (PlayerPrefs.HasKey("isFullscreen"))
        {
            Screen.fullScreen = PlayerPrefs.GetInt("isFullscreen") == 1 ? true : false;
        }
        else
        {
            PlayerPrefs.SetInt("isFullscreen", 1);
            Screen.fullScreen = true;
        }

        //Music
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            float x = PlayerPrefs.GetFloat("musicVolume");
            audioMixer.SetFloat("musicVolume", (Mathf.Pow(x + 10, 1f / 2f) * 2f - 6.325f) * 10);  // antro laipsnio saknis
            //audioMixer.SetFloat("musicVolume", (Mathf.Pow(x + 10, 1f / 3f) * 3f - 6.463f) * 10);  // trecio laipsnio saknis
        }
        else
        {
            PlayerPrefs.SetInt("musicVolume", -20);
            audioMixer.SetFloat("musicVolume", -20);
        }

        //SFX
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            float x = PlayerPrefs.GetFloat("sfxVolume");
            audioMixer.SetFloat("sfxVolume", (Mathf.Pow(x + 10, 1f / 2f) * 2f - 6.325f) * 10);  // antro laipsnio saknis
            //audioMixer.SetFloat("musicVolume", (Mathf.Pow(x + 10, 1f / 3f) * 3f - 6.463f) * 10);  // trecio laipsnio saknis
        }
        else
        {
            PlayerPrefs.SetInt("sfxVolume", -20);
            audioMixer.SetFloat("sfxVolume", -20);
        }

        SceneControl.LoadScene("Start Menu");
    }
}
