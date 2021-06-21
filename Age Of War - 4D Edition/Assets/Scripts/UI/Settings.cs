using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Toggle fullscreenToggle;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Start()
    {
        //Fulscreen
        fullscreenToggle.isOn = PlayerPrefs.GetInt("isFullscreen") == 1 ? true : false;

        //Music
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");

        //SFX
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }

    public void SetMusicVolume(float volume)
    {
        float x = volume;
        audioMixer.SetFloat("musicVolume", (Mathf.Pow(x + 10, 1f / 2f) * 2f - 6.325f) * 10);  // antro laipsnio saknis
        //audioMixer.SetFloat("musicVolume", (Mathf.Pow(x+10, 1f / 3f) * 3f - 6.463f) * 10);  // trecio laipsnio saknis
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        float x = volume;
        audioMixer.SetFloat("sfxVolume", (Mathf.Pow(x + 10, 1f / 2f) * 2f - 6.325f) * 10);  // antro laipsnio saknis
        //audioMixer.SetFloat("musicVolume", (Mathf.Pow(x+10, 1f / 3f) * 3f - 6.463f) * 10);  // trecio laipsnio saknis
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("isFullscreen", isFullscreen == true ? 1 : 0);
    }
}
