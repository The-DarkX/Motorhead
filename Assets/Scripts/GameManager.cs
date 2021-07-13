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
    public GameObject speedometerCanvas;
    public RectTransform speedometerArrow;

    public bool useTimer = true;
    public bool isGameOn = false;

    [Header("Fuel")]
    public float fuel = 100;
    public float fuelLeakRate = 0.5f;
    public float fuelLeakIncrease = 0.5f;
    public float maxFuel = 120f;

    float score;
    float currentFuelLeakRate;

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
        speedometerCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);

        currentFuelLeakRate = fuelLeakRate;
    }

    void Update()
    {
        FuelCounter();

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

    void FuelCounter() 
    {
        Quaternion arrowRotation = Quaternion.Euler(0, 0, fuel * (-200 / maxFuel));
        speedometerArrow.rotation = arrowRotation;
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
        speedometerCanvas.SetActive(true);

        audioManager.PlaySound("MainTheme");
        audioManager.PlaySound("CarEngine");
    }

    public void StopGame()
    {
        isGameOn = false;
        speedometerCanvas.SetActive(false);

        audioManager.StopSound("MainTheme");
        audioManager.StopSound("CarEngine");
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

	#region Fuel
	void DecreaseFuel()
    {
        if (fuel > 0)
        {
            fuel -= Time.deltaTime * currentFuelLeakRate;
        }
        else
        {
            fuel = 0;
            GameOver();
        }
    }

    public void AddFuel(float amount)
    {
        currentFuelLeakRate += fuelLeakIncrease;

        if (fuel + amount < maxFuel)
        {
            fuel += amount;
        }
        else
        {
            fuel = maxFuel;
        }
    }
	#endregion

	#region Score
	public void AddScore(float scoreIncrement)
    {
        float newScore = score + scoreIncrement;

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
	#endregion

	#region Scene Management
	public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Quit()
    {
        Application.Quit();
    }
	#endregion
}
