using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    public static LevelLoader instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

	public void LoadScene(int index)
    {
        StartCoroutine(LoadSequence(index));
    }

    public void RestartGame(float delay) 
    {
        StartCoroutine(Reload(delay));
    }

    IEnumerator Reload(float delay)
    {
        yield return new WaitForSeconds(delay);

        int index = SceneManager.GetActiveScene().buildIndex;
        LoadScene(index);
    }

    public void Quit() 
    {
        StartCoroutine(QuitSequence());
    }

    IEnumerator QuitSequence()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        Debug.Log("Quitting...");
        Application.Quit();
    }

    IEnumerator LoadSequence(int index)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        Debug.Log("Loading a new scene...");
        SceneManager.LoadScene(index);
    }
}
