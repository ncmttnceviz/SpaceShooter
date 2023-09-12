using System.Collections;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPreFab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private bool enemySpawning = true;

    private void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (enemySpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject enemy = Instantiate(enemyPreFab, posToSpawn, Quaternion.identity);
            enemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (enemySpawning)
        {
            Vector3 posToSpown = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomId = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[randomId], posToSpown, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }
    
    public void OnPlayerDeath()
    {
        enemySpawning = false;
    }
}