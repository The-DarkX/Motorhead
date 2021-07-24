using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EZCameraShake;

public class GameManager : MonoBehaviour
{
    [Header("Particles")]
    public GameObject explosionParticles;
    public GameObject catchParticles;
    public GameObject refueledParticles;

    AudioManager audioManager;

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
    }

    public void StopGame()
    {
        isGameOn = false;

        audioManager.StopSound("Music");
        audioManager.StopSound("CarEngine");
    }

    public void GameOver()
    {
        if (player != null)
            player.gameObject.SetActive(false);

        Instantiate(explosionParticles, player.position, Quaternion.identity);

        StopGame();

        audioManager.PlaySound("Explosion");
        audioManager.PlaySound("GameOver");

        CameraShaker.Instance.ShakeOnce(7, 2, 0, 5);

        LevelLoader.instance.LoadScene(0);
    }

	
}