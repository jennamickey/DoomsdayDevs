using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int maxHealth = 100;

    private int currentHealth;
    private bool isDead;
    private RoundController roundController;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Initialize(RoundController controller)
    {
        roundController = controller;
    }

    public void TakeDamage(int amount)
    {
        if (isDead || amount <= 0)
        {
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - amount);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddPoints(maxHealth);
        }
        if (roundController != null)
        {
            roundController.OnZombieDied();
        }

        Destroy(gameObject);
    }
}
