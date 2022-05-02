using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    GameObject optionsMenu;
    private void Start(){
        optionsMenu = this.transform.parent.Find("OptionsMenu").gameObject;
    }
    public void PlayGame(){
        SceneManager.LoadSceneAsync("Level 1");
    }

    public void PlayTutorial(){
        SceneManager.LoadSceneAsync("Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenOptions(){
        optionsMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void LoadMenu() 
    {
        SceneManager.LoadScene("Menu");
    }

}
