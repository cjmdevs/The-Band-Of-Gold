using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    private GameObject spawnedBoss;
    public Transform spawnPoint;
    public LayerMask playerLayer;
    public float detectionRadius = 5f; // Adjust this radius as needed

    AudioManager audioManager;

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (spawnedBoss == null && (playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            // Player entered the area and the boss is not spawned
            audioManager.PlaySFX(audioManager.minoSummon);
            SpawnBoss();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (spawnedBoss != null && (playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            // Player exited the area and the boss is spawned
            DespawnBoss();
        }
    }

    private void SpawnBoss()
    {
        if (bossPrefab != null && spawnPoint != null)
        {
            spawnedBoss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("Boss spawned!");
        }
        else
        {
            Debug.LogError("Boss Prefab or Spawn Point is not assigned in the Inspector.");
        }
    }

    private void DespawnBoss()
    {
        if (spawnedBoss != null)
        {
            Destroy(spawnedBoss);
            spawnedBoss = null;
            Debug.Log("Boss despawned!");
        }
    }

    // Optional: Visualize the detection radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        if (spawnPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(spawnPoint.position, 0.5f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, spawnPoint.position);
        }
    }
}