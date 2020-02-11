using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] powerUps;

    private bool _stopSpawn = false;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
    }

    private void Update()
    {
    }

    private IEnumerator SpawnEnemy()
    {
        while (!_stopSpawn)
        {
            Vector3 spawnPoint = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0); ;
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPoint, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    private IEnumerator SpawnPowerUp()
    {
        while (!_stopSpawn)
        {
            Vector3 spawnPoint = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(powerUps[randomPowerUp], spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(2.0f, 10.0f));
        }
    }

    public void PlayerDeath()
    {
        _stopSpawn = true;
    }
}