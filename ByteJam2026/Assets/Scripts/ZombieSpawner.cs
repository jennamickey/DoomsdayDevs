using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private int numberToSpawn = 2;

    [Header("Terrain")]
    [SerializeField] private Collider spawnTerrain;

    [Header("Edge Padding")]
    [SerializeField] private float padding = 1f;

    private void Start()
    {
        SpawnZombies();
    }

    private void SpawnZombies()
    {
        Bounds bounds = spawnTerrain.bounds;

        for (int i = 0; i < numberToSpawn; i++)
        {
            float randomX = Random.Range(
                bounds.min.x + padding,
                bounds.max.x - padding
            );

            float randomZ = Random.Range(
                bounds.min.z + padding,
                bounds.max.z - padding
            );

            // Spawn slightly above the terrain.
            Vector3 spawnPosition = new Vector3(
                randomX,
                bounds.max.y + 0.5f,
                randomZ
            );

            Instantiate(
                zombiePrefab,
                spawnPosition,
                Quaternion.identity
            );
        }
    }
}