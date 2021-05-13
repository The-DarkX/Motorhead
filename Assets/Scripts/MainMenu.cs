using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool inSettings = false;

    public GameObject settingsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void OpenSettings()
    {
        if(inSettings == false)
        {
            settingsPanel.SetActive(true);
            inSettings = true;
        }
    }

    public void CloseSettings()
    {
        if (inSettings == true)
        {
            settingsPanel.SetActive(false);
            inSettings = false;
        }

    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
