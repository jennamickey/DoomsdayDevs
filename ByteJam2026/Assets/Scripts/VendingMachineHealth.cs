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
        
        playerHealth.IncreaseMaxHealth(50);

        Debug.Log("Purchased " + machineName);
    }

    public string GetPrompt()
    {
        return "Press E to Buy " + machineName + " - " + price + " Points";
    }
}