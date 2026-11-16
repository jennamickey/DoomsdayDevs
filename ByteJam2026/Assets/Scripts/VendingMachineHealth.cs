using UnityEngine;

public class VendingMachineHealth : MonoBehaviour, IInteractable
{
    [Header("Vending Machine Settings")]
    [SerializeField] private string machineName = "Extra Health";
    [SerializeField] private int price = 1000;
    [SerializeField] private PlayerHealth playerHealth;

    public void Interact()
    {
        Debug.Log("Attempting to purchase " + machineName);

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth not assigned!");
            return;
        }

        if (GameManager.Instance.score < price)
        {
            Debug.Log("Not enough points!");
            return;
        }

        if (GameManager.Instance.SpendPoints(price))
        {
            playerHealth.IncreaseMaxHealth(50);

            Debug.Log("Purchased " + machineName);
        }
    }

    public string GetPrompt()
    {
        return "Press E to Buy " + machineName + " - " + price + " Points";
    }
}