using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GraphicsSettings : MonoBehaviour
{
    public TMP_Dropdown Vsync;
    public TMP_Dropdown DisplayMode;
    public TMP_Dropdown Resolutions;
    public TMP_Dropdown Graphics;
    
    Resolution[] AllResolutions;
    int SelectedResolution; 
    List<Resolution> SelectedResolutionList = new List<Resolution>();

    private List<Vector2Int> customResolutions = new List<Vector2Int>
    {
        new Vector2Int(640, 360),   // nHD
        new Vector2Int(854, 480),   // FWVGA
        new Vector2Int(960, 540),   // qHD
        new Vector2Int(1024, 576),  // WSVGA
        new Vector2Int(1280, 720),  // HD/WXGA
        new Vector2Int(1366, 768),  // FWXGA
        new Vector2Int(1600, 900),  // HD+
        new Vector2Int(1920, 1080), // FHD
        new Vector2Int(2048, 1152), // QWXGA
        new Vector2Int(2560, 1440), // QHD
        new Vector2Int(3200, 1800), // WQXGA+
        new Vector2Int(3840, 2160), // UHD
        new Vector2Int(5120, 2880), // UHD+
        new Vector2Int(7680, 4320), // FUHD
        new Vector2Int(15360, 8640),    // QUHD
        new Vector2Int(30720, 17280),   // HHD
        new Vector2Int(61440, 34560),   // FHHD
        new Vector2Int(122880, 69120),  // QHHD
    };


    // Start is called before the first frame update
    void Start()
    {
        // Load saved setting
        int savedVSync = PlayerPrefs.GetInt("VSyncSetting", 0);
        Vsync.value = savedVSync;
        ApplyVSync(savedVSync);

        // Set initial dropdown value based on current screen mode
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                DisplayMode.value = 0; // Fullscreen
                break;
            case FullScreenMode.Windowed:
                DisplayMode.value = 1; // Windowed
                break;
            case FullScreenMode.FullScreenWindow:
                DisplayMode.value = 2; // Borderless Windowed
                break;
        }

        // Add listener for when the dropdown value changes
        DisplayMode.onValueChanged.AddListener(SetDisplayMode);
        Resolutions.onValueChanged.AddListener(SetResolution);
        Vsync.onValueChanged.AddListener(ApplyVSync);

        PopulateResolutions();
    }

    void ApplyVSync(int index)
    {
        if (index == 0) QualitySettings.vSyncCount = 0; // VSync Off
        else if (index == 1) QualitySettings.vSyncCount = 1; // 1 VSync Count
        else if (index == 2) QualitySettings.vSyncCount = 2; // 2 VSync Count

        // Save setting
        PlayerPrefs.SetInt("VSyncSetting", index);
        PlayerPrefs.Save();
    }

    public void SetDisplayMode(int index)
    {
        switch (index)
        {
            case 0: // Fullscreen
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Screen.fullScreen = true;
                break;
            case 1: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.fullScreen = false;
                break;
            case 2: // Borderless Windowed
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.fullScreen = true;
                break;
        }
    }

    void PopulateResolutions()
    {
        Resolutions.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < customResolutions.Count; i++)
        {
            string option = customResolutions[i].x + " x " + customResolutions[i].y;
            options.Add(option);
        }

        Resolutions.AddOptions(options);

        // Set the current resolution as default
        Vector2Int currentRes = new Vector2Int(Screen.width, Screen.height);
        int currentIndex = customResolutions.IndexOf(currentRes);
        Resolutions.value = currentIndex >= 0 ? currentIndex : 0;
        Resolutions.RefreshShownValue();
    }

    void SetResolution(int index)
    {
        Vector2Int selectedRes = customResolutions[index];
        Screen.SetResolution(selectedRes.x, selectedRes.y, Screen.fullScreenMode);
    }


    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(Graphics.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
