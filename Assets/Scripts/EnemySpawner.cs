using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float minSpawnDistance = 10;
    public LayerMask whatIsObstacle;
    public LayerMask whatIsGround;

    public float waitTime = 1;
    public int maxAmount = 10;
    public Transform planet;
    public GameObject[] enemyPrefabs;
    public List<EnemyController> enemies = new List<EnemyController>();

    Vector3 spawnPos;

    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        if (enemies.Count < maxAmount)
            StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        GameObject obj = SelectPrefab();

        spawnPos = Random.onUnitSphere * planet.localScale.x;

        GameObject enemy = Instantiate(obj, spawnPos, Quaternion.identity, transform);

        while (Physics.CheckSphere(spawnPos, minSpawnDistance, whatIsObstacle) || !Physics.CheckSphere(spawnPos, minSpawnDistance, whatIsGround) || enemy.GetComponentInChildren<Renderer>().isVisible)
        {
            spawnPos = Random.onUnitSphere * planet.localScale.x;
        }

        enemy.transform.position = spawnPos;
        enemies.Add(enemy.GetComponent<EnemyController>());

        yield return new WaitForSeconds(waitTime);
    }

    GameObject SelectPrefab()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        return enemyPrefabs[index];
    }
}