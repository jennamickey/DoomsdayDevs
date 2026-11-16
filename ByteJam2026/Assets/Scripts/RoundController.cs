using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    [SerializeField] private ZombieSpawner zombieSpawner;
    [SerializeField] private ZombieSpawner[] zombieSpawners;
    [SerializeField] private float timeBetweenRounds = 10f;
    [SerializeField] private TMP_Text zombiesLeftText;

    private int currentRound;
    private int zombiesCount = 10;
    private int zombiesAlive;
    private bool waitingForNextRound;

    private void Start()
    {
        if (zombieSpawners == null || zombieSpawners.Length == 0)
        {
            zombieSpawners = zombieSpawner != null
                ? new[] { zombieSpawner }
                : FindObjectsOfType<ZombieSpawner>();
        }
        if (zombieSpawners.Length == 0)
        {
            Debug.LogError("Rounds need at least one active ZombieSpawner.", this);
            enabled = false;
            return;
        }
        if (zombiesLeftText == null)
        {
            GameObject counter = GameObject.Find("ZombiesLeft");
            if (counter != null)
            {
                zombiesLeftText = counter.GetComponent<TMP_Text>();
            }
        }

        StartRound();
    }

    private void StartRound()
    {

        currentRound++;
        if (HighScoreManager.Instance != null)
        {
            HighScoreManager.Instance.UpdateRound(currentRound);
        }
        
        waitingForNextRound = false;
        bool spawnBoss = currentRound % 10 == 0;
        zombiesAlive = 0;
        int activeSpawners = 0;
        foreach (ZombieSpawner spawner in zombieSpawners)
        {
            if (spawner != null && spawner.isActiveAndEnabled) activeSpawners++;
        }
        if (activeSpawners == 0)
        {
            Debug.LogError("No active zombie spawners; rounds stopped.", this);
            enabled = false;
            return;
        }
        int index = 0;
        foreach (ZombieSpawner spawner in zombieSpawners)
        {
            if (spawner == null || !spawner.isActiveAndEnabled) continue;
            int count = zombiesCount / activeSpawners + (index < zombiesCount % activeSpawners ? 1 : 0);
            zombiesAlive += spawner.SpawnZombies(count, this, spawnBoss && index == 0);
            index++;
        }
        UpdateZombieCounter();

        if (zombiesAlive == 0)
        {
            Debug.LogError("No zombies spawned; check templates, terrain, and the baked NavMesh. Rounds stopped.", this);
            enabled = false;
            return;
        }

        Debug.Log($"Round {currentRound}: {zombiesAlive} zombies");
    }

    public void OnZombieDied()
    {
        if (waitingForNextRound || zombiesAlive <= 0)
        {
            return;
        }

        zombiesAlive--;
        UpdateZombieCounter();

        if (zombiesAlive == 0)
        {
            waitingForNextRound = true;
            StartCoroutine(StartNextRound());
        }
    }

    private void UpdateZombieCounter()
    {
        if (zombiesLeftText != null)
        {
            zombiesLeftText.text = waitingForNextRound
                ? $"Round {currentRound} complete! Next round in {timeBetweenRounds:0.#} seconds"
                : $"Round {currentRound} | Zombies = {zombiesAlive}";
        }
    }

    private IEnumerator StartNextRound()
    {
        SoundManager.Instance.Stopzombies();
        SoundManager.Instance.PlayRoundStart();
        UpdateZombieCounter();
        Debug.Log($"Round complete. Next round in {timeBetweenRounds} seconds.");
        yield return new WaitForSeconds(timeBetweenRounds);

        zombiesCount += 10;
        StartRound();
        SoundManager.Instance.PlayZombieSounds(); 
    }
}
