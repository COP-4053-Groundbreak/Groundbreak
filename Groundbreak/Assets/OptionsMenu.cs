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
        float adjVolume = 1 - (currVol * -1) / 80.0f;
        volumePercent.GetComponent<TextMeshProUGUI>().SetText((int)(adjVolume * 100) + "%");   
        // Set slider to value sound is currently at
        volumePercent.transform.parent.Find("Volume Slider").GetComponent<Slider>().SetValueWithoutNotify(currVol);
    }
    public void setVolume(float rawVolume){
        float adjVolume = 1 - (rawVolume * -1) / 80.0f;
        volumePercent.GetComponent<TextMeshProUGUI>().SetText((int) (adjVolume * 100) + "%");   
        audioMixer.SetFloat("MasterVolume", rawVolume);
    }

    public void BackToMenu(){
        mainMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
