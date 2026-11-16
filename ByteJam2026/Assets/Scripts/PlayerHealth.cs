using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float Health;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject deathScreenUI;

    void Start()
    {
        Health = 100;
        Time.timeScale = 1f;
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

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + Health;
    }

    void Die()
    {
        Debug.Log("Player Has Died");

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
