using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private int _numberOfPowerUps = 3;

    [SerializeField] private GameObject _PowerupContainer;

    private bool _stopSpawning = false;
    

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupsRoutine());

    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawnEnemy = new Vector3(Random.Range(-8.0f, 8.0f), 8f, 0);
            
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawnEnemy, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform; // this will keep the hierachy clean

            yield return new WaitForSeconds(5.0f);
        }

    }

    IEnumerator SpawnPowerupsRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawnPowerup = new Vector3(Random.Range(-8.0f, 8.0f), 10f, 0);

            int randomPowerup = Random.Range(0, _numberOfPowerUps);
            GameObject newPowerup = Instantiate(_powerups[randomPowerup], posToSpawnPowerup, Quaternion.identity);

            newPowerup.transform.parent = _PowerupContainer.transform; // keeps the hierachy clean (this isnt needed but looks nice)

            float randomPowerupSpawnTimer = Random.Range(5.0f, 15.0f);

            yield return new WaitForSeconds(randomPowerupSpawnTimer);
        }

    }

    public void OnPlayersDeath(Vector3 position)
    {
        _stopSpawning = true;

        Instantiate(_explosionPrefab, position, Quaternion.identity); // no sound but no one is alive to hear it anyway

        // clear screen
        ClearObjects();

    }

    private void ClearObjects()
    {
        // remove all enemies
        foreach (Transform child in _enemyContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // remove all powerups
        foreach (Transform child in _PowerupContainer.transform)
        {
            Destroy(child.gameObject);
        }

    }

}
