using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    GameObject mainMenu;
    
    private void Start() {
        mainMenu = this.transform.parent.Find("MainMenu").gameObject;        
    }
    public void setVolume(float volume){
        
    }

    public void BackToMenu(){
        mainMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
