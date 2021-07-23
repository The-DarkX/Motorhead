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
    public bool canSpawn = true;

    public Transform planet;
    public GameObject[] enemyPrefabs;
    public List<EnemyController> enemies = new List<EnemyController>();

    Vector3 spawnPos;

    void Update()
    {
        if (enemies.Count < maxAmount && canSpawn)
            StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        GameObject obj = SelectPrefab();

        spawnPos = planet.position + (Random.onUnitSphere * planet.localScale.x);

        GameObject enemy = Instantiate(obj, spawnPos, Quaternion.identity, transform);

		for (int i = 0; i < 10; i++)
		{
            if (Physics.CheckSphere(spawnPos, minSpawnDistance, whatIsObstacle) || !Physics.CheckSphere(spawnPos, minSpawnDistance, whatIsGround) || enemy.GetComponentInChildren<Renderer>().isVisible)
            {
                spawnPos = planet.position + Random.onUnitSphere;
            }
            else break;
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