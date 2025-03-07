using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;
    public TMP_Dropdown ResolutionDropdown;
    public Slider masterVol, musicVol, sfxVol;
    public AudioMixer mainAudioMixer;

    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }

    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterVol", masterVol.value);
    }

    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("MusicVol", musicVol.value);
    }

    public void ChangeSfxVolume()
    {
        mainAudioMixer.SetFloat("SfxVol", sfxVol.value);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
