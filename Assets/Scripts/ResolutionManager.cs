using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private float currentRefreshRate;
    private int currentResolutionIndex = 0; 
    

    void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;
        foreach (Resolution res in resolutions)
        {
            if(res.refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(res);
            }
        }

        List<string> options = new List<string>();
        int i = 0;
        foreach(Resolution res in filteredResolutions)
        {
            string newOption = res.width + "x" + res.height + " " + res.refreshRate + "Hz";
            options.Add(newOption);
            if(res.width == Screen.width && res.height == Screen.height)
            {
                currentResolutionIndex = i;
            }
            i++;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void setRes(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            Screen.SetResolution(resolution.width, resolution.height, true);
        else if (Screen.fullScreenMode == FullScreenMode.Windowed)
            Screen.SetResolution(resolution.width, resolution.height, false);

    }
    public void setCurrentResolutionIndex()
    {
     

    }
    void Update()
    {
        
    }
}
