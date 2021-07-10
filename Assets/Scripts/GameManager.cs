using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using EZCameraShake;

public class GameManager : MonoBehaviour
{
    [Header("Particles")]
    public GameObject explosionParticles;
    public GameObject catchParticles;

    [Header("Score")]
    public TMP_Text scoreText;

    public GameObject gameOverCanvas;

    public bool useTimer = true;
    public bool isGameOn = false;

    [Header("Fuel")]
    public float fuel = 100;
    public float fuelLeakRate = 0.5f;
    public float maxFuel = 120f;

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

        isGameOn = false;
        scoreText.transform.parent.gameObject.SetActive(false);
        gameOverCanvas.SetActive(false);
    }

    void Update()
    {
        if (isGameOn) 
        {
            if (useTimer)
                ScoreCounter();

            DecreaseFuel();
        }
    }

    void ScoreCounter() 
    {
        if (score > 0)
        {
            scoreText.text = score.ToString("C", CultureInfo.CurrentCulture);
        }
        else 
        {
            score = 0;
            scoreText.text = score.ToString("C", CultureInfo.CurrentCulture);
        }
    }

    IEnumerator Restart(float delay)
    {
        yield return new WaitForSeconds(delay);

        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    public void BeginGame() 
    {
        isGameOn = true;
        scoreText.transform.parent.gameObject.SetActive(true);

        audioManager.PlaySound("MainTheme");
    }

    public void StopGame()
    {
        isGameOn = false;
        scoreText.transform.parent.gameObject.SetActive(false);

        audioManager.StopSound("MainTheme");
    }

    public void GameOver()
    {
        player.gameObject.SetActive(false);
        Instantiate(explosionParticles, player.position, Quaternion.identity);

        StopGame();

        audioManager.PlaySound("Explosion");
        audioManager.PlaySound("GameOver");

        CameraShaker.Instance.ShakeOnce(7, 2, 0, 5);

        gameOverCanvas.SetActive(true);

        StartCoroutine(Restart(4));
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

    void DecreaseFuel()
    {
        if (fuel > 0)
        {
            fuel -= Time.deltaTime * fuelLeakRate;
        }
        else
        {
            fuel = 0;
            GameOver();
        }
    }

    public void AddFuel(float amount)
    {
        if (fuel + amount < maxFuel)
        {
            fuel += amount;
        }
        else
        {
            fuel = maxFuel;
        }
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
