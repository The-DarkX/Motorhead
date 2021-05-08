using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;

    public float decreaseRate = 0.1f;

    float score;

    bool counterStarted = false;

    AudioManager audioManager;

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
    }

    void Update()
    {
        Score();
    }

    void Score() 
    {
        if (score > 0)
        {
            counterStarted = true;
            score -= decreaseRate * Time.deltaTime;
            scoreText.text = score.ToString("C", CultureInfo.CurrentCulture);
        }
        else 
        {
            score = 0;
            scoreText.text = score.ToString("C", CultureInfo.CurrentCulture);

            if (counterStarted)
                Restart();
        }
    }

    public void Restart()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    public void GameOver()
    {
        print("Game Over");
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
