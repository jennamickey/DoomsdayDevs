using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Zombie Templates")]
    [SerializeField] private Transform zombieTypes;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField, Range(0.1f, 1f)] private float crawlerSpawnWeight = 0.75f;

    [Header("Terrain")]
    [SerializeField] private Collider spawnTerrain;

    [Header("Edge Padding")]
    [SerializeField] private float padding = 1f;

    private readonly List<GameObject> regularZombies = new List<GameObject>();
    private GameObject bossZombie;

    private void Awake()
    {
        if (zombieTypes == null)
        {
            GameObject templates = GameObject.Find("Zombie Types");
            if (templates != null)
            {
                zombieTypes = templates.transform;
            }
        }

        if (zombieTypes != null)
        {
            foreach (Transform template in zombieTypes)
            {
                template.gameObject.SetActive(false);
                if (template.name == "Boss Zombie")
                {
                    bossZombie = template.gameObject;
                }
                else
                {
                    regularZombies.Add(template.gameObject);
                }
            }

            // So that we can't destroy the templates
            zombieTypes.gameObject.SetActive(false);
        }
        else if (zombiePrefab != null && zombiePrefab.GetComponent<ZombieHealth>() != null)
        {
            regularZombies.Add(zombiePrefab);
        }
    }

    public int SpawnZombies(int numberToSpawn, RoundController roundController, bool spawnBoss = false)
    {
        if (spawnTerrain == null || regularZombies.Count == 0)
        {
            Debug.LogError("Assign spawn terrain and at least one regular zombie template.", this);
            return 0;
        }
        int spawned = 0;
        for (int i = 0; i < numberToSpawn; i++)
        {
            if (SpawnZombie(SelectRegularZombie(), roundController))
            {
                spawned++;
            }
        }

        if (spawnBoss)
        {
            if (SpawnZombie(bossZombie, roundController))
            {
                spawned++;
            }
        }

        return spawned;
    }

    private GameObject SelectRegularZombie()
    {
        float totalWeight = 0f;
        foreach (GameObject zombie in regularZombies)
        {
            totalWeight += GetSpawnWeight(zombie);
        }

        float selection = Random.value * totalWeight;
        foreach (GameObject zombie in regularZombies)
        {
            selection -= GetSpawnWeight(zombie);
            if (selection < 0f)
            {
                return zombie;
            }
        }

        return regularZombies[regularZombies.Count - 1];
    }

    private float GetSpawnWeight(GameObject zombie)
    {
        return zombie.name.IndexOf("Crawler", System.StringComparison.OrdinalIgnoreCase) >= 0
            ? Mathf.Clamp(crawlerSpawnWeight, 0.1f, 1f)
            : 1f;
    }

    private bool SpawnZombie(GameObject template, RoundController roundController)
    {
        if (template == null)
        {
            Debug.LogError("Missing zombie", this);
            return false;
        }

        Bounds bounds = spawnTerrain.bounds;
        NavMeshAgent templateAgent = template.GetComponent<NavMeshAgent>();
        NavMeshQueryFilter filter = new NavMeshQueryFilter
        {
            agentTypeID = templateAgent != null ? templateAgent.agentTypeID : 0,
            areaMask = templateAgent != null ? templateAgent.areaMask : NavMesh.AllAreas
        };
        Vector3 spawnPosition = default;
        bool foundPosition = false;
        for (int attempt = 0; attempt < 30; attempt++)
        {
            Vector3 candidate = new Vector3(
                Random.Range(bounds.min.x + padding, bounds.max.x - padding),
                bounds.max.y,
                Random.Range(bounds.min.z + padding, bounds.max.z - padding)
            );
            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 10f, filter))
            {
                if (hit.position.x < bounds.min.x + padding || hit.position.x > bounds.max.x - padding ||
                    hit.position.z < bounds.min.z + padding || hit.position.z > bounds.max.z - padding)
                {
                    continue;
                }

                spawnPosition = hit.position;
                foundPosition = true;
                break;
            }
        }

        if (!foundPosition)
        {
            Debug.LogError($"No valid spawnpoint for {template.name}.", this);
            return false;
        }

        GameObject zombie = Instantiate(template, spawnPosition, Quaternion.identity);
        ZombieHealth health = zombie.GetComponent<ZombieHealth>();
        if (health == null)
        {
            health = zombie.AddComponent<ZombieHealth>();
        }
        health.Initialize(roundController);
        zombie.SetActive(true);
        NavMeshAgent spawnedAgent = zombie.GetComponent<NavMeshAgent>();
        if (spawnedAgent != null && (!spawnedAgent.isActiveAndEnabled || !spawnedAgent.isOnNavMesh))
        {
            Debug.LogError($"Spawned {template.name} cannot use the NavMesh; check its agent settings.", this);
            Destroy(zombie);
            return false;
        }
        return true;
    }
}
