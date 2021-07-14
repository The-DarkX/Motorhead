using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool inSettings = false;

    public GameObject settingsPanel;

    public Animator sceneTransition;
    public float transitionTime = 1.0f;

    public void PlayGame()
    {
        StartCoroutine(PlayGame(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator PlayGame(int levelIndex)
    {
    //Play Animation
        sceneTransition.SetTrigger("ButtonPress");
    //Wait
        yield return new WaitForSeconds(transitionTime);
    //LoadScene
        SceneManager.LoadScene(levelIndex);

    }

    /*public void OpenSettings()
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

    }*/

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

}
