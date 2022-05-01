using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject pause;
    public bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (pause.activeInHierarchy)
            {
                Time.timeScale = 1f;
                pause.SetActive(false);
                FindObjectOfType<SoundManagerScript>().gameObject.GetComponent<AudioSource>().UnPause();
                isPaused = false;
            }
            else 
            {
                Time.timeScale = 0f;
                pause.SetActive(true);
                FindObjectOfType<SoundManagerScript>().gameObject.GetComponent<AudioSource>().Pause();
                isPaused = true;
            }
        }
    }

    public void LoadMenu() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void ResumeGame() 
    {
        Time.timeScale = 1f;
        pause.SetActive(false);
        isPaused = false;
    }

    public void ResetGame() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 1");
    }
}
