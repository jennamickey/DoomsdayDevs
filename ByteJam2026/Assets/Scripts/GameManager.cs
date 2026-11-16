using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private float doublePointsDuration = 30f;
    private bool doublePointsActive = false;
    private float doublePointsTimer = 0f;

    public int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (doublePointsActive)
        {
            doublePointsTimer -= Time.deltaTime;

            if (doublePointsTimer <= 0)
            {
                doublePointsActive = false;
                Debug.Log("Double Points has ended!");
            }
        }
    }

    public void AddPoints(int amount)
    {
        if (doublePointsActive)
        {
            amount *= 2;
        }

        score += amount;
        Debug.Log("Score: " + score);
    }

    public bool SpendPoints(int amount)
    {
        if (score < amount)
        {
            Debug.Log("Not enough points!");
            return false;
        }

        score -= amount;
        Debug.Log("Score: " + score);

        return true;
    }

    public void ActivateDoublePoints()
    {
        doublePointsActive = true;
        doublePointsTimer = doublePointsDuration;

        Debug.Log("Double Points Activated!");
    }

    public bool IsDoublePointsActive()
    {
        return doublePointsActive;
    }
}