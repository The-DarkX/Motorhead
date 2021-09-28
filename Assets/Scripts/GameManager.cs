using UnityEngine;
using TMPro;
using EZCameraShake;

public class GameManager : MonoBehaviour
{
    [Header("Particles")]
    public GameObject explosionParticles;
    public GameObject catchParticles;
    public GameObject refueledParticles;

    [Header("Version")]
    public TMP_Text versionText;

    AudioManager audioManager;

    public PlayFabManager playFabManager;

    [HideInInspector] public Transform player;

    public static GameManager instance { get; private set; }

    public bool isGameOn = false;

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

        versionText.text = "v" + Application.version;
    }

    void Start()
    {
        audioManager = AudioManager.instance;

        if (FindObjectOfType<PlayerController>() != null)
            player = FindObjectOfType<PlayerController>().transform;
    }

    public void BeginGame() 
    {
        isGameOn = true;

        audioManager.PlaySound("Music");
        audioManager.PlaySound("CarEngine");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StopGame()
    {
        isGameOn = false;

        audioManager.StopSound("Music");
        audioManager.StopSound("CarEngine");
    }

    public void GameOver()
    {
        //PlayFabManager.instance.SendLeaderboard(ScoreSystem.instance.score);

        if (player != null)
            player.gameObject.SetActive(false);

        Instantiate(explosionParticles, player.position, Quaternion.identity);

        StopGame();

        audioManager.PlaySound("Explosion");
        audioManager.PlaySound("GameOver");

        CameraShaker.Instance.ShakeOnce(7, 2, 0, 5);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        LevelLoader.instance.LoadScene(0);
    }

    public void ResetScore() 
    {
        PlayerPrefs.SetInt("HighScore", 0);
    }

    public void ShowHighScore(TMP_Text scoreText) 
    {
        scoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
}