using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    InputManager input;
    GameManager manager;

	private void Start()
	{
        input = InputManager.instance;
        manager = GameManager.instance;
	}

	void Update()
    {
        if (input.pauseButton == true)
        {
            Pause();
        }
        else if (input.pauseButton == false) 
        {
            Resume();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;

        manager.BeginGame();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;

        manager.StopGame();

        //unlock cursor here
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Loading Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
}
