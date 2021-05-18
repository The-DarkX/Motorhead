using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Particles")]
    public GameObject explosionParticles;
    public GameObject catchParticles;

    [Header("Score")]
    public TMP_Text scoreText;

    public float decreaseRate = 0.1f;
    public float startingScore = 20f;
    public bool canCount = true;

    float score;

    AudioManager audioManager;

    [HideInInspector] public Transform player;

    public static GameManager instance { get; private set; }

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

    void Start()
    {
        audioManager = AudioManager.instance;
        player = FindObjectOfType<PlayerController>().transform;

        score = startingScore;
    }

    void Update()
    {
        if (canCount)
            Score();
    }

    void Score() 
    {
        if (score > 0)
        {
            score -= decreaseRate * Time.deltaTime;
            scoreText.text = score.ToString("C", CultureInfo.CurrentCulture);
        }
        else 
        {
            score = 0;
            scoreText.text = score.ToString("C", CultureInfo.CurrentCulture);

            GameOver();
        }
    }

    IEnumerator Restart(float delay)
    {
        yield return new WaitForSeconds(delay);

        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    public void GameOver()
    {
        canCount = false;

        player.gameObject.SetActive(false);
        Instantiate(explosionParticles, player.position, Quaternion.identity);

        audioManager.StopSound("MainTheme");
        audioManager.PlaySound("Explosion");
        audioManager.PlaySound("GameOver");
        
        StartCoroutine(Restart(2));
    }

    public void AddScore(float scoreIncrement)
    {
        float newScore = score + scoreIncrement;

        if (newScore > PlayerPrefs.GetFloat("GameScore", 0))
        {
            PlayerPrefs.SetFloat("GameScore", newScore);
        }
        score = newScore;
    }

    public void AddScore(float minScoreIncrement, float maxScoreIncrement)
    {
        float newScore = Random.Range(minScoreIncrement, maxScoreIncrement);

        if (newScore > PlayerPrefs.GetFloat("GameScore", 0))
        {
            PlayerPrefs.SetFloat("GameScore", newScore);
        }

        score = newScore;
    }

    public void SubtractScore(int scoreDecrement)
    {
        if (score - scoreDecrement <= 0)
        {
            score = 0;
        }
        else
        {
            score -= scoreDecrement;
        }
    }

    public void IncreaseDecrement(float increase) 
    {
        decreaseRate += increase;
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
