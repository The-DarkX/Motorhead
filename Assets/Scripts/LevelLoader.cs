using System.Collections;
using System.Collections.Generic;
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
        StartCoroutine(Load(index));
    }

    IEnumerator Load(int index)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index);
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
}
