using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject pause;
    public bool isPaused = false;
    [SerializeField] HoldPlayerStats playerStats;
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
        playerStats.playerPassiveInventory = null;
        playerStats.playerConsumableInventory = null;
        playerStats.playerActiveItem = null;
        Time.timeScale = 1f;
        Seed.playerInput = false;
        SceneManager.LoadScene("Menu");
    }
    public void QuitGame()
    {
        playerStats.playerPassiveInventory = null;
        playerStats.playerConsumableInventory = null;
        playerStats.playerActiveItem = null;
        Time.timeScale = 1f;
        Seed.playerInput = false;
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

        playerStats.playerPassiveInventory = null;
        playerStats.playerConsumableInventory = null;
        playerStats.playerActiveItem = null;
        Seed.playerInput = false;
        SceneManager.LoadScene("Level 1");
    }

    private void OnApplicationQuit()
    {
        playerStats.playerPassiveInventory = null;
        playerStats.playerConsumableInventory = null;
        playerStats.playerActiveItem = null;
    }

}
