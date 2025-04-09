using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public int maxEnemiesTotal = 10;
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 5f;

    [Header("Spawn Points")]
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    [Header("Advanced Settings")]
    public bool spawnOnStart = true;
    public bool spawnContinuously = true;
    public Transform playerTransform; // Optional: To spawn relative to the player
    public bool spawnRelativePlayer = false;
    public Vector3 spawnOffset = Vector3.zero; // Offset relative to player or origin.

    private int currentEnemyCount = 0;
    private float nextSpawnTime;
    private List<GameObject> activeEnemies = new List<GameObject>();

    [System.Serializable]
    public class SpawnPoint
    {
        public Transform transform;
        public int maxEnemiesPerPoint = 3;
        public float spawnRadius = 5f; // Customizable radius per spawn point
        [HideInInspector] public int currentEnemiesAtPoint = 0;
        
        [Header("Enemy Types for this Spawn Point")]
        public bool useSpecificEnemies = false;
        public List<int> allowedEnemyIndices = new List<int>(); // Indices from the main enemyPrefabs list
    }

    void Start()
    {
        if (spawnOnStart)
        {
            SetNextSpawnTime();
        }
        else
        {
            nextSpawnTime = Mathf.Infinity;
        }
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && currentEnemyCount < maxEnemiesTotal && spawnContinuously)
        {
            SpawnEnemy();
            SetNextSpawnTime();
        }

        activeEnemies.RemoveAll(item => item == null);
        currentEnemyCount = activeEnemies.Count;

        foreach(SpawnPoint sp in spawnPoints)
        {
            sp.currentEnemiesAtPoint = 0;
        }

        foreach(GameObject enemy in activeEnemies)
        {
            foreach(SpawnPoint sp in spawnPoints)
            {
                if(Vector3.Distance(enemy.transform.position, sp.transform.position) <= sp.spawnRadius)
                {
                    sp.currentEnemiesAtPoint++;
                }
            }
        }
    }

    void SpawnEnemy()
    {
        // Check if there are any enemy prefabs to spawn
        if (enemyPrefabs.Count == 0)
            return;

        SpawnPoint selectedSpawnPoint = null;

        List<SpawnPoint> availableSpawnPoints = new List<SpawnPoint>();
        foreach (SpawnPoint sp in spawnPoints)
        {
            if (sp.currentEnemiesAtPoint < sp.maxEnemiesPerPoint)
            {
                availableSpawnPoints.Add(sp);
            }
        }

        if (availableSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            selectedSpawnPoint = availableSpawnPoints[randomIndex];
        }

        if (selectedSpawnPoint != null)
        {
            Vector2 randomCircleOffset = Random.insideUnitCircle * selectedSpawnPoint.spawnRadius; // Use radius from SpawnPoint
            Vector3 spawnPosition = selectedSpawnPoint.transform.position + new Vector3(randomCircleOffset.x, randomCircleOffset.y, 0);

            if (spawnRelativePlayer && playerTransform != null)
            {
                Vector2 playerOffset = new Vector2(playerTransform.position.x, playerTransform.position.y);
                Vector3 playerPosition = new Vector3(playerOffset.x, playerOffset.y, 0);
                spawnPosition = playerPosition + new Vector3(randomCircleOffset.x, randomCircleOffset.y, 0) + spawnOffset;
            }
            else
            {
                spawnPosition = selectedSpawnPoint.transform.position + new Vector3(randomCircleOffset.x, randomCircleOffset.y, 0) + spawnOffset;
            }

            // Select an enemy prefab based on spawn point restrictions
            GameObject selectedEnemyPrefab;
            
            if (selectedSpawnPoint.useSpecificEnemies && selectedSpawnPoint.allowedEnemyIndices.Count > 0)
            {
                // Select from specified enemy types for this spawn point
                int randomAllowedIndex = Random.Range(0, selectedSpawnPoint.allowedEnemyIndices.Count);
                int enemyIndex = selectedSpawnPoint.allowedEnemyIndices[randomAllowedIndex];
                
                // Validate index is within range
                if (enemyIndex >= 0 && enemyIndex < enemyPrefabs.Count)
                {
                    selectedEnemyPrefab = enemyPrefabs[enemyIndex];
                }
                else
                {
                    // Fallback to random enemy if index is out of range
                    selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                    Debug.LogWarning("Enemy index out of range for spawn point. Using random enemy instead.");
                }
            }
            else
            {
                // Use any random enemy from the full list
                int randomEnemyIndex = Random.Range(0, enemyPrefabs.Count);
                selectedEnemyPrefab = enemyPrefabs[randomEnemyIndex];
            }
            
            GameObject newEnemy = Instantiate(selectedEnemyPrefab, spawnPosition, Quaternion.identity);
            activeEnemies.Add(newEnemy);
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }

    public void TriggerSpawn()
    {
        if (currentEnemyCount < maxEnemiesTotal)
        {
            SpawnEnemy();
        }
    }

    public void ChangeMaxEnemies(int newMax)
    {
        maxEnemiesTotal = newMax;
    }

    public void ChangeSpawnRate(float newMin, float newMax)
    {
        minSpawnTime = newMin;
        maxSpawnTime = newMax;
        SetNextSpawnTime();
    }

    public void ClearEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        activeEnemies.Clear();
        currentEnemyCount = 0;
        foreach(SpawnPoint sp in spawnPoints)
        {
            sp.currentEnemiesAtPoint = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (spawnPoints != null)
        {
            foreach (SpawnPoint sp in spawnPoints)
            {
                if (sp.transform != null)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(sp.transform.position, sp.spawnRadius);
                }
            }
        }
    }
}