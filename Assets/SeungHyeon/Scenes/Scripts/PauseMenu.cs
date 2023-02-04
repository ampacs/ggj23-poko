using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuPanel;
    public GameObject optionPanel;
    bool isOptionMenu = false;

    private void Awake()
    {
        pauseMenuPanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused && isOptionMenu == false)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

   public void Play()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void goBack()
    {
        Time.timeScale = 1.0f;
        optionPanel.SetActive(false);
        isOptionMenu = false;
    }

    public void Option()
    {
        optionPanel.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        isOptionMenu = true;
    }

    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ToMain()
    {
       
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

    }

    public void QuitGame()
    {
       
        Application.Quit();
    }
}
