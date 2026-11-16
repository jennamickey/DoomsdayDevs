using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float Health;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float regenerationLimit = 100;
    [SerializeField] private GameObject deathScreenUI;

    void Start()
    {
        Health = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage){
        Health -= damage;
        
        if(Health <= 0){
            Health = 0;
            Die();
        }
        UpdateHealthUI();
    }

    public bool IncreaseMaxHealth(float amount)
    {
        if (Health >= 200)
        {
            return false;
        }

        maxHealth += amount;

        if (maxHealth > 250)
        {
            maxHealth = 250;
        }

        Health += amount;

        if (Health > maxHealth)
        {
            Health = maxHealth;
        }

        if (Health >= 200)
        {
            Health = 200;
        }

        UpdateHealthUI();

        return true;
    }

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + Health;
    }

    void Die()
    {
        Debug.Log("Player Has Died");

        if (HighScoreManager.Instance != null)
        {
            HighScoreManager.Instance.SaveHighScore();
        }

        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (HighScoreManager.Instance != null)
        {
            HighScoreManager.Instance.ResetCurrentGame();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
