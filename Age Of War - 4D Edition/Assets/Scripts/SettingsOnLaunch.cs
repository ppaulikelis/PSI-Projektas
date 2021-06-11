using UnityEngine;
using UnityEngine.Audio;

public class SettingsOnLaunch : MonoBehaviour
{
    public AudioMixer audioMixer;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void SettingsApplier()
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
            audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume") * 10);
        }
        else
        {
            PlayerPrefs.SetInt("musicVolume", 0);
            audioMixer.SetFloat("musicVolume", 0);
        }

        //SFX
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            audioMixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("sfxVolume") * 10);
        }
        else
        {
            PlayerPrefs.SetInt("sfxVolume", 0);
            audioMixer.SetFloat("sfxVolume", 0);
        }
    }
}
