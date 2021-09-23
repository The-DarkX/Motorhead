using System.Globalization;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [Header("Score")]
    public TMP_Text scoreText;

    public GameObject gameOverCanvas;
    public GameObject speedometerCanvas;
    public RectTransform speedometerArrow;

    [Header("Fuel")]
    public float fuel = 100;
    public float fuelLeakRate = 0.5f;
    public float fuelLeakIncrease = 0.5f;
    public float maxFuel = 120f;

    int score;
    float currentFuelLeakRate;

    GameManager gameManager;

    public static ScoreSystem instance { get; private set; }

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
        gameManager = GameManager.instance;

        speedometerCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);

        currentFuelLeakRate = fuelLeakRate;
    }

    void Update()
    {
        FuelCounter();

        if (gameManager.isGameOn)
        {
            speedometerCanvas.SetActive(true);

            ScoreCounter();
            DecreaseFuel();
        }
        else 
        {
            speedometerCanvas.SetActive(false);
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
            gameManager.GameOver();
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
    public void AddScore(int scoreIncrement)
    {
        int newScore = score + scoreIncrement;

        if (newScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", newScore);
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
}
