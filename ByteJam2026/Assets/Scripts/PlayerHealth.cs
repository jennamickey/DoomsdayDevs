using UnityEngine;
using TMPro;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float Health;
    [SerializeField] private TMP_Text healthText;

    void Start()
    {
        Health = 100;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage){
        Health -= damage;
        
        if(Health < 0){
            Health = 0;
            Debug.Log("Player Has Died");
        }
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + Health;
    }
}
