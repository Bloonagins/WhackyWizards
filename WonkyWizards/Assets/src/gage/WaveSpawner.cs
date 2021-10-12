using UnityEngine;
[System.Serializable]

public class Wave
{
    public string waveName;
    public int numberOfEnemies;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;
}

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPoints;

    private Wave currentWave;
    private int currentWaveNumber;
    private bool canSpawn = true;
    private float nextSpawnTime, currentSpawnTime;
    //private float timer;

    private void Start()
    {
        currentSpawnTime = 0.0f; // timer to track when the next enemy spawns
        nextSpawnTime = 3.0f; // countdown in seconds till next enemy spawn
    }

    private void FixedUpdate()
    {
        currentWave = waves[currentWaveNumber];

        if (currentSpawnTime > 0)
        {
            currentSpawnTime -= Time.fixedDeltaTime; // decrement the timer
        }
        else
        {
            SpawnWave();
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !canSpawn && currentWaveNumber+1 != waves.Length)
            {
                currentWaveNumber++;
                canSpawn = true;
            }
            currentSpawnTime += nextSpawnTime; // increment the time
        } 
    }

    void SpawnWave()
    {
        if(canSpawn) 
        {
            //GameObject randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];
            //Transform randomPoint = spawnPoints[randomEnemy.Range(0, spawnPoints.Length)];
            Instantiate(currentWave.typeOfEnemies[0], spawnPoints[0].position, Quaternion.identity);
            nextSpawnTime = 3.0f;
            currentWave.numberOfEnemies--;
            //nextSpawnTime -= Time.fixedDeltaTime;
            if(currentWave.numberOfEnemies == 0)
            {
                canSpawn = false;
            }
        }
    }
}

/// Use this to comment functions