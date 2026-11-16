using UnityEngine;

public class VendingMachineSpeed : MonoBehaviour, IInteractable
{
    [Header("Vending Machine Settings")]
    [SerializeField] private string machineName = "Speed Up";
    [SerializeField] private int price = 2500;
    [SerializeField] private PlayerMovement playerMovement;

    public void Interact()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement not assigned!");
            return;
        }

        if (playerMovement.IsSpeedBoostActive())
        {
            Debug.Log("Speed Boost Already Active!");
            return;
        }

        if (GameManager.Instance.SpendPoints(price))
        {
            playerMovement.ActivateSpeedBoost();

            Debug.Log("Purchased " + machineName);
        }
    }

    public string GetPrompt()
    {
        if (playerMovement.IsSpeedBoostActive())
        {
            return "Speed Boost Active";
        }

        return "Press E to Buy " + machineName + " - " + price + " Points";
    }
}