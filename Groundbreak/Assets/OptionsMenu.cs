using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    GameObject mainMenu;
    GameObject volumePercent;

    public AudioMixer audioMixer;
    
    private void Start() {
        mainMenu = this.transform.parent.Find("MainMenu").gameObject;        
        volumePercent = this.transform.Find("Volume").transform.Find("VolumePercent").gameObject;
        
        // Set % to value sound is currently at
        float currVol;
        audioMixer.GetFloat("MasterVolume", out currVol);
        currVol = Mathf.Pow(10, currVol / 20);

        volumePercent.GetComponent<TextMeshProUGUI>().SetText((int)(currVol * 100) + "%");   
        volumePercent.transform.parent.Find("Volume Slider").GetComponent<Slider>().SetValueWithoutNotify(currVol);
    }
    public void setVolume(float rawVolume){  
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(rawVolume) * 20);
        volumePercent.GetComponent<TextMeshProUGUI>().SetText(((int)(rawVolume * 100)).ToString() + "%");
    }

    public void setFullscreen(bool isFullscreen){
        Screen.fullScreen = isFullscreen;
        Debug.Log("Set to " + isFullscreen);
    }

    public void BackToMenu(){
        mainMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
